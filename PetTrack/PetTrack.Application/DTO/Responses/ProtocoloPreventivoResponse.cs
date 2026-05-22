using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Protocolo Preventivo.
/// </summary>
/// <param name="Id">Identificador único do Protocolo gerado pelo Oracle.</param>
/// <param name="Tipo">Tipo do protocolo: ANTIPULGA, CHECKUP, VACINA ou VERMIFUGO.</param>
/// <param name="Nome">Nome do protocolo preventivo.</param>
/// <param name="DateAplicacao">Data da última aplicação. Opcional.</param>
/// <param name="DateProxima">Data da próxima aplicação. Opcional.</param>
/// <param name="Status">Status: ATRASADO, PENDENTE ou REALIZADO.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record ProtocoloPreventivoResponse(
    long Id,
    TipoProtocoloPreventivoEnum Tipo,
    string Nome,
    DateTime? DateAplicacao,
    DateTime? DateProxima,
    StatusProtocoloPreventivoEnum Status,
    PetResponse Pet)
{
    /// <summary>Converte a entidade ProtocoloPreventivo para o DTO de resposta.</summary>
    public static ProtocoloPreventivoResponse ToDTO(ProtocoloPreventivo protocolo) => new(
        protocolo.Id,
        protocolo.Tipo,
        protocolo.Nome,
        protocolo.DateAplicacao,
        protocolo.DateProxima,
        protocolo.Status,
        PetResponse.ToDTO(protocolo.Pet));
}