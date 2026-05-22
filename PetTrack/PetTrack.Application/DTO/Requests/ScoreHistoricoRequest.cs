using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Score Histórico.
/// </summary>
/// <param name="Score">Pontuação do score. Entre 0 e 100.</param>
/// <param name="Observacao">Observação sobre o score. Opcional.</param>
/// <param name="Pet">Pet associado ao score.</param>
public record ScoreHistoricoRequest(
    double Score,
    string? Observacao,
    Pet Pet){
    /// <summary>Converte o DTO para a entidade de domínio ScoreHistorico.</summary>
    public ScoreHistorico ToEntity() => new ScoreHistorico(Score, Observacao, Pet);
}