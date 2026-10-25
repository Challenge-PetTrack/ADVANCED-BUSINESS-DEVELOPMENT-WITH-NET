using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Services;

/// <summary>
/// Contrato do serviço de aplicação para operações NoSQL com MongoDB.
/// </summary>
public interface IPetMongoService
{
    Task<IReadOnlyList<PetMongoResponse>> GetAllAsync();
    Task<PetMongoResponse?> GetByIdAsync(long id);
    Task<IReadOnlyList<PetMongoResponse>> GetByTutorIdAsync(long tutorId);
    Task<IReadOnlyList<PetMongoResponse>> GetByClinicaIdAsync(long clinicaId);
    Task<PetMongoResponse> CreateAsync(PetMongoRequest request);
    Task<PetMongoResponse> UpdateAsync(long id, PetMongoRequest request);
    Task<bool> DeleteAsync(long id);
    Task<long> CountAsync();
    Task<int> SyncRelationalToMongoAsync();
}

/// <summary>
/// Implementação do serviço de aplicação NoSQL para Pets no MongoDB.
/// </summary>
public class PetMongoService : IPetMongoService
{
    private readonly IPetMongoRepository _mongoRepository;
    private readonly IPetRepository _relationalRepository;

    public PetMongoService(IPetMongoRepository mongoRepository, IPetRepository relationalRepository)
    {
        _mongoRepository = mongoRepository;
        _relationalRepository = relationalRepository;
    }

    public async Task<IReadOnlyList<PetMongoResponse>> GetAllAsync()
    {
        var docs = await _mongoRepository.GetAllAsync();
        return docs.Select(PetMongoResponse.ToDTO).ToList();
    }

    public async Task<PetMongoResponse?> GetByIdAsync(long id)
    {
        var doc = await _mongoRepository.GetByIdAsync(id);
        return doc is null ? null : PetMongoResponse.ToDTO(doc);
    }

    public async Task<IReadOnlyList<PetMongoResponse>> GetByTutorIdAsync(long tutorId)
    {
        var docs = await _mongoRepository.GetByTutorIdAsync(tutorId);
        return docs.Select(PetMongoResponse.ToDTO).ToList();
    }

    public async Task<IReadOnlyList<PetMongoResponse>> GetByClinicaIdAsync(long clinicaId)
    {
        var docs = await _mongoRepository.GetByClinicaIdAsync(clinicaId);
        return docs.Select(PetMongoResponse.ToDTO).ToList();
    }

    public async Task<PetMongoResponse> CreateAsync(PetMongoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome do pet é obrigatório.", nameof(request));

        if (string.IsNullOrWhiteSpace(request.Especie))
            throw new ArgumentException("Espécie do pet é obrigatória.", nameof(request));

        var doc = new PetMongoDocument
        {
            Nome = request.Nome.Trim(),
            Especie = request.Especie.Trim(),
            Raca = request.Raca?.Trim(),
            PesoKg = request.PesoKg,
            Tutor = new PetMongoTutor { Id = request.TutorId, Nome = request.TutorNome },
            Clinica = new PetMongoClinica { Id = request.ClinicaId, Nome = request.ClinicaNome }
        };

        await _mongoRepository.InsertAsync(doc);
        return PetMongoResponse.ToDTO(doc);
    }

    public async Task<PetMongoResponse> UpdateAsync(long id, PetMongoRequest request)
    {
        var existing = await _mongoRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Documento de pet com id '{id}' não encontrado no MongoDB.");

        existing.Nome = request.Nome.Trim();
        existing.Especie = request.Especie.Trim();
        existing.Raca = request.Raca?.Trim();
        existing.PesoKg = request.PesoKg;
        existing.Tutor = new PetMongoTutor { Id = request.TutorId, Nome = request.TutorNome };
        existing.Clinica = new PetMongoClinica { Id = request.ClinicaId, Nome = request.ClinicaNome };

        await _mongoRepository.UpdateAsync(id, existing);
        return PetMongoResponse.ToDTO(existing);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await _mongoRepository.DeleteAsync(id);
    }

    public async Task<long> CountAsync()
    {
        return await _mongoRepository.CountAsync();
    }

    public async Task<int> SyncRelationalToMongoAsync()
    {
        var petsRelacionais = _relationalRepository.GetAll();
        var sincronizados = 0;

        foreach (var p in petsRelacionais)
        {
            var doc = new PetMongoDocument
            {
                Id = p.Id,
                IdPet = p.Id,
                Nome = p.Nome,
                Especie = p.Especie,
                Raca = p.Raca,
                PesoKg = p.Peso,
                Tutor = new PetMongoTutor { Id = p.Tutor?.Id ?? 0, Nome = p.Tutor?.Nome ?? string.Empty },
                Clinica = new PetMongoClinica { Id = p.Clinica?.Id ?? 0, Nome = p.Clinica?.Nome ?? string.Empty },
                DataAtualizacao = DateTime.UtcNow
            };

            var existing = await _mongoRepository.GetByIdAsync(p.Id);
            if (existing is null)
            {
                await _mongoRepository.InsertAsync(doc);
            }
            else
            {
                await _mongoRepository.UpdateAsync(p.Id, doc);
            }

            sincronizados++;
        }

        return sincronizados;
    }
}
