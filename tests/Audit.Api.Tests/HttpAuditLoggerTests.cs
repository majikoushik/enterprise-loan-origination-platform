using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Auditing;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace Audit.Api.Tests;

public class HttpAuditLoggerTests
{
    [Fact]
    public async Task LogAsync_Success_ShouldNotThrow()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            })
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost:5005/")
        };

        var loggerMock = new Mock<ILogger<HttpAuditLogger>>();
        var auditLogger = new HttpAuditLogger(httpClient, "api/v1/audit/events", loggerMock.Object);

        var record = new AuditEventRecord(Guid.NewGuid(), "123", "Test", "Cat", "Type", "1", null, "Actor", "1", "Act", "Sum", "{}", DateTimeOffset.UtcNow, "Src", "Info");

        // Act
        var exception = await Record.ExceptionAsync(() => auditLogger.LogAsync(record, CancellationToken.None));

        // Assert
        Assert.Null(exception);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri.ToString() == "http://localhost:5005/api/v1/audit/events"),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
