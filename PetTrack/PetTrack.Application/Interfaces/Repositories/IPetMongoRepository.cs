using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

/// <summary>
/// Contrato do repositório NoSQL para documentos de pets no MongoDB.
/// Implementa o padrão Repository para desacoplar a camada de negócio da persistência de documentos.
/// </summary>
public interface IPetMongoRepository
{
    /// <summary>Retorna todos os documentos de pets armazenados no MongoDB.</summary>
    Task<IReadOnlyList<PetMongoDocument>> GetAllAsync();

    /// <summary>Busca um documento de pet pelo seu identificador primário (_id / idPet).</summary>
    Task<PetMongoDocument?> GetByIdAsync(long id);

    /// <summary>Busca documentos de pets pertencentes a um tutor específico.</summary>
    Task<IReadOnlyList<PetMongoDocument>> GetByTutorIdAsync(long tutorId);

    /// <summary>Busca documentos de pets atendidos por uma clínica específica.</summary>
    Task<IReadOnlyList<PetMongoDocument>> GetByClinicaIdAsync(long clinicaId);

    /// <summary>Insere um novo documento de pet no MongoDB.</summary>
    Task InsertAsync(PetMongoDocument document);

    /// <summary>Atualiza um documento de pet existente no MongoDB.</summary>
    Task<bool> UpdateAsync(long id, PetMongoDocument document);

    /// <summary>Remove um documento de pet do MongoDB pelo identificador.</summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>Retorna a contagem total de documentos na coleção.</summary>
    Task<long> CountAsync();
}
