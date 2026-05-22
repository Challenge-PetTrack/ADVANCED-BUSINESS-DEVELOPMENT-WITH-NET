using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados da Adesão ao Medicamento.
/// </summary>
/// <param name="Id">Identificador único da Adesão gerado pelo Oracle.</param>
/// <param name="DataDose">Data em que a dose foi administrada.</param>
/// <param name="Status">Indica se o pet tomou: S ou N.</param>
/// <param name="Observacao">Observação sobre a adesão. Opcional.</param>
/// <param name="Medicamento">Dados do medicamento associado.</param>
public record AdesaoMedicamentoResponse(
    long Id,
    DateTime DataDose,
    SimNaoEnum Status,
    string? Observacao,
    MedicamentoResponse Medicamento)
{
    /// <summary>Converte a entidade AdesaoMedicamento para o DTO de resposta.</summary>
    public static AdesaoMedicamentoResponse ToDTO(AdesaoMedicamento adesao) => new(
        adesao.Id,
        adesao.DataDose,
        adesao.Status,
        adesao.Observacao,
        MedicamentoResponse.ToDTO(adesao.Medicamento));
}