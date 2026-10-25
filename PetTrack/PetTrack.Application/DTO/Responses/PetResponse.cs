using PetTrack.Application.DTO.Common;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Pet e links HATEOAS.
/// </summary>
/// <param name="Id">Identificador único do Pet gerado pelo Oracle.</param>
/// <param name="Nome">Nome do pet.</param>
/// <param name="Especie">Espécie do pet.</param>
/// <param name="Raca">Raça do pet. Opcional.</param>
/// <param name="Sexo">Sexo do pet: M ou F.</param>
/// <param name="Idade">Idade do pet em anos.</param>
/// <param name="Peso">Peso do pet em kg.</param>
/// <param name="DataCadastro">Data de cadastro gerada automaticamente pelo Oracle.</param>
/// <param name="Tutor">Dados do tutor responsável.</param>
/// <param name="Clinica">Dados da clínica vinculada.</param>
public record PetResponse(
    long Id,
    string Nome,
    string Especie,
    string? Raca,
    SexoPetEnum Sexo,
    double Idade,
    double Peso,
    DateTime DataCadastro,
    TutorResponse Tutor,
    ClinicaResponse Clinica)
{
    /// <summary>Links hipermidiáticos HATEOAS do recurso.</summary>
    public List<LinkDto> Links { get; set; } = [];

    /// <summary>Converte a entidade Pet para o DTO de resposta.</summary>
    public static PetResponse ToDTO(Pet pet) => new(
        pet.Id,
        pet.Nome,
        pet.Especie,
        pet.Raca,
        pet.Sexo,
        pet.Idade,
        pet.Peso,
        pet.DataCadastro,
        pet.Tutor != null ? TutorResponse.ToDTO(pet.Tutor) : null!,
        pet.Clinica != null ? ClinicaResponse.ToDTO(pet.Clinica) : null!);
}