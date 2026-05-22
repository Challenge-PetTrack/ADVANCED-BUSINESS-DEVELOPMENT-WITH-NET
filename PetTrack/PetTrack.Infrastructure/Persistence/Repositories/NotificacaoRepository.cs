using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Infrastructure.Persistence.Repositories;

public class NotificacaoRepository : INotificacaoRepository
{
    private readonly PetTrackContext _context;

    public NotificacaoRepository(PetTrackContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Notificacao> GetAll() =>
        _context.Notificacoes.ToList();

    public Notificacao? GetById(long id) =>
        _context.Notificacoes.FirstOrDefault(n => n.Id == id);

    public IReadOnlyList<Notificacao> GetByStatus(SimNaoEnum status) =>
        _context.Notificacoes
            .Where(n => n.Status == status)
            .ToList();

    public IReadOnlyList<Notificacao> GetByTipo(TipoNotificacaoEnum tipo) =>
        _context.Notificacoes
            .Where(n => n.Tipo == tipo)
            .ToList();

    public IReadOnlyList<Notificacao> GetUrgentesNaoLidasByTutor(long idTutor) =>
        _context.Notificacoes
            .Where(n => n.Tutor.Id == idTutor
                        && n.Tipo == TipoNotificacaoEnum.URGENTE
                        && n.Status == SimNaoEnum.N)
            .ToList();

    public void Add(Notificacao notificacao)
    {
        _context.Notificacoes.Add(notificacao);
        _context.SaveChanges();
    }

    public void Update(Notificacao notificacao)
    {
        _context.Notificacoes.Update(notificacao);
        _context.SaveChanges();
    }

    public bool Delete(long id)
    {
        var notificacao = _context.Notificacoes.FirstOrDefault(n => n.Id == id);
        if (notificacao == null) return false;
        _context.Notificacoes.Remove(notificacao);
        _context.SaveChanges();
        return true;
    }

    public void SaveChanges() => _context.SaveChanges();
}