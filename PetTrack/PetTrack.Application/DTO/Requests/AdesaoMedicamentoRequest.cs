using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar uma Adesão ao Medicamento.
/// </summary>
/// <param name="DataDose">Data em que a dose foi administrada.</param>
/// <param name="Status">Indica se o pet tomou: S ou N.</param>
/// <param name="Observacao">Observação sobre a adesão. Opcional.</param>
/// <param name="Medicamento">Medicamento associado.</param>
public record AdesaoMedicamentoRequest(
    DateTime DataDose,
    SimNaoEnum Status,
    string? Observacao,
    Medicamento Medicamento)
{
    /// <summary>Converte o DTO para a entidade de domínio AdesaoMedicamento.</summary>
    public AdesaoMedicamento ToEntity() => new AdesaoMedicamento(DataDose, Status, Observacao, Medicamento);
}