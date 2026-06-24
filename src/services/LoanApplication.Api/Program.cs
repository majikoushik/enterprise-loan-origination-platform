using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;
using Messaging;
using Auditing;
using LoanApplication.Api.Infrastructure.Data;
using LoanApplication.Api.Application.Services;
using LoanApplication.Api.Application.Validators;
using LoanApplication.Api.Infrastructure.Integration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddStructuredLogging("LoanApplication.Api");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCorrelationIdSupport();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var useSyntheticDataStore = ShouldUseSyntheticDataStore(builder.Configuration, builder.Environment, connectionString);
builder.Services.AddDbContext<LoanApplicationDbContext>(options =>
{
    if (useSyntheticDataStore)
    {
        options.UseInMemoryDatabase("Synthetic_LoanApplicationDb");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

// Add Application Services
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
builder.Services.AddValidatorsFromAssemblyContaining<LoanApplicationRequestValidator>();

if (useSyntheticDataStore)
{
#pragma warning disable CS0618
    builder.Services.AddScoped<ICustomerLookupService, StubCustomerLookupService>();
#pragma warning restore CS0618
}
else
{
    var customerApiUrl = builder.Configuration["ServiceUrls:CustomerApi"]
        ?? throw new InvalidOperationException("CustomerApi URL not configured.");
    builder.Services.AddHttpClient<ICustomerLookupService, HttpCustomerLookupService>(client =>
    {
        client.BaseAddress = new Uri(customerApiUrl);
    });
}

// Configure HTTP Event Publisher for MVP simulation of Azure Service Bus
builder.Services.AddHttpClient<IMessagePublisher, HttpEventPublisher>(client =>
{
    // Configuration happens in the factory below
});
builder.Services.AddScoped<IMessagePublisher>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(nameof(IMessagePublisher));
    var configuration = sp.GetRequiredService<IConfiguration>();
    var webhookUrl = configuration["ServiceUrls:NotificationWorker"] ?? "http://localhost:5004/api/v1/internal/events";
    return new HttpEventPublisher(httpClient, webhookUrl);
});

builder.Services.AddHttpAuditLogging(builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Loan Application API",
        Version = "v1",
        Description = "Owns loan application submission, lifecycle, and status transition APIs."
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
    healthChecks.AddCheck("SyntheticLoanApplicationDb", () => HealthCheckResult.Healthy("Using synthetic in-memory loan application data because SQL Server is unavailable."), tags: ["ready"]);
}
else
{
    healthChecks.AddSqlServer(connectionString, name: "LoanApplicationDb", tags: ["ready"]);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LoanApplicationDbContext>();
    if (useSyntheticDataStore)
    {
        await dbContext.Database.EnsureCreatedAsync();
        await SyntheticLoanApplicationDataSeeder.SeedAsync(dbContext);
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
app.MapGet("/api/v1/loan-application-service/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("LoanApplication.Api", "Loan application lifecycle service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetLoanApplicationServiceMetadata");

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
