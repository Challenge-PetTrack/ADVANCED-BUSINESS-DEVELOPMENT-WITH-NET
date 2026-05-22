using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class ClinicaRepository : IClinicaRepository
{
    private readonly PetTrackContext _context;

    public ClinicaRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Clinica> GetAll() =>
        _context.Clinicas.ToList();

    public Clinica? GetById(long id) =>
        _context.Clinicas.FirstOrDefault(c => c.Id == id);

    public IReadOnlyList<Clinica> GetByNomeOuCnpj(string busca) =>
        _context.Clinicas
            .Where(c => c.Nome.Contains(busca) || c.Cnpj.Contains(busca))
            .ToList();

    public IReadOnlyList<Clinica> GetByNomePet(string nomePet) =>
        _context.Clinicas
            .Where(c => c.Pets.Any(p => p.Nome.Contains(nomePet)))
            .ToList();

    public void Add(Clinica clinica)
    {
        _context.Clinicas.Add(clinica);
        _context.SaveChanges();
    }

    public void Update(Clinica clinica)
    {
        _context.Clinicas.Update(clinica);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var clinica = _context.Clinicas.FirstOrDefault(c => c.Id == id);
        if (clinica == null) return false;
        _context.Clinicas.Remove(clinica);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}