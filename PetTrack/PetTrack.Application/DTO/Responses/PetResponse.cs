using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Pet.
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
        TutorResponse.ToDTO(pet.Tutor),
        ClinicaResponse.ToDTO(pet.Clinica));
}