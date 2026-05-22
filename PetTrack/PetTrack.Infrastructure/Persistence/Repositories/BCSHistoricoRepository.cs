using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class BcsHistoricoRepository : IBCSHistoricoRepository
{
    private readonly PetTrackContext _context;

    public BcsHistoricoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<BCSHistorico> GetAll() =>
        _context.BcsHistoricos.ToList();

    public BCSHistorico? GetById(long id) =>
        _context.BcsHistoricos.FirstOrDefault(b => b.Id == id);

    public IReadOnlyList<BCSHistorico> GetHistoricoByPet(long idPet) =>
        _context.BcsHistoricos
            .Where(b => b.Pet.Id == idPet)
            .OrderByDescending(b => b.DataAnalise)
            .ToList();

    public double GetMediaBcsByPet(long idPet)
    {
        var bcs = _context.BcsHistoricos
            .Where(b => b.Pet.Id == idPet && b.Bcs.HasValue)
            .Select(b => b.Bcs!.Value)
            .ToList();

        return bcs.Any() ? bcs.Average() : 0;
    }

    public void Add(BCSHistorico bcs)
    {
        _context.BcsHistoricos.Add(bcs);
        _context.SaveChanges();
    }

    public void Update(BCSHistorico bcs)
    {
        _context.BcsHistoricos.Update(bcs);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var bcs = _context.BcsHistoricos.FirstOrDefault(b => b.Id == id);
        if (bcs == null) return false;
        _context.BcsHistoricos.Remove(bcs);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}