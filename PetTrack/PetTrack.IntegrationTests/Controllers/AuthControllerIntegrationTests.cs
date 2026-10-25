using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para o AuthController validando o fluxo de autenticação e proteção JWT.
/// </summary>
public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornar200Ok_ETokenJwt()
    {
        // Arrange
        var request = new LoginRequest("admin@pettrack.com", "admin123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        loginResponse.Should().NotBeNull();
        loginResponse!.Token.Should().NotBeNullOrWhiteSpace();
        loginResponse.TokenType.Should().Be("Bearer");
        loginResponse.Role.Should().Be("Admin");
        loginResponse.Email.Should().Be("admin@pettrack.com");
    }

    [Fact]
    public async Task Login_ComCredenciaisInvalidas_DeveRetornar401Unauthorized()
    {
        // Arrange
        var request = new LoginRequest("usuario@inexistente.com", "senhaerrada");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetMe_ComTokenValido_DeveRetornar200Ok_EDadosDoUsuario()
    {
        // Arrange
        var authClient = _factory.CreateAuthenticatedClient("vet@clyvovet.com", "Dr. Clyvo", "Veterinario");

        // Act
        var response = await authClient.GetAsync("/api/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("vet@clyvovet.com");
        content.Should().Contain("Veterinario");
    }

    [Fact]
    public async Task GetMe_SemToken_DeveRetornar401Unauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
