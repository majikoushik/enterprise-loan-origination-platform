using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Observability.Tests;

public class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenCorrelationIdNotProvided_GeneratesNewId()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<CorrelationIdMiddleware>>();
        var context = new DefaultHttpContext();
        var isNextCalled = false;

        var middleware = new CorrelationIdMiddleware(innerContext =>
        {
            isNextCalled = true;
            return Task.CompletedTask;
        }, mockLogger.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(isNextCalled);
        Assert.True(context.Items.ContainsKey("X-Correlation-ID"));
        Assert.NotNull(context.Items["X-Correlation-ID"]);
        Assert.True(context.Response.Headers.ContainsKey("X-Correlation-ID"));
    }

    [Fact]
    public async Task InvokeAsync_WhenCorrelationIdProvided_PreservesId()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<CorrelationIdMiddleware>>();
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Correlation-ID"] = "provided-id-123";
        var isNextCalled = false;

        var middleware = new CorrelationIdMiddleware(innerContext =>
        {
            isNextCalled = true;
            return Task.CompletedTask;
        }, mockLogger.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(isNextCalled);
        Assert.Equal("provided-id-123", context.Items["X-Correlation-ID"]);
        Assert.Equal("provided-id-123", context.Response.Headers["X-Correlation-ID"]);
    }
}
