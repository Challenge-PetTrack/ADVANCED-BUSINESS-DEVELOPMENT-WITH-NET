using PetTrack.Application.DTO.Common;
using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Modelo de resposta para documento NoSQL de Pet recuperado do MongoDB com suporte a HATEOAS.
/// </summary>
public record PetMongoResponse(
    long Id,
    long IdPet,
    string Nome,
    string Especie,
    string? Raca,
    double? PesoKg,
    PetMongoTutor Tutor,
    PetMongoClinica Clinica,
    DateTime? DataAtualizacao)
{
    /// <summary>Links hipermidiáticos HATEOAS.</summary>
    public List<LinkDto> Links { get; set; } = [];

    /// <summary>Converte o documento BSON/Mongo para DTO com links HATEOAS.</summary>
    public static PetMongoResponse ToDTO(PetMongoDocument doc)
    {
        var resp = new PetMongoResponse(
            doc.Id,
            doc.IdPet,
            doc.Nome,
            doc.Especie,
            doc.Raca,
            doc.PesoKg,
            doc.Tutor,
            doc.Clinica,
            doc.DataAtualizacao);

        resp.Links =
        [
            new LinkDto($"/api/mongopets/{doc.Id}", "self", "GET"),
            new LinkDto($"/api/mongopets/{doc.Id}", "update", "PUT"),
            new LinkDto($"/api/mongopets/{doc.Id}", "delete", "DELETE"),
            new LinkDto("/api/mongopets", "all", "GET"),
            new LinkDto("/api/pets", "relational_equivalent", "GET")
        ];

        return resp;
    }
}
