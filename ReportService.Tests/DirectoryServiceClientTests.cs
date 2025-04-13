using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using ReportService.Services;
using ReportService.Services.Abstract;
using ReportService.DTOs.Person;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ReportService.Tests
{
    public class DirectoryServiceClientTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<ILogger<DirectoryServiceClient>> _loggerMock;
        private readonly DirectoryServiceClient _service;

        public DirectoryServiceClientTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _loggerMock = new Mock<ILogger<DirectoryServiceClient>>();
            _service = new DirectoryServiceClient(_httpClientFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ShouldReturnPersons_WhenApiReturnsSuccess()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"IsSuccessful\": true, \"Data\": [{\"ID\":\"d290f1ee-6c54-4b01-90e6-d701748f0851\", \"FirstName\": \"John\"}]}")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://example.com") // Set a valid BaseAddress
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _service.GetAllPersonsAsync();

            Assert.Single(result);
            Assert.Equal("John", result[0].FirstName);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ShouldReturnEmptyList_WhenApiReturnsFailure()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://example.com") 
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _service.GetAllPersonsAsync();

            Assert.Empty(result);
        }
    }
}