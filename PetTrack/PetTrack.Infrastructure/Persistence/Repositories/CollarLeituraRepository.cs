using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class CollarLeituraRepository : ICollarLeituraRepository
{
    private readonly PetTrackContext _context;

    public CollarLeituraRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<CollarLeitura> GetAll() =>
        _context.CollarLeituras.ToList();

    public CollarLeitura? GetById(long id) =>
        _context.CollarLeituras.FirstOrDefault(c => c.Id == id);

    public IReadOnlyList<CollarLeitura> GetByPetETemperaturaAcimaDe(long idPet, double temperatura) =>
        _context.CollarLeituras
            .Where(c => c.Pet.Id == idPet && c.Temperatura > temperatura)
            .OrderByDescending(c => c.DataLeitura)
            .ToList();

    public CollarLeitura? GetUltimaLeituraByPet(long idPet) =>
        _context.CollarLeituras
            .Where(c => c.Pet.Id == idPet)
            .OrderByDescending(c => c.DataLeitura)
            .FirstOrDefault();

    public void Add(CollarLeitura leitura)
    {
        _context.CollarLeituras.Add(leitura);
        _context.SaveChanges();
    }

    public void Update(CollarLeitura leitura)
    {
        _context.CollarLeituras.Update(leitura);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var leitura = _context.CollarLeituras.FirstOrDefault(c => c.Id == id);
        if (leitura == null) return false;
        _context.CollarLeituras.Remove(leitura);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}