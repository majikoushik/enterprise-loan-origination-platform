using System;
using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;
using Auditing;
using Eligibility.Api.Application.Services;
using Eligibility.Api.Application.Validators;
using Eligibility.Api.Domain.Rules;
using Eligibility.Api.Infrastructure.Data;
using Eligibility.Api.Infrastructure.Integration;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddStructuredLogging("Eligibility.Api");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCorrelationIdSupport();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var useSyntheticDataStore = ShouldUseSyntheticDataStore(builder.Configuration, builder.Environment, connectionString);
builder.Services.AddDbContext<EligibilityDbContext>(options =>
{
    if (useSyntheticDataStore)
    {
        options.UseInMemoryDatabase("Synthetic_EligibilityDb");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddValidatorsFromAssemblyContaining<EvaluateEligibilityRequestValidator>();

builder.Services.AddHttpAuditLogging(builder.Configuration);

builder.Services.AddScoped<IEligibilityRule, MinimumIncomeRule>();
builder.Services.AddScoped<IEligibilityRule, DebtToIncomeRule>();
builder.Services.AddScoped<IEligibilityRule, MaximumAmountRule>();
builder.Services.AddScoped<IEligibilityRule, TenureRule>();
builder.Services.AddScoped<IEligibilityRule, EmiObligationRule>();
builder.Services.AddScoped<IRuleEngine, RuleEngine>();

// Add Services

// Add HttpClient for LoanApplication API integration
if (useSyntheticDataStore)
{
    builder.Services.AddScoped<ILoanApplicationClient, SyntheticLoanApplicationClient>();
}
else
{
    var loanAppApiUrl = builder.Configuration["ServiceUrls:LoanApplicationApi"] 
        ?? throw new InvalidOperationException("LoanApplicationApi URL not configured.");

    builder.Services.AddHttpClient<ILoanApplicationClient, HttpLoanApplicationClient>(client =>
    {
        client.BaseAddress = new Uri(loanAppApiUrl);
    });
}

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eligibility API",
        Version = "v1",
        Description = "Evaluates loan applications against risk and eligibility rules."
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? Array.Empty<string>();

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var healthChecks = builder.Services.AddStandardHealthChecks();
if (useSyntheticDataStore)
{
    healthChecks.AddCheck("SyntheticEligibilityDb", () => HealthCheckResult.Healthy("Using synthetic in-memory eligibility data because SQL Server is unavailable."), tags: ["ready"]);
}
else
{
    healthChecks.AddSqlServer(connectionString, name: "EligibilityDb", tags: ["ready"]);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EligibilityDbContext>();
    if (useSyntheticDataStore)
    {
        await dbContext.Database.EnsureCreatedAsync();
        await SyntheticEligibilityDataSeeder.SeedAsync(dbContext);
    }
    else
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCorrelationId();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapStandardHealthChecks();
app.MapGet("/api/v1/eligibility-service/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("Eligibility.Api", "Rule-based eligibility evaluation service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetEligibilityServiceMetadata");

app.Run();

static bool ShouldUseSyntheticDataStore(IConfiguration configuration, IHostEnvironment environment, string connectionString)
{
    var fallbackEnabled = configuration.GetValue<bool?>("DataStore:UseSyntheticFallbackWhenSqlUnavailable")
        ?? environment.IsDevelopment();

    return fallbackEnabled && !CanOpenSqlConnection(connectionString);
}

static bool CanOpenSqlConnection(string connectionString)
{
    try
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        builder.ConnectTimeout = builder.ConnectTimeout <= 0 ? 2 : Math.Min(builder.ConnectTimeout, 2);

        using var connection = new SqlConnection(builder.ConnectionString);
        connection.Open();
        return true;
    }
    catch
    {
        return false;
    }
}

internal sealed record ServiceMetadata(string ServiceName, string Responsibility, string ApiVersion);
