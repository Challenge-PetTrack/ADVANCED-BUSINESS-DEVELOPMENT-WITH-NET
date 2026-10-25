using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.IntegrationTests.Fixtures;

/// <summary>
/// Implementação em memória de IPetMongoRepository para isolamento completo da suíte de testes de integração.
/// </summary>
public class InMemoryPetMongoRepository : IPetMongoRepository
{
    private readonly List<PetMongoDocument> _docs;
    private readonly object _lock = new();

    public InMemoryPetMongoRepository()
    {
        _docs =
        [
            new PetMongoDocument
            {
                Id = 1,
                IdPet = 1,
                Nome = "Thor",
                Especie = "Cão",
                Raca = "Golden Retriever",
                PesoKg = 12.0,
                Tutor = new PetMongoTutor { Id = 3, Nome = "Mariana Oliveira" },
                Clinica = new PetMongoClinica { Id = 1, Nome = "VetLife Clínica Veterinária" }
            },
            new PetMongoDocument
            {
                Id = 2,
                IdPet = 2,
                Nome = "Luna",
                Especie = "Cão",
                Raca = "Poodle",
                PesoKg = 10.0,
                Tutor = new PetMongoTutor { Id = 2, Nome = "Carlos Eduardo Santos" },
                Clinica = new PetMongoClinica { Id = 1, Nome = "VetLife Clínica Veterinária" }
            },
            new PetMongoDocument
            {
                Id = 3,
                IdPet = 3,
                Nome = "Bob",
                Especie = "Cão",
                Raca = "Bulldog",
                PesoKg = 16.0,
                Tutor = new PetMongoTutor { Id = 1, Nome = "Ana Paula Silva" },
                Clinica = new PetMongoClinica { Id = 1, Nome = "VetLife Clínica Veterinária" }
            }
        ];
    }

    public Task<IReadOnlyList<PetMongoDocument>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IReadOnlyList<PetMongoDocument>>(_docs.ToList());
        }
    }

    public Task<PetMongoDocument?> GetByIdAsync(long id)
    {
        lock (_lock)
        {
            var doc = _docs.FirstOrDefault(p => p.Id == id || p.IdPet == id);
            return Task.FromResult(doc);
        }
    }

    public Task<IReadOnlyList<PetMongoDocument>> GetByTutorIdAsync(long tutorId)
    {
        lock (_lock)
        {
            var docs = _docs.Where(p => p.Tutor.Id == tutorId).ToList();
            return Task.FromResult<IReadOnlyList<PetMongoDocument>>(docs);
        }
    }

    public Task<IReadOnlyList<PetMongoDocument>> GetByClinicaIdAsync(long clinicaId)
    {
        lock (_lock)
        {
            var docs = _docs.Where(p => p.Clinica.Id == clinicaId).ToList();
            return Task.FromResult<IReadOnlyList<PetMongoDocument>>(docs);
        }
    }

    public Task InsertAsync(PetMongoDocument document)
    {
        lock (_lock)
        {
            if (document.Id <= 0)
            {
                document.Id = _docs.Count + 1;
                document.IdPet = document.Id;
            }
            document.DataAtualizacao = DateTime.UtcNow;
            _docs.Add(document);
            return Task.CompletedTask;
        }
    }

    public Task<bool> UpdateAsync(long id, PetMongoDocument document)
    {
        lock (_lock)
        {
            var index = _docs.FindIndex(p => p.Id == id || p.IdPet == id);
            if (index < 0) return Task.FromResult(false);

            document.Id = id;
            document.IdPet = id;
            _docs[index] = document;
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(long id)
    {
        lock (_lock)
        {
            var removed = _docs.RemoveAll(p => p.Id == id || p.IdPet == id);
            return Task.FromResult(removed > 0);
        }
    }

    public Task<long> CountAsync()
    {
        lock (_lock)
        {
            return Task.FromResult((long)_docs.Count);
        }
    }
}
