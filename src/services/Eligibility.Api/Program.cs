using System;
using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;
using Eligibility.Api.Application.Services;
using Eligibility.Api.Domain.Rules;
using Eligibility.Api.Infrastructure.Data;
using Eligibility.Api.Infrastructure.Integration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<EligibilityDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Rules Engine
builder.Services.AddScoped<IEligibilityRule, MinimumIncomeRule>();
builder.Services.AddScoped<IEligibilityRule, DebtToIncomeRule>();
builder.Services.AddScoped<IEligibilityRule, MaximumAmountRule>();
builder.Services.AddScoped<IEligibilityRule, TenureRule>();
builder.Services.AddScoped<IEligibilityRule, EmiObligationRule>();
builder.Services.AddScoped<RuleEngine>();

// Add Services
builder.Services.AddScoped<IEligibilityService, EligibilityService>();

// Add HttpClient for LoanApplication API integration
var loanAppApiUrl = builder.Configuration["ServiceUrls:LoanApplicationApi"] 
    ?? throw new InvalidOperationException("LoanApplicationApi URL not configured.");

builder.Services.AddHttpClient<ILoanApplicationClient, HttpLoanApplicationClient>(client =>
{
    client.BaseAddress = new Uri(loanAppApiUrl);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eligibility API",
        Version = "v1",
        Description = "Evaluates loan applications against risk and eligibility rules."
    });
});
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCorrelationId();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/api/v1/eligibility-service/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("Eligibility.Api", "Rule-based eligibility evaluation service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetEligibilityServiceMetadata")
.WithOpenApi();

app.Run();

internal sealed record ServiceMetadata(string ServiceName, string Responsibility, string ApiVersion);
