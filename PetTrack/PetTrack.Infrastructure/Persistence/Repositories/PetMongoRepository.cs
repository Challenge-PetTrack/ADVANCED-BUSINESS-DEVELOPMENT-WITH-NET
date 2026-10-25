using MongoDB.Driver;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Infrastructure.Persistence.Mongo;

namespace PetTrack.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementação concreta do repositório NoSQL de Pets utilizando o MongoDB C# Driver.
/// </summary>
public class PetMongoRepository : IPetMongoRepository
{
    private readonly PetTrackMongoContext _context;

    public PetMongoRepository(PetTrackMongoContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PetMongoDocument>> GetAllAsync()
    {
        return await _context.Pets.Find(_ => true).ToListAsync();
    }

    public async Task<PetMongoDocument?> GetByIdAsync(long id)
    {
        return await _context.Pets.Find(p => p.Id == id || p.IdPet == id).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<PetMongoDocument>> GetByTutorIdAsync(long tutorId)
    {
        return await _context.Pets.Find(p => p.Tutor.Id == tutorId).ToListAsync();
    }

    public async Task<IReadOnlyList<PetMongoDocument>> GetByClinicaIdAsync(long clinicaId)
    {
        return await _context.Pets.Find(p => p.Clinica.Id == clinicaId).ToListAsync();
    }

    public async Task InsertAsync(PetMongoDocument document)
    {
        if (document.Id <= 0)
        {
            var count = await _context.Pets.CountDocumentsAsync(_ => true);
            document.Id = count + 1;
            document.IdPet = document.Id;
        }

        document.DataAtualizacao = DateTime.UtcNow;
        await _context.Pets.InsertOneAsync(document);
    }

    public async Task<bool> UpdateAsync(long id, PetMongoDocument document)
    {
        document.Id = id;
        document.IdPet = id;
        document.DataAtualizacao = DateTime.UtcNow;

        var result = await _context.Pets.ReplaceOneAsync(
            p => p.Id == id || p.IdPet == id,
            document,
            new ReplaceOptions { IsUpsert = false });

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var result = await _context.Pets.DeleteOneAsync(p => p.Id == id || p.IdPet == id);
        return result.DeletedCount > 0;
    }

    public async Task<long> CountAsync()
    {
        return await _context.Pets.CountDocumentsAsync(_ => true);
    }
}
