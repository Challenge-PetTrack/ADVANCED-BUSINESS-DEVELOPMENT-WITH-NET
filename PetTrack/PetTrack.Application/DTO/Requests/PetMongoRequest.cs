namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Modelo de requisição para inserção ou atualização de Pet no MongoDB (NoSQL).
/// </summary>
public record PetMongoRequest(
    string Nome,
    string Especie,
    string? Raca,
    double? PesoKg,
    long TutorId,
    string TutorNome,
    long ClinicaId,
    string ClinicaNome);
