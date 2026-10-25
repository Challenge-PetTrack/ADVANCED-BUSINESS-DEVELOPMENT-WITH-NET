using Microsoft.EntityFrameworkCore;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class PetRepository : IPetRepository
{
    private readonly PetTrackContext _context;

    public PetRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Pet> GetAll() =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .ToList();

    public Pet? GetById(long id) =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<Pet> GetByClinicaId(long idClinica) =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .Where(p => p.Clinica.Id == idClinica)
            .ToList();

    public IReadOnlyList<Pet> GetBySexo(SexoPetEnum sexo) =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .Where(p => p.Sexo == sexo)
            .ToList();

    public IReadOnlyList<Pet> GetByNomeOuEspecie(string busca) =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .Where(p => p.Nome.Contains(busca) || p.Especie.Contains(busca))
            .ToList();

    public IReadOnlyList<Pet> GetPetsComAlertasPendentes() =>
        _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .Where(p => p.Alertas.Any(a => a.Status == SimNaoEnum.N))
            .ToList();

    public void Add(Pet pet)
    {
        _context.Pets.Add(pet);
        _context.SaveChanges();
    }

    public void Update(Pet pet)
    {
        _context.Pets.Update(pet);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var pet = _context.Pets.FirstOrDefault(p => p.Id == id);
        if (pet == null) return false;
        _context.Pets.Remove(pet);
        _context.SaveChanges();
        return true;
    }

    public (IReadOnlyList<Pet> Items, int TotalCount) GetPaged(
        string? nome,
        string? especie,
        string? raca,
        SexoPetEnum? sexo,
        long? clinicaId,
        long? tutorId,
        string? sortBy,
        string? sortDirection,
        int pageNumber,
        int pageSize)
    {
        var query = _context.Pets
            .Include(p => p.Tutor)
            .Include(p => p.Clinica)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

        if (!string.IsNullOrWhiteSpace(especie))
            query = query.Where(p => p.Especie.ToLower().Contains(especie.ToLower()));

        if (!string.IsNullOrWhiteSpace(raca))
            query = query.Where(p => p.Raca != null && p.Raca.ToLower().Contains(raca.ToLower()));

        if (sexo.HasValue)
            query = query.Where(p => p.Sexo == sexo.Value);

        if (clinicaId.HasValue)
            query = query.Where(p => p.Clinica.Id == clinicaId.Value);

        if (tutorId.HasValue)
            query = query.Where(p => p.Tutor.Id == tutorId.Value);

        var isDesc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        query = (sortBy?.ToLower()) switch
        {
            "id" => isDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            "idade" => isDesc ? query.OrderByDescending(p => p.Idade) : query.OrderBy(p => p.Idade),
            "peso" => isDesc ? query.OrderByDescending(p => p.Peso) : query.OrderBy(p => p.Peso),
            "especie" => isDesc ? query.OrderByDescending(p => p.Especie) : query.OrderBy(p => p.Especie),
            _ => isDesc ? query.OrderByDescending(p => p.Nome) : query.OrderBy(p => p.Nome)
        };

        var total = query.Count();
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return (items, total);
    }

    public void SaveChanges() => _context.SaveChanges();
}