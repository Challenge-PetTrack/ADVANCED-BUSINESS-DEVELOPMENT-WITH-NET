using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using PetTrack.Application.DTO.Requests;
using PetTrack.Infrastructure.Services;
using Xunit;

namespace PetTrack.UnitTests.Services;

/// <summary>
/// Testes unitários para o serviço de autenticação e geração de tokens JWT (padrão AAA com Moq).
/// </summary>
public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Jwt:Key"]).Returns("SuperSecretUnitTestKey123456789012345678901234567890!");
        configMock.Setup(c => c["Jwt:Issuer"]).Returns("PetTrack.UnitTests");
        configMock.Setup(c => c["Jwt:Audience"]).Returns("PetTrack.Tests");
        configMock.Setup(c => c["Jwt:ExpirationInMinutes"]).Returns("60");

        _tokenService = new TokenService(configMock.Object);
    }

    [Fact]
    public void Authenticate_ComCredenciaisValidas_DeveRetornarLoginResponseComToken()
    {
        // Arrange
        var request = new LoginRequest("admin@pettrack.com", "admin123");

        // Act
        var response = _tokenService.Authenticate(request);

        // Assert
        response.Should().NotBeNull();
        response!.Token.Should().NotBeNullOrWhiteSpace();
        response.TokenType.Should().Be("Bearer");
        response.Role.Should().Be("Admin");
        response.Email.Should().Be("admin@pettrack.com");
        response.ExpiresInSeconds.Should().Be(3600);
    }

    [Theory]
    [InlineData("admin@pettrack.com", "senhaIncorreta")]
    [InlineData("naoexiste@pettrack.com", "admin123")]
    [InlineData("usuario@qualquer.com", "qualquer")]
    public void Authenticate_ComCredenciaisInvalidas_DeveRetornarNull(string email, string password)
    {
        // Arrange
        var request = new LoginRequest(email, password);

        // Act
        var response = _tokenService.Authenticate(request);

        // Assert
        response.Should().BeNull();
    }

    [Theory]
    [InlineData("", "admin123")]
    [InlineData("admin@pettrack.com", "")]
    [InlineData("   ", "   ")]
    public void Authenticate_ComCamposVazios_DeveRetornarNull(string email, string password)
    {
        // Arrange
        var request = new LoginRequest(email, password);

        // Act
        var response = _tokenService.Authenticate(request);

        // Assert
        response.Should().BeNull();
    }

    [Fact]
    public void GenerateToken_DeveGerarJwtValidoComClaimsCorretas()
    {
        // Arrange
        var email = "vet@clyvovet.com";
        var name = "Dr. Clyvo";
        var role = "Veterinario";

        // Act
        var tokenString = _tokenService.GenerateToken(email, name, role);

        // Assert
        tokenString.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenString);

        jwtToken.Issuer.Should().Be("PetTrack.UnitTests");
        jwtToken.Audiences.Should().Contain("PetTrack.Tests");
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == email);
        jwtToken.Claims.Should().Contain(c => (c.Type == "role" || c.Type == ClaimTypes.Role) && c.Value == role);
    }
}
