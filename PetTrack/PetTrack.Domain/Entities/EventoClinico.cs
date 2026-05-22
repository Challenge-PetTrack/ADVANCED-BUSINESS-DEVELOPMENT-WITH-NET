using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_EVENTO_CLINICO no Oracle DB.
/// </summary>
public class EventoClinico
{
    /// <summary>Identificador. Gerado via SEQ_EVENTO_CLINICO no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Tipo do evento: CIRURGIA, CONSULTA, EXAME ou RETORNO.</summary>
    public TipoEventoClinicoEnum Tipo { get; private set; }

    /// <summary>Data do evento clínico. Obrigatório.</summary>
    public DateTime DataEvento { get; private set; }

    /// <summary>Diagnóstico registrado. Opcional, máximo 1000 caracteres.</summary>
    public string? Diagnostico { get; private set; }

    /// <summary>Observação clínica. Opcional, máximo 2000 caracteres.</summary>
    public string? Observacao { get; private set; }

    /// <summary>Pet associado ao evento. FK para TB_PET. 1:1</summary>
    public Pet Pet { get; private set; } = null!;

    /// <summary>Clínica onde ocorreu o evento. FK para TB_CLINICA. 1:1</summary>
    public Clinica Clinica { get; private set; } = null!;

    /// <summary>Lista de medicamentos prescritos. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<Medicamento> Medicamentos { get; private set; }

    public EventoClinico(
        TipoEventoClinicoEnum tipo,
        DateTime dataEvento,
        string? diagnostico,
        string? observacao,
        Pet pet,
        Clinica clinica)
    {
        Tipo = tipo;
        UpdateDataEvento(dataEvento);
        UpdateDiagnostico(diagnostico);
        UpdateObservacao(observacao);
        UpdatePet(pet);
        UpdateClinica(clinica);
    }

    /// <summary>Atualiza todos os campos editáveis do evento clínico. Usado no PUT.</summary>
    public void Transferir(
        TipoEventoClinicoEnum tipo,
        DateTime dataEvento,
        string? diagnostico,
        string? observacao,
        Pet pet,
        Clinica clinica)
    {
        Tipo = tipo;
        UpdateDataEvento(dataEvento);
        UpdateDiagnostico(diagnostico);
        UpdateObservacao(observacao);
        UpdatePet(pet);
        UpdateClinica(clinica);
    }

    /// <summary>Atualiza a data do evento. Não pode ser no futuro.</summary>
    public void UpdateDataEvento(DateTime dataEvento)
    {
        if (dataEvento > DateTime.Now)
            throw new Exception("A data do evento não pode ser no futuro.");
        DataEvento = dataEvento;
    }

    /// <summary>Atualiza o diagnóstico. Opcional, máximo 1000 caracteres.</summary>
    public void UpdateDiagnostico(string? diagnostico)
    {
        if (diagnostico != null && diagnostico.Length > 1000)
            throw new Exception("O diagnóstico deve ter no máximo 1000 caracteres.");
        Diagnostico = diagnostico;
    }

    /// <summary>Atualiza a observação. Opcional, máximo 2000 caracteres.</summary>
    public void UpdateObservacao(string? observacao)
    {
        if (observacao != null && observacao.Length > 2000)
            throw new Exception("A observação deve ter no máximo 2000 caracteres.");
        Observacao = observacao;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("O pet não pode ser nulo.");
        Pet = pet;
    }

    /// <summary>Atualiza a clínica associada. Não pode ser nula.</summary>
    public void UpdateClinica(Clinica clinica)
    {
        if (clinica is null)
            throw new Exception("A clínica não pode ser nula.");
        Clinica = clinica;
    }

    public EventoClinico()
    {
        Medicamentos = [];
    }
}