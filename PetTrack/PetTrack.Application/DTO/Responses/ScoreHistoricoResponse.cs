using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Score Histórico.
/// </summary>
/// <param name="Id">Identificador único do Score gerado pelo Oracle.</param>
/// <param name="Score">Pontuação do score. Entre 0 e 100.</param>
/// <param name="DataRegistro">Data do registro gerada automaticamente pelo Oracle.</param>
/// <param name="Observacao">Observação sobre o score. Opcional.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record ScoreHistoricoResponse(
    long Id,
    double Score,
    DateTime DataRegistro,
    string? Observacao,
    PetResponse Pet)
{
    /// <summary>Converte a entidade ScoreHistorico para o DTO de resposta.</summary>
    public static ScoreHistoricoResponse ToDTO(ScoreHistorico score) => new(
        score.Id,
        score.Score,
        score.DataRegistro,
        score.Observacao,
        PetResponse.ToDTO(score.Pet));
}