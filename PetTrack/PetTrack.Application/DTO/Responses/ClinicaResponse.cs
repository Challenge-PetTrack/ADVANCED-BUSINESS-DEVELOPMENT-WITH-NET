using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados da Clínica.
/// </summary>
/// <param name="Id">Identificador único da Clínica gerado pelo Oracle.</param>
/// <param name="Nome">Nome da clínica veterinária.</param>
/// <param name="Cnpj">CNPJ da clínica.</param>
/// <param name="Email">Email de contato. Opcional.</param>
/// <param name="Telefone">Telefone de contato. Opcional.</param>
/// <param name="Endereco">Endereço da clínica. Opcional.</param>
/// <param name="DataCadastro">Data de cadastro gerada automaticamente pelo Oracle.</param>
public record ClinicaResponse(
    long Id,
    string Nome,
    string Cnpj,
    string? Email,
    string? Telefone,
    string? Endereco,
    DateTime DataCadastro)
{
    /// <summary>Converte a entidade Clinica para o DTO de resposta.</summary>
    public static ClinicaResponse ToDTO(Clinica clinica) => new(
        clinica.Id,
        clinica.Nome,
        clinica.Cnpj,
        clinica.Email,
        clinica.Telefone,
        clinica.Endereco,
        clinica.DataCadastro);
}