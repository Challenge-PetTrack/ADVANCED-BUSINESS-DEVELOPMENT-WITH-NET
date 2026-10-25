namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Modelo de requisição para autenticação na API.
/// </summary>
/// <param name="Email">E-mail do usuário cadastrado.</param>
/// <param name="Password">Senha do usuário.</param>
public record LoginRequest(string Email, string Password);
