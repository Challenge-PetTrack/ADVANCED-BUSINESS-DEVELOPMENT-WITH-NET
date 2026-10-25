namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Modelo de resposta para autenticação JWT bem-sucedida.
/// </summary>
/// <param name="Token">Token JWT assinado.</param>
/// <param name="TokenType">Tipo do token (sempre 'Bearer').</param>
/// <param name="ExpiresInSeconds">Tempo de expiração do token em segundos.</param>
/// <param name="Email">E-mail do usuário autenticado.</param>
/// <param name="Name">Nome do usuário autenticado.</param>
/// <param name="Role">Perfil de acesso do usuário (ex: Admin, Veterinario, Tutor).</param>
public record LoginResponse(
    string Token,
    string TokenType,
    int ExpiresInSeconds,
    string Email,
    string Name,
    string Role);
