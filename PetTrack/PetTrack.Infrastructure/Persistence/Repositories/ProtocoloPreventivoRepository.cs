using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class ProtocoloPreventivoRepository : IProtocoloPreventivoRepository
{
    private readonly PetTrackContext _context;

    public ProtocoloPreventivoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<ProtocoloPreventivo> GetAll() =>
        _context.ProtocolosPreventivos.ToList();

    public ProtocoloPreventivo? GetById(long id) =>
        _context.ProtocolosPreventivos.FirstOrDefault(p => p.Id == id);

    public IReadOnlyList<ProtocoloPreventivo> GetByTipo(TipoProtocoloPreventivoEnum tipo) =>
        _context.ProtocolosPreventivos
            .Where(p => p.Tipo == tipo)
            .ToList();

    public IReadOnlyList<ProtocoloPreventivo> GetPendentesOuAtrasadosByPet(long idPet) =>
        _context.ProtocolosPreventivos
            .Where(p => p.Pet.Id == idPet &&
                        (p.Status == StatusProtocoloPreventivoEnum.PENDENTE ||
                         p.Status == StatusProtocoloPreventivoEnum.ATRASADO))
            .ToList();

    public void Add(ProtocoloPreventivo protocolo)
    {
        _context.ProtocolosPreventivos.Add(protocolo);
        _context.SaveChanges();
    }

    public void Update(ProtocoloPreventivo protocolo)
    {
        _context.ProtocolosPreventivos.Update(protocolo);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var protocolo = _context.ProtocolosPreventivos.FirstOrDefault(p => p.Id == id);
        if (protocolo == null) return false;
        _context.ProtocolosPreventivos.Remove(protocolo);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}