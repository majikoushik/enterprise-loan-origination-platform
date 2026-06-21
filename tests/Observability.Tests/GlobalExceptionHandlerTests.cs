using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Exceptions;
using Xunit;

namespace Observability.Tests;

public class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task TryHandleAsync_ValidationException_ReturnsProblemDetails()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<GlobalExceptionHandler>>();
        var mockCorrelationIdProvider = new Mock<CorrelationIdProvider>(new Mock<IHttpContextAccessor>().Object);
        mockCorrelationIdProvider.Setup(c => c.Get()).Returns("test-correlation-id");

        var handler = new GlobalExceptionHandler(mockLogger.Object, mockCorrelationIdProvider.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var exception = new ValidationException(new Dictionary<string, string[]> { { "Field", new[] { "Error" } } });

        // Act
        var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(400, context.Response.StatusCode);
    }
}
