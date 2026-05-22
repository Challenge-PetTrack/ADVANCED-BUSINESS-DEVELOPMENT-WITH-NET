using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_SCORE_HISTORICO no Oracle DB.
/// </summary>
public class ScoreHistorico
{
    /// <summary>Identificador. Gerado via SEQ_SCORE_HIST no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Valor da pontuação do score. Entre 0 e 100.</summary>
    public double Score { get; private set; }

    /// <summary>Data do registro. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataRegistro { get; private set; }

    /// <summary>Observação opcional sobre o score.</summary>
    public string? Observacao { get; private set; }

    /// <summary>Pet associado ao score. 1:1.</summary>
    public Pet Pet { get; private set; } = null!;

    public ScoreHistorico(double score, string? observacao, Pet pet)
    {
        UpdateScore(score);
        UpdateObservacao(observacao);
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis do score. Usado no PUT.</summary>
    public void Transferir(double score, string? observacao, Pet pet)
    {
        UpdateScore(score);
        UpdateObservacao(observacao);
        UpdatePet(pet);
    }

    /// <summary>Atualiza o score. Deve estar entre 0 e 100.</summary>
    public void UpdateScore(double score)
    {
        if (score < 0 || score > 100)
            throw new Exception("Score deve estar entre 0 e 100.");
        Score = score;
    }
    
    /// <summary>Atualiza a observação. Opcional, máximo 500 caracteres.</summary>
    public void UpdateObservacao(string? observacao)
    {
        if (observacao != null && observacao.Length > 500)
            throw new Exception("Observação deve ter no máximo 500 caracteres.");
        Observacao = observacao;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("Pet não pode ser nulo.");
        Pet = pet;
    }
    
    public ScoreHistorico()
    {
    }
}