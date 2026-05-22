using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Alerta.
/// </summary>
/// <param name="Id">Identificador único do Alerta gerado pelo Oracle.</param>
/// <param name="Tipo">Tipo do alerta: ADESAO, BCS_CRITICO, FEBRE, PESO ou SEDENTARISMO.</param>
/// <param name="Descricao">Descrição do alerta. Opcional.</param>
/// <param name="Valor">Valor de referência que gerou o alerta. Opcional.</param>
/// <param name="DataAlerta">Data do alerta gerada automaticamente pelo Oracle.</param>
/// <param name="Status">Indica se foi resolvido: S ou N.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record AlertaResponse(
    long Id,
    TipoAlertaEnum Tipo,
    string? Descricao,
    double? Valor,
    DateTime DataAlerta,
    SimNaoEnum Status,
    PetResponse Pet)
{
    /// <summary>Converte a entidade Alerta para o DTO de resposta.</summary>
    public static AlertaResponse ToDTO(Alerta alerta) => new(
        alerta.Id,
        alerta.Tipo,
        alerta.Descricao,
        alerta.Valor,
        alerta.DataAlerta,
        alerta.Status,
        PetResponse.ToDTO(alerta.Pet));
}