using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;
using LoanApplication.Api.Infrastructure.Data;
using LoanApplication.Api.Application.Services;
using LoanApplication.Api.Application.Validators;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<LoanApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Application Services
builder.Services.AddScoped<ILoanApplicationService, LoanApplicationService>();
builder.Services.AddScoped<ICustomerLookupService, StubCustomerLookupService>();
builder.Services.AddValidatorsFromAssemblyContaining<LoanApplicationRequestValidator>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Loan Application API",
        Version = "v1",
        Description = "Owns loan application submission, lifecycle, and status transition APIs."
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
app.MapGet("/api/v1/loan-application-service/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("LoanApplication.Api", "Loan application lifecycle service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetLoanApplicationServiceMetadata")
.WithOpenApi();

app.Run();

internal sealed record ServiceMetadata(string ServiceName, string Responsibility, string ApiVersion);
