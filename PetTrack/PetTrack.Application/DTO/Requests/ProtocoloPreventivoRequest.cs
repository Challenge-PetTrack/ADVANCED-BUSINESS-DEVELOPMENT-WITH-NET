using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Protocolo Preventivo.
/// </summary>
/// <param name="Tipo">Tipo do protocolo: ANTIPULGA, CHECKUP, VACINA ou VERMIFUGO.</param>
/// <param name="Nome">Nome do protocolo preventivo.</param>
/// <param name="DateAplicacao">Data da última aplicação. Opcional.</param>
/// <param name="DateProxima">Data da próxima aplicação. Opcional.</param>
/// <param name="Status">Status: ATRASADO, PENDENTE ou REALIZADO.</param>
/// <param name="Pet">Pet associado.</param>
public record ProtocoloPreventivoRequest(
    TipoProtocoloPreventivoEnum Tipo,
    string Nome,
    DateTime? DateAplicacao,
    DateTime? DateProxima,
    StatusProtocoloPreventivoEnum Status,
    Pet Pet)
{
    /// <summary>Converte o DTO para a entidade de domínio ProtocoloPreventivo.</summary>
    public ProtocoloPreventivo ToEntity() => new ProtocoloPreventivo(Tipo, Nome, DateAplicacao, DateProxima, Status, Pet);
}