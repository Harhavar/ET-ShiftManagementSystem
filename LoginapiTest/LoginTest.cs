using ET_ShiftManagementSystem.Models.Authmodel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;

namespace LoginapiTest
{
    public class LoginApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public LoginApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_Returns_Successful_Response()
        {
            // Arrange
            var loginDto = new LoginRequest
            {
                username = "testuser",
                password = "testpassword"
            };
            var json = JsonConvert.SerializeObject(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Auth/Login", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<LoginResponce>(responseString);
            Assert.True(responseData.IsSuccess);
            Assert.NotNull(responseData.Token);
        }
    }

}