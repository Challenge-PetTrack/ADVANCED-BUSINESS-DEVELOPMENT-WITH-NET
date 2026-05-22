using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Alerta.
/// </summary>
/// <param name="Tipo">Tipo do alerta: ADESAO, BCS_CRITICO, FEBRE, PESO ou SEDENTARISMO.</param>
/// <param name="Descricao">Descrição do alerta. Opcional.</param>
/// <param name="Valor">Valor de referência que gerou o alerta. Opcional.</param>
/// <param name="Status">Indica se foi resolvido: S ou N.</param>
/// <param name="Pet">Pet associado.</param>
public record AlertaRequest(
    TipoAlertaEnum Tipo,
    string? Descricao,
    double? Valor,
    SimNaoEnum Status,
    Pet Pet)
{
    /// <summary>Converte o DTO para a entidade de domínio Alerta.</summary>
    public Alerta ToEntity() => new Alerta(Tipo, Descricao, Valor, Status, Pet);
}