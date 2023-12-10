using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Testing.IntegrationTests;


    [TestFixture, Ignore("Not working yet")]
    public class ControllerIntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            // Initialize your services and controllers here if needed

            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Api.Program>()); // Assuming your Program class is in the Api namespace
            _client = _server.CreateClient();
        }

        [Test]
        public async Task RequestToUsersEndpoint_CallsUserController()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/users");
            request.Content = new StringContent("{ \"username\": \"testuser\", \"password\": \"testpassword\" }", Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            // Add more assertions if needed
        }

        [Test]
        public async Task RequestToPackagesEndpoint_CallsPackageController()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/packages");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Add more assertions if needed
        }

        // Add more integration tests for other endpoints

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
