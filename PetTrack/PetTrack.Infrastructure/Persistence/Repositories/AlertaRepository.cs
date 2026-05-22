using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class AlertaRepository : IAlertaRepository
{
    private readonly PetTrackContext _context;

    public AlertaRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Alerta> GetAll() =>
        _context.Alertas.ToList();

    public Alerta? GetById(long id) =>
        _context.Alertas.FirstOrDefault(a => a.Id == id);

    public IReadOnlyList<Alerta> GetByTipo(TipoAlertaEnum tipo) =>
        _context.Alertas
            .Where(a => a.Tipo == tipo)
            .ToList();

    public IReadOnlyList<Alerta> GetPendentesByPet(long idPet) =>
        _context.Alertas
            .Where(a => a.Pet.Id == idPet && a.Status == SimNaoEnum.N)
            .ToList();

    public void Add(Alerta alerta)
    {
        _context.Alertas.Add(alerta);
        _context.SaveChanges();
    }

    public void Update(Alerta alerta)
    {
        _context.Alertas.Update(alerta);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var alerta = _context.Alertas.FirstOrDefault(a => a.Id == id);
        if (alerta == null) return false;
        _context.Alertas.Remove(alerta);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}