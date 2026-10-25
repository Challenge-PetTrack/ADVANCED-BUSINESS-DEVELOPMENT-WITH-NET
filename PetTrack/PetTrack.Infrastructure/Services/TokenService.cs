using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.Infrastructure.Services;

/// <summary>
/// Serviço de geração e autenticação de tokens JWT para o ecossistema PetTrack.
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    // Usuários padrão para autenticação no ambiente acadêmico / avaliação do Challenge
    private static readonly Dictionary<string, (string Password, string Name, string Role)> Users = new(StringComparer.OrdinalIgnoreCase)
    {
        ["admin@pettrack.com"] = ("admin123", "Administrador PetTrack", "Admin"),
        ["vet@clyvovet.com"] = ("vet123", "Dr. Clyvo Veterinário", "Veterinario"),
        ["tutor@pettrack.com"] = ("tutor123", "Moisés Waidemann", "Tutor")
    };

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LoginResponse? Authenticate(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        if (Users.TryGetValue(request.Email.Trim(), out var userData) && userData.Password == request.Password)
        {
            var token = GenerateToken(request.Email.Trim(), userData.Name, userData.Role);
            var expMinutes = int.TryParse(_configuration["Jwt:ExpirationInMinutes"], out var parsedExp) ? parsedExp : 120;
            return new LoginResponse(
                token,
                "Bearer",
                expMinutes * 60,
                request.Email.Trim(),
                userData.Name,
                userData.Role);
        }

        return null;
    }

    public string GenerateToken(string email, string name, string role)
    {
        var secretKey = _configuration["Jwt:Key"]
            ?? "PetTrackSecretKey_Sprint4_FIAP2026_ClyvoVet_JWT_Token_Super_Secure_Key_LongEnough!";
        var issuer = _configuration["Jwt:Issuer"] ?? "PetTrack.API";
        var audience = _configuration["Jwt:Audience"] ?? "PetTrack.Client";
        var expirationMinutes = int.TryParse(_configuration["Jwt:ExpirationInMinutes"], out var parsedExp) ? parsedExp : 120;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
