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
        _context.Pets.ToList();

    public Pet? GetById(long id) =>
        _context.Pets.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<Pet> GetByClinicaId(long idClinica) =>
        _context.Pets
            .Where(p => p.Clinica.Id == idClinica)
            .ToList();

    public IReadOnlyList<Pet> GetBySexo(SexoPetEnum sexo) =>
        _context.Pets
            .Where(p => p.Sexo == sexo)
            .ToList();

    public IReadOnlyList<Pet> GetByNomeOuEspecie(string busca) =>
        _context.Pets
            .Where(p => p.Nome.Contains(busca) || p.Especie.Contains(busca))
            .ToList();

    public IReadOnlyList<Pet> GetPetsComAlertasPendentes() =>
        _context.Pets
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

    public void SaveChanges() => _context.SaveChanges();
}