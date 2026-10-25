namespace PetTrack.Application.DTO.Common;

/// <summary>
/// Representa um link hipermidiático no padrão HATEOAS.
/// </summary>
/// <param name="Href">URI relativa ou absoluta do recurso.</param>
/// <param name="Rel">Relação do link com o recurso atual (ex: self, update, delete, first, next, last).</param>
/// <param name="Method">Método HTTP aceito (GET, POST, PUT, DELETE).</param>
public record LinkDto(string Href, string Rel, string Method);
