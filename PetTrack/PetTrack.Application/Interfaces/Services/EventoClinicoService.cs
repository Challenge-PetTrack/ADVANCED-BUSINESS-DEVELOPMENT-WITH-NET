using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface IEventoClinicoService
{
    EventoClinicoResponse Create(EventoClinicoRequest request);
    EventoClinicoResponse? GetById(long id);
    IReadOnlyList<EventoClinicoResponse> GetAll();
    IReadOnlyList<EventoClinicoResponse> GetByTipo(TipoEventoClinicoEnum tipo);
    IReadOnlyList<EventoClinicoResponse> GetEventosComMedicamentos(long idPet);
    EventoClinicoResponse Update(long id, EventoClinicoRequest request);
    bool Delete(long id);
}

public class EventoClinicoService : IEventoClinicoService
{
    private readonly IEventoClinicoRepository _eventoRepository;

    public EventoClinicoService(IEventoClinicoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    public EventoClinicoResponse Create(EventoClinicoRequest request)
    {
        var evento = request.ToEntity();
        _eventoRepository.Add(evento);
        return EventoClinicoResponse.ToDTO(evento);
    }

    public EventoClinicoResponse? GetById(long id)
    {
        var evento = _eventoRepository.GetById(id);
        return evento is null ? null : EventoClinicoResponse.ToDTO(evento);
    }

    public IReadOnlyList<EventoClinicoResponse> GetAll() =>
        _eventoRepository.GetAll().Select(EventoClinicoResponse.ToDTO).ToList();

    public IReadOnlyList<EventoClinicoResponse> GetByTipo(TipoEventoClinicoEnum tipo) =>
        _eventoRepository.GetByTipo(tipo).Select(EventoClinicoResponse.ToDTO).ToList();

    public IReadOnlyList<EventoClinicoResponse> GetEventosComMedicamentos(long idPet) =>
        _eventoRepository.GetEventosComMedicamentos(idPet).Select(EventoClinicoResponse.ToDTO).ToList();

    public EventoClinicoResponse Update(long id, EventoClinicoRequest request)
    {
        var evento = _eventoRepository.GetById(id)
            ?? throw new KeyNotFoundException("Evento clínico não encontrado.");

        evento.Transferir(request.Tipo, request.DataEvento, request.Diagnostico, request.Observacao, request.Pet, request.Clinica);
        _eventoRepository.SaveChanges();
        return EventoClinicoResponse.ToDTO(evento);
    }

    public bool Delete(long id) => _eventoRepository.Delete(id);
}
