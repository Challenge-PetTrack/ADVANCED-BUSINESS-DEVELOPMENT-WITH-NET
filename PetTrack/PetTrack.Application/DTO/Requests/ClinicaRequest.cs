using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar uma Clínica.
/// </summary>
/// <param name="Nome">Nome da clínica veterinária.</param>
/// <param name="Cnpj">CNPJ da clínica. Único.</param>
/// <param name="Email">Email de contato. Opcional.</param>
/// <param name="Telefone">Telefone de contato. Opcional.</param>
/// <param name="Endereco">Endereço da clínica. Opcional.</param>
public record ClinicaRequest(
    string Nome,
    string Cnpj,
    string? Email,
    string? Telefone,
    string? Endereco)
{
    /// <summary>Converte o DTO para a entidade de domínio Clinica.</summary>
    public Clinica ToEntity() => new Clinica(Nome, Cnpj, Email, Telefone, Endereco);
}