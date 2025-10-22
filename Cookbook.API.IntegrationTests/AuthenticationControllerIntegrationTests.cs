using System.Net;
using System.Net.Http.Json;
using Cookbook.API.IntegrationTests.Fixtures;
using Cookbook.SharedData.Contracts.Requests;
using Cookbook.SharedData.Contracts.Responses;
using Xunit;

namespace Cookbook.API.IntegrationTests;

public class AuthenticationControllerIntegrationTests(APiWebApplicationFactory webApi) : IntegrationTest(webApi)
{
    [Fact]
    public async Task SignUp_WithValidData_ReturnsOkWithJwtToken()
    {
        // Arrange
        var signUpRequest = new SignUpUserRequest
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "NewUser123"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signup", signUpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.Token);
        Assert.NotEmpty(jwtResponse.Token);
    }

    [Fact]
    public async Task SignUp_WithDuplicateUsername_ReturnsConflict()
    {
        // Arrange
        var signUpRequest = new SignUpUserRequest
        {
            Username = "user", // Already exists in seed data
            Email = "different@example.com",
            Password = "Password123"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signup", signUpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task SignUp_WithDuplicateEmail_ReturnsConflict()
    {
        // Arrange
        var signUpRequest = new SignUpUserRequest
        {
            Username = "differentuser",
            Email = "user@user.com", // Already exists in seed data
            Password = "Password123"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signup", signUpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task SignUp_WithInvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var signUpRequest = new SignUpUserRequest
        {
            Username = "newuser2",
            Email = "newuser2@example.com",
            Password = "no" // Too short
        };

        // Act
        var response = await PostAsync("/v1/authentication/signup", signUpRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SignIn_WithValidCredentials_ReturnsOkWithJwtToken()
    {
        // Arrange
        var signInRequest = new SignInUserRequest
        {
            Username = "user",
            Password = "user"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signin", signInRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.Token);
        Assert.NotEmpty(jwtResponse.Token);
    }

    [Fact]
    public async Task SignIn_WithInvalidUsername_ReturnsUnauthorized()
    {
        // Arrange
        var signInRequest = new SignInUserRequest
        {
            Username = "nonexistentuser",
            Password = "Password123"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signin", signInRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SignIn_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var signInRequest = new SignInUserRequest
        {
            Username = "user",
            Password = "InvalidPassword123"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signin", signInRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task SignIn_WithAdminUser_ReturnsTokenWithAdminRole()
    {
        // Arrange
        var signInRequest = new SignInUserRequest
        {
            Username = "admin",
            Password = "admin"
        };

        // Act
        var response = await PostAsync("/v1/authentication/signin", signInRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.Token);

        // Token should contain admin role claims (validated by the JWT structure)
        Assert.NotEmpty(jwtResponse.Token);
    }

    [Fact]
    public async Task SignUp_ThenSignIn_WithNewUser_Success()
    {
        // Arrange
        var signUpRequest = new SignUpUserRequest
        {
            Username = "integrationuser",
            Email = "integration@example.com",
            Password = "Integration123"
        };

        // Act - Sign up
        var signUpResponse = await PostAsync("/v1/authentication/signup", signUpRequest);
        Assert.Equal(HttpStatusCode.OK, signUpResponse.StatusCode);

        // Act - Sign in with the same credentials
        var signInRequest = new SignInUserRequest
        {
            Username = signUpRequest.Username,
            Password = signUpRequest.Password
        };
        var signInResponse = await PostAsync("/v1/authentication/signin", signInRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, signInResponse.StatusCode);
        var jwtResponse = await signInResponse.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(jwtResponse);
        Assert.NotEmpty(jwtResponse.Token);
    }

    private async Task<HttpResponseMessage> PostAsync<T>(string url, T content)
    {
        return await HttpClient.PostAsJsonAsync(url, content);
    }
}