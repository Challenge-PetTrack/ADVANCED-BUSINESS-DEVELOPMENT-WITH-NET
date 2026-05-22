using Microsoft.EntityFrameworkCore;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class TutorRepository : ITutorRepository
{
    private readonly PetTrackContext _context;

    public TutorRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Tutor> GetAll() =>
        _context.Tutores.ToList();

    public Tutor? GetById(long id) =>
        _context.Tutores.FirstOrDefault(t => t.Id == id);

    public IReadOnlyList<Tutor> GetByNomeOuEmail(string busca) =>
        _context.Tutores
            .Where(t => t.Nome.Contains(busca) || t.Email.Contains(busca))
            .ToList();

    public IReadOnlyList<Tutor> GetByNomePet(string nomePet) =>
        _context.Tutores
            .Where(t => t.Pets.Any(p => p.Nome.Contains(nomePet)))
            .ToList();

    public void Add(Tutor tutor)
    {
        _context.Tutores.Add(tutor);
        _context.SaveChanges();
    }

    public void Update(Tutor tutor)
    {
        _context.Tutores.Update(tutor);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var tutor = _context.Tutores.FirstOrDefault(t => t.Id == id);
        if (tutor == null) return false;
        _context.Tutores.Remove(tutor);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}