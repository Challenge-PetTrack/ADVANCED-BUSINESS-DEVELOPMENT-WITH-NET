using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do BCS Histórico.
/// </summary>
/// <param name="Id">Identificador único do BCS gerado pelo Oracle.</param>
/// <param name="Bcs">Body Condition Score. Entre 1 e 9. Opcional.</param>
/// <param name="FotoUrl">URL da foto utilizada na análise. Opcional.</param>
/// <param name="Observacao">Observação sobre o BCS. Opcional.</param>
/// <param name="DataAnalise">Data da análise gerada automaticamente pelo Oracle.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record BcsHistoricoResponse(
    long Id,
    int? Bcs,
    string? FotoUrl,
    string? Observacao,
    DateTime DataAnalise,
    PetResponse Pet)
{
    /// <summary>Converte a entidade BCSHistorico para o DTO de resposta.</summary>
    public static BcsHistoricoResponse ToDTO(BCSHistorico bcs) => new(
        bcs.Id,
        bcs.Bcs,
        bcs.FotoUrl,
        bcs.Observacao,
        bcs.DataAnalise,
        PetResponse.ToDTO(bcs.Pet));
}