using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_ALERTA no Oracle DB.
/// Valida regras específicas para atributos que representam colunas no DB.
/// </summary>
public class Alerta
{
    /// <summary>Identificador. Gerado via SEQ_ALERTA no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Tipo do alerta: ADESAO, BCS_CRITICO, FEBRE, PESO ou SEDENTARISMO.</summary>
    public TipoAlertaEnum Tipo { get; private set; }

    /// <summary>Descrição do alerta. Opcional, máximo 1000 caracteres.</summary>
    public string? Descricao { get; private set; }

    /// <summary>Valor de referência que gerou o alerta (temperatura, passos, etc). Opcional.</summary>
    public double? Valor { get; private set; }

    /// <summary>Data do alerta. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataAlerta { get; private set; }

    /// <summary>Indica se o alerta foi resolvido: S ou N.</summary>
    public SimNaoEnum Status { get; private set; }

    /// <summary>Pet associado ao alerta. FK para TB_PET. 1:1</summary>
    public Pet Pet { get; private set; } = null!;

    public Alerta(
        TipoAlertaEnum tipo,
        string? descricao,
        double? valor,
        SimNaoEnum status,
        Pet pet)
    {
        Tipo = tipo;
        UpdateDescricao(descricao);
        Valor = valor;
        Status = status;
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis do alerta. Usado no PUT.</summary>
    public void Atualizar(
        TipoAlertaEnum tipo,
        string? descricao,
        double? valor,
        SimNaoEnum status,
        Pet pet)
    {
        Tipo = tipo;
        UpdateDescricao(descricao);
        Valor = valor;
        Status = status;
        UpdatePet(pet);
    }

    /// <summary>Atualiza a descrição. Opcional, máximo 1000 caracteres.</summary>
    public void UpdateDescricao(string? descricao)
    {
        if (descricao != null && descricao.Length > 1000)
            throw new Exception("A descrição deve ter no máximo 1000 caracteres.");
        Descricao = descricao;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("O pet não pode ser nulo.");
        Pet = pet;
    }

    public Alerta() { }
}