using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface INotificacaoRepository
{
    /// <summary>Retorna todas as notificações.</summary>
    IReadOnlyList<Notificacao> GetAll();

    /// <summary>Retorna uma notificação pelo seu identificador.</summary>
    Notificacao? GetById(long id);

    /// <summary>Retorna notificações pelo status de leitura.</summary>
    IReadOnlyList<Notificacao> GetByStatus(SimNaoEnum status);

    /// <summary>Retorna notificações pelo tipo.</summary>
    IReadOnlyList<Notificacao> GetByTipo(TipoNotificacaoEnum tipo);

    /// <summary>Retorna notificações urgentes não lidas por tutor.</summary>
    IReadOnlyList<Notificacao> GetUrgentesNaoLidasByTutor(long idTutor);

    /// <summary>Adiciona uma nova notificação.</summary>
    void Add(Notificacao notificacao);

    /// <summary>Atualiza uma notificação existente.</summary>
    void Update(Notificacao notificacao);

    /// <summary>Remove uma notificação pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}