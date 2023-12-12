using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Api.Tests
{
    [TestFixture, Ignore("Not working yet")]
    public class IntegrationTests : WebApplicationFactory<Program>
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new TestServer(new WebHostBuilder()
                    .UseStartup<Api.Program>())
                .CreateClient();

        }

        [Test]
        public void CreateUser_EndpointReturnsCreated()
        {
            // Arrange
            var userJson = "{ \"Username\": \"testuser\", \"Password\": \"testpassword\" }";
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");

            // Act
            var response = _client.PostAsync("/users", content).Result; // Synchronously wait for the result

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("text/html; charset=utf-8"));
        }
    }
}