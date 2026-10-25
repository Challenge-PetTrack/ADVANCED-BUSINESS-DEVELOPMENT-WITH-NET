using PetTrack.Application.DTO.Common;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Parâmetros de filtro, ordenação e paginação para consulta de pets.
/// </summary>
public class PetFilterParams : PaginationParams
{
    /// <summary>Filtro por nome do pet (busca parcial case-insensitive).</summary>
    public string? Nome { get; set; }

    /// <summary>Filtro por espécie (ex: Canina, Felina).</summary>
    public string? Especie { get; set; }

    /// <summary>Filtro por raça.</summary>
    public string? Raca { get; set; }

    /// <summary>Filtro por sexo do pet (M ou F).</summary>
    public SexoPetEnum? Sexo { get; set; }

    /// <summary>Filtro por identificador da clínica.</summary>
    public long? ClinicaId { get; set; }

    /// <summary>Filtro por identificador do tutor.</summary>
    public long? TutorId { get; set; }
}
