using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Pet.
/// </summary>
/// <param name="Nome">Nome do pet.</param>
/// <param name="Especie">Espécie do pet (ex: Cão, Gato).</param>
/// <param name="Raca">Raça do pet. Opcional.</param>
/// <param name="Sexo">Sexo do pet: M ou F.</param>
/// <param name="Idade">Idade do pet em anos.</param>
/// <param name="Peso">Peso do pet em kg.</param>
/// <param name="Tutor">Tutor responsável.</param>
/// <param name="Clinica">Clínica vinculada.</param>
public record PetRequest(
    string Nome,
    string Especie,
    string? Raca,
    SexoPetEnum Sexo,
    double Idade,
    double Peso,
    Tutor Tutor,
    Clinica Clinica)
{
    /// <summary>Converte o DTO para a entidade de domínio Pet.</summary>
    public Pet ToEntity() => new Pet(Nome, Especie, Raca,  Sexo, Idade, Peso, Tutor, Clinica);
}