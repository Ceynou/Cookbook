using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Microsoft.Extensions.Configuration;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Cookbook.API.IntegrationTests.Fixtures;

public abstract class IntegrationTest(APiWebApplicationFactory webApi) : IClassFixture<APiWebApplicationFactory>
{
    protected HttpClient HttpClient { get; } = webApi.CreateClient();
    private IConfiguration Configuration { get; set; } = webApi.Configuration;

    protected async Task SignIn(string username, string password)
    {
        var httpResponse = await HttpClient.PostAsJsonAsync("/api/Authentication/signin",
            new SignInUserRequest
            {
                Username = username,
                Password = password
            });

        if (httpResponse.IsSuccessStatusCode)
        {
            var jwtDto = await httpResponse.Content.ReadFromJsonAsync<JwtResponse>();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtDto?.Token);
        }
        else
        {
            Assert.Fail($"Impossible to sign in with {username}, {password}");
        }
    }

    protected void SignOut()
    {
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }
}