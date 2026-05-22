using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados da Notificação.
/// </summary>
/// <param name="Id">Identificador único da Notificação gerado pelo Oracle.</param>
/// <param name="Tipo">Tipo da notificação: ALERTA, INFO, LEMBRETE ou URGENTE.</param>
/// <param name="Titulo">Título da notificação.</param>
/// <param name="Mensagem">Mensagem da notificação. Opcional.</param>
/// <param name="DataEnvio">Data de envio gerada automaticamente pelo Oracle.</param>
/// <param name="Status">Status de leitura: S (lida) ou N (não lida).</param>
/// <param name="Tutor">Dados do tutor destinatário.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record NotificacaoResponse(
    long Id,
    TipoNotificacaoEnum Tipo,
    string Titulo,
    string? Mensagem,
    DateTime DataEnvio,
    SimNaoEnum Status,
    TutorResponse Tutor,
    PetResponse Pet)
{
    /// <summary>Converte a entidade Notificacao para o DTO de resposta.</summary>
    public static NotificacaoResponse ToDTO(Notificacao notificacao) => new(
        notificacao.Id,
        notificacao.Tipo,
        notificacao.Titulo,
        notificacao.Mensagem,
        notificacao.DataEnvio,
        notificacao.Status,
        TutorResponse.ToDTO(notificacao.Tutor),
        PetResponse.ToDTO(notificacao.Pet));
}