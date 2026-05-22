using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Tutor.
/// </summary>
/// <param name="Nome">Nome do Tutor.</param>
/// <param name="Email">Email do Tutor.</param>
/// <param name="Telefone">Telefone do Tutor.</param>
/// <param name="Endereco">Endereço do Tutor.</param>
public record TutorRequest(string Nome, string Email, string? Telefone, string? Endereco)
{
    /// <summary>Converte o DTO para a entidade de domínio Tutor.</summary>
    public Tutor ToDomain() => new Tutor(Nome, Email, Telefone, Endereco);
}