using Audit.Api.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Observability;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddStructuredLogging("Audit.Api");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddCorrelationIdSupport();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Audit API",
        Version = "v1",
        Description = "Provides traceability APIs for business audit events."
    });
});

var connectionString = builder.Configuration.GetConnectionString("AuditDb")
    ?? throw new InvalidOperationException("Connection string 'AuditDb' not found.");
var useSyntheticDataStore = ShouldUseSyntheticDataStore(builder.Configuration, builder.Environment, connectionString);

builder.Services.AddDbContext<AuditDbContext>(options =>
{
    if (useSyntheticDataStore)
    {
        options.UseInMemoryDatabase("Synthetic_AuditDb");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

var healthChecks = builder.Services.AddStandardHealthChecks();
if (useSyntheticDataStore)
{
    healthChecks.AddCheck("SyntheticAuditDb", () => HealthCheckResult.Healthy("Using synthetic in-memory audit data because SQL Server is unavailable."), tags: ["ready"]);
}
else
{
    healthChecks.AddSqlServer(connectionString, name: "AuditDb", tags: ["ready"]);
}

builder.Services.AddHealthChecks();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
    if (useSyntheticDataStore)
    {
        await dbContext.Database.EnsureCreatedAsync();
        await SyntheticAuditDataSeeder.SeedAsync(dbContext);
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
app.MapGet("/api/v1/audit-service/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("Audit.Api", "Business audit trail service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetAuditServiceMetadata");

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
