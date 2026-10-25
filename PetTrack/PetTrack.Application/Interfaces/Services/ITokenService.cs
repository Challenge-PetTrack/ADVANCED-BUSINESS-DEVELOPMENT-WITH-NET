using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;

namespace PetTrack.Application.Interfaces.Services;

/// <summary>
/// Contrato do serviço de geração e validação de tokens JWT.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Autentica o usuário a partir de credenciais e retorna o token JWT.
    /// </summary>
    LoginResponse? Authenticate(LoginRequest request);

    /// <summary>
    /// Gera um token JWT assinado para um usuário e role específicos.
    /// </summary>
    string GenerateToken(string email, string name, string role);
}
