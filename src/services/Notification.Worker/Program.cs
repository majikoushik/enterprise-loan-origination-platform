using Observability;
using SharedKernel;
using Auditing;
using Notification.Worker.Infrastructure.Data;
using Notification.Worker;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddStructuredLogging("Notification.Worker");

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCorrelationIdSupport();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddProblemDetails();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("NotificationDb") 
    ?? throw new InvalidOperationException("Connection string 'NotificationDb' not found.");
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddStandardHealthChecks()
    .AddSqlServer(connectionString, name: "NotificationDb", tags: ["ready"]);

builder.Services.AddHttpAuditLogging(builder.Configuration);

// Background Worker
builder.Services.AddHostedService<NotificationSimulationWorker>();

// CORS
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
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCorrelationId();
app.MapControllers();
app.MapStandardHealthChecks();
app.MapGet("/api/v1/notification-worker/metadata", (HttpContext context) =>
{
    var correlationId = context.Items[CorrelationIdOptions.HeaderName]?.ToString() ?? string.Empty;
    var metadata = new ServiceMetadata("Notification.Worker", "Notification delivery and simulation service", "v1");
    return Results.Ok(ApiResponse<ServiceMetadata>.Create(metadata, correlationId));
})
.WithName("GetNotificationWorkerMetadata");

app.Run();

internal sealed record ServiceMetadata(string ServiceName, string Responsibility, string ApiVersion);
