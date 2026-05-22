using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_MEDICAMENTO no Oracle DB.
/// </summary>
public class Medicamento
{
    /// <summary>Identificador. Gerado via SEQ_MEDICAMENTO no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Nome do medicamento. Obrigatório, máximo 200 caracteres.</summary>
    public string Nome { get; private set; } = null!;

    /// <summary>Dosagem do medicamento. Obrigatório, máximo 100 caracteres.</summary>
    public string Dosagem { get; private set; } = null!;

    /// <summary>Frequência de administração. Obrigatório, máximo 100 caracteres.</summary>
    public string Frequencia { get; private set; } = null!;

    /// <summary>Data de início do tratamento. Obrigatório.</summary>
    public DateTime DataInicio { get; private set; }

    /// <summary>Data de término do tratamento. Opcional.</summary>
    public DateTime? DataFim { get; private set; }

    /// <summary>Evento clínico associado. FK para TB_EVENTO_CLINICO. 1:1</summary>
    public virtual EventoClinico Evento { get; private set; } = null!;

    /// <summary>Lista de adesões ao medicamento. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public virtual List<AdesaoMedicamento> Adesoes { get; private set; }

    public Medicamento(
        string nome,
        string dosagem,
        string frequencia,
        DateTime dataInicio,
        DateTime? dataFim,
        EventoClinico evento)
    {
        UpdateNome(nome);
        UpdateDosagem(dosagem);
        UpdateFrequencia(frequencia);
        UpdateDatas(dataInicio, dataFim);
        UpdateEvento(evento);
    }

    /// <summary>Atualiza todos os campos editáveis do medicamento. Usado no PUT.</summary>
    public void Transferir(
        string nome,
        string dosagem,
        string frequencia,
        DateTime dataInicio,
        DateTime? dataFim,
        EventoClinico evento)
    {
        UpdateNome(nome);
        UpdateDosagem(dosagem);
        UpdateFrequencia(frequencia);
        UpdateDatas(dataInicio, dataFim);
        UpdateEvento(evento);
    }

    /// <summary>Atualiza o nome. Não pode ser vazio, máximo 200 caracteres.</summary>
    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome não pode ser vazio.");
        if (nome.Length > 200)
            throw new Exception("O nome deve ter no máximo 200 caracteres.");
        Nome = nome.Trim();
    }

    /// <summary>Atualiza a dosagem. Não pode ser vazia, máximo 100 caracteres.</summary>
    public void UpdateDosagem(string dosagem)
    {
        if (string.IsNullOrWhiteSpace(dosagem))
            throw new Exception("A dosagem não pode ser vazia.");
        if (dosagem.Length > 100)
            throw new Exception("A dosagem deve ter no máximo 100 caracteres.");
        Dosagem = dosagem.Trim();
    }

    /// <summary>Atualiza a frequência. Não pode ser vazia, máximo 100 caracteres.</summary>
    public void UpdateFrequencia(string frequencia)
    {
        if (string.IsNullOrWhiteSpace(frequencia))
            throw new Exception("A frequência não pode ser vazia.");
        if (frequencia.Length > 100)
            throw new Exception("A frequência deve ter no máximo 100 caracteres.");
        Frequencia = frequencia.Trim();
    }

    /// <summary>Atualiza as datas. DataFim deve ser após DataInicio quando informada.</summary>
   public void UpdateDatas(DateTime dataInicio, DateTime? dataFim)
    {
        if (dataFim.HasValue && dataFim <= dataInicio)
            throw new Exception("A data de término deve ser após a data de início.");
        DataInicio = dataInicio;
        DataFim = dataFim;
    }

    /// <summary>Atualiza o evento clínico associado. Não pode ser nulo.</summary>
    public void UpdateEvento(EventoClinico evento)
    {
        if (evento is null)
            throw new Exception("O evento clínico não pode ser nulo.");
        Evento = evento;
    }

    public Medicamento()
    {
        Adesoes = [];
    }
}