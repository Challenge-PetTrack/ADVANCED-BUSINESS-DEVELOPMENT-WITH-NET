using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Tutor.
/// </summary>
/// <param name="Id">Identificador único do Tutor gerado pelo Oracle.</param>
/// <param name="Nome">Nome completo do Tutor.</param>
/// <param name="Email">Email do Tutor.</param>
/// <param name="Telefone">Telefone do Tutor. Opcional.</param>
/// <param name="Endereco">Endereço do Tutor. Opcional.</param>
/// <param name="DataCadastro">Data de cadastro gerada automaticamente pelo Oracle.</param>
public record TutorResponse(
    long Id,
    string Nome,
    string Email,
    string? Telefone,
    string? Endereco,
    DateTime DataCadastro)
{
    /// <summary>Converte a entidade Tutor para o DTO de resposta.</summary>
    public static TutorResponse ToDTO(Tutor tutor) => new(
        tutor.Id,
        tutor.Nome,
        tutor.Email,
        tutor.Telefone,
        tutor.Endereco,
        tutor.DataCadastro);
}