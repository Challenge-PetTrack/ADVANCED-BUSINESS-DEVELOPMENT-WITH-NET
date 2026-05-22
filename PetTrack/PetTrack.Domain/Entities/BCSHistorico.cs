using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_BCS_HISTORICO no Oracle DB.
/// Valida regras específicas para atributos que representam colunas no DB.
/// </summary>
public class BCSHistorico
{
    /// <summary>Identificador. Gerado via SEQ_BCS_HIST no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Body Condition Score. Deve estar entre 1 e 9.</summary>
    public int? Bcs { get; private set; }

    /// <summary>URL da foto utilizada na análise. Opcional, máximo 500 caracteres.</summary>
    public string? FotoUrl { get; private set; }

    /// <summary>Observação sobre o BCS. Opcional, máximo 1000 caracteres.</summary>
    public string? Observacao { get; private set; }

    /// <summary>Data da análise. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataAnalise { get; private set; }

    /// <summary>Pet associado ao BCS. FK para TB_PET. 1:1</summary>
    public virtual Pet Pet { get; private set; } = null!;

    public BCSHistorico(
        int? bcs,
        string? fotoUrl,
        string? observacao,
        Pet pet)
    {
        UpdateBcs(bcs);
        UpdateFotoUrl(fotoUrl);
        UpdateObservacao(observacao);
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis do BCS. Usado no PUT.</summary>
    public void Transferir(
        int? bcs,
        string? fotoUrl,
        string? observacao)
    {
        UpdateBcs(bcs);
        UpdateFotoUrl(fotoUrl);
        UpdateObservacao(observacao);
    }

    /// <summary>Atualiza o BCS. Deve estar entre 1 e 9.</summary>
   public void UpdateBcs(int? bcs)
    {
        if (bcs < 1 || bcs > 9)
            throw new Exception("O BCS deve estar entre 1 e 9.");
        Bcs = bcs;
    }

    /// <summary>Atualiza a URL da foto. Opcional, máximo 500 caracteres.</summary>
    public void UpdateFotoUrl(string? fotoUrl)
    {
        if (fotoUrl != null && fotoUrl.Length > 500)
            throw new Exception("A URL da foto deve ter no máximo 500 caracteres.");
        FotoUrl = fotoUrl;
    }

    /// <summary>Atualiza a observação. Opcional, máximo 1000 caracteres.</summary>
    public void UpdateObservacao(string? observacao)
    {
        if (observacao != null && observacao.Length > 1000)
            throw new Exception("A observação deve ter no máximo 1000 caracteres.");
        Observacao = observacao;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("O pet não pode ser nulo.");
        Pet = pet;
    }

    public BCSHistorico() { }
}