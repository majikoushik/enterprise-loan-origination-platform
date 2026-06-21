using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eligibility API",
        Version = "v1",
        Description = "Hosts rule-based eligibility evaluation APIs for demo credit decisions."
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
