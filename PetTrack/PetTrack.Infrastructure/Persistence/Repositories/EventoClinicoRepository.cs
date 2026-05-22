using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class EventoClinicoRepository : IEventoClinicoRepository
{
    private readonly PetTrackContext _context;

    public EventoClinicoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<EventoClinico> GetAll() =>
        _context.EventosClinicos.ToList();

    public EventoClinico? GetById(long id) =>
        _context.EventosClinicos.FirstOrDefault(e => e.Id == id);

    public IReadOnlyList<EventoClinico> GetByTipo(TipoEventoClinicoEnum tipo) =>
        _context.EventosClinicos
            .Where(e => e.Tipo == tipo)
            .ToList();

    public IReadOnlyList<EventoClinico> GetEventosComMedicamentos(long idPet) =>
        _context.EventosClinicos
            .Where(e => e.Pet.Id == idPet && e.Medicamentos.Any())
            .ToList();

    public void Add(EventoClinico evento)
    {
        _context.EventosClinicos.Add(evento);
        _context.SaveChanges();
    }

    public void Update(EventoClinico evento)
    {
        _context.EventosClinicos.Update(evento);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var evento = _context.EventosClinicos.FirstOrDefault(e => e.Id == id);
        if (evento == null) return false;
        _context.EventosClinicos.Remove(evento);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}