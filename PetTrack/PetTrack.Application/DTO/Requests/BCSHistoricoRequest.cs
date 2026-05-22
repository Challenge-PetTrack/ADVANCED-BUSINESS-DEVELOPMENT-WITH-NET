using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um BCS Histórico.
/// </summary>
/// <param name="Bcs">Body Condition Score. Entre 1 e 9. Opcional.</param>
/// <param name="FotoUrl">URL da foto utilizada na análise. Opcional.</param>
/// <param name="Observacao">Observação sobre o BCS. Opcional.</param>
/// <param name="Pet">Pet associado.</param>
public record BCSHistoricoRequest(
    int? Bcs,
    string? FotoUrl,
    string? Observacao,
    Pet Pet)
{
    /// <summary>Converte o DTO para a entidade de domínio BCSHistorico.</summary>
    public BCSHistorico ToEntity() => new BCSHistorico(Bcs, FotoUrl, Observacao, Pet);
}