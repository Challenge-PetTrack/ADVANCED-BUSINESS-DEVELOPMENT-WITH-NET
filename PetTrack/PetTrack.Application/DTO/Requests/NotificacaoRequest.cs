using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar uma Notificação.
/// </summary>
/// <param name="Tipo">Tipo da notificação: ALERTA, INFO, LEMBRETE ou URGENTE.</param>
/// <param name="Titulo">Título da notificação.</param>
/// <param name="Mensagem">Mensagem da notificação. Opcional.</param>
/// <param name="Status">Status de leitura: S (lida) ou N (não lida).</param>
/// <param name="Tutor">Tutor destinatário.</param>
/// <param name="Pet">Pet associado.</param>
public record NotificacaoRequest(
    TipoNotificacaoEnum Tipo,
    string Titulo,
    string? Mensagem,
    SimNaoEnum Status,
    Tutor Tutor,
    Pet Pet)
{
    /// <summary>Converte o DTO para a entidade de domínio Notificacao.</summary>
    public Notificacao ToEntity() => new Notificacao(Tipo, Titulo, Mensagem, Status, Tutor, Pet);
}