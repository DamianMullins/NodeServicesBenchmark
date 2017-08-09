using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NodeServicesBenchmark.Tests
{
    public class MiddlewareIntegrationTests : IClassFixture<TestFixture<Startup>>
    {
        public MiddlewareIntegrationTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        public HttpClient Client { get; }

        [Theory]
        [InlineData("GET")]
        [InlineData("HEAD")]
        [InlineData("POST")]
        public async Task AllMethods_RemovesServerHeader(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/");

            // Act
            var response = await Client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("Test response", content);
            Assert.False(response.Headers.Contains("Server"), "Should not contain server header");
        }
    }
}
