using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_ADESAO_MEDICAMENTO no Oracle DB.
/// Valida regras específicas para atributos que representam colunas no DB.
/// </summary>
public class AdesaoMedicamento
{
    /// <summary>Identificador. Gerado via SEQ_ADESAO no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Data em que a dose foi administrada. Obrigatório.</summary>
    public DateTime DataDose { get; private set; }

    /// <summary>Indica se o pet tomou o medicamento: S ou N.</summary>
    public SimNaoEnum Status { get; private set; }

    /// <summary>Observação sobre a adesão. Opcional, máximo 500 caracteres.</summary>
    public string? Observacao { get; private set; }

    /// <summary>Medicamento associado. FK para TB_MEDICAMENTO. 1:1</summary>
    public virtual Medicamento Medicamento { get; private set; } = null!;

    public AdesaoMedicamento(
        DateTime dataDose,
        SimNaoEnum status,
        string? observacao,
        Medicamento medicamento)
    {
        UpdateDataDose(dataDose);
        Status = status;
        UpdateObservacao(observacao);
        UpdateMedicamento(medicamento);
    }

    /// <summary>Atualiza todos os campos editáveis da adesão. Usado no PUT.</summary>
    public void Transferir(
        DateTime dataDose,
        SimNaoEnum status,
        string? observacao,
        Medicamento medicamento)
    {
        UpdateDataDose(dataDose);
        Status = status;
        UpdateObservacao(observacao);
        UpdateMedicamento(medicamento);
    }

    /// <summary>Atualiza a data da dose. Não pode ser no futuro.</summary>
    public void UpdateDataDose(DateTime dataDose)
    {
        if (dataDose > DateTime.Now)
            throw new Exception("A data da dose não pode ser no futuro.");
        DataDose = dataDose;
    }

    /// <summary>Atualiza a observação. Opcional, máximo 500 caracteres.</summary>
    public void UpdateObservacao(string? observacao)
    {
        if (observacao != null && observacao.Length > 500)
            throw new Exception("A observação deve ter no máximo 500 caracteres.");
        Observacao = observacao;
    }

    /// <summary>Atualiza o medicamento associado. Não pode ser nulo.</summary>
    public void UpdateMedicamento(Medicamento medicamento)
    {
        if (medicamento is null)
            throw new Exception("O medicamento não pode ser nulo.");
        Medicamento = medicamento;
    }

    public AdesaoMedicamento() { }
}