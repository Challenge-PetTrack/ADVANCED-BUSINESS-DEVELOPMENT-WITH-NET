using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface INotificacaoService
{
    NotificacaoResponse Create(NotificacaoRequest request);
    NotificacaoResponse? GetById(long id);
    IReadOnlyList<NotificacaoResponse> GetAll();
    IReadOnlyList<NotificacaoResponse> GetByStatus(SimNaoEnum status);
    IReadOnlyList<NotificacaoResponse> GetByTipo(TipoNotificacaoEnum tipo);
    IReadOnlyList<NotificacaoResponse> GetUrgentesNaoLidasByTutor(long idTutor);
    NotificacaoResponse Update(long id, NotificacaoRequest request);
    bool Delete(long id);
}

public class NotificacaoService : INotificacaoService
{
    private readonly INotificacaoRepository _notificacaoRepository;

    public NotificacaoService(INotificacaoRepository notificacaoRepository)
    {
        _notificacaoRepository = notificacaoRepository;
    }

    public NotificacaoResponse Create(NotificacaoRequest request)
    {
        var notificacao = request.ToEntity();
        _notificacaoRepository.Add(notificacao);
        return NotificacaoResponse.ToDTO(notificacao);
    }

    public NotificacaoResponse? GetById(long id)
    {
        var notificacao = _notificacaoRepository.GetById(id);
        return notificacao is null ? null : NotificacaoResponse.ToDTO(notificacao);
    }

    public IReadOnlyList<NotificacaoResponse> GetAll() =>
        _notificacaoRepository.GetAll().Select(NotificacaoResponse.ToDTO).ToList();

    public IReadOnlyList<NotificacaoResponse> GetByStatus(SimNaoEnum status) =>
        _notificacaoRepository.GetByStatus(status).Select(NotificacaoResponse.ToDTO).ToList();

    public IReadOnlyList<NotificacaoResponse> GetByTipo(TipoNotificacaoEnum tipo) =>
        _notificacaoRepository.GetByTipo(tipo).Select(NotificacaoResponse.ToDTO).ToList();

    public IReadOnlyList<NotificacaoResponse> GetUrgentesNaoLidasByTutor(long idTutor) =>
        _notificacaoRepository.GetUrgentesNaoLidasByTutor(idTutor).Select(NotificacaoResponse.ToDTO).ToList();

    public NotificacaoResponse Update(long id, NotificacaoRequest request)
    {
        var notificacao = _notificacaoRepository.GetById(id)
            ?? throw new KeyNotFoundException("Notificação não encontrada.");

        notificacao.Transferir(request.Tipo, request.Titulo, request.Mensagem, request.Status, request.Tutor, request.Pet);
        _notificacaoRepository.SaveChanges();
        return NotificacaoResponse.ToDTO(notificacao);
    }

    public bool Delete(long id) => _notificacaoRepository.Delete(id);
}
