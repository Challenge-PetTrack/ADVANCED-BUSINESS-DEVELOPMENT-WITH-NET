using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_PET no Oracle DB.
/// </summary>
public class Pet
{
    /// <summary>Identificador. Gerado via SEQ_PET no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Nome do pet. Obrigatório, máximo 100 caracteres.</summary>
    public string Nome { get; private set; } = null!;

    /// <summary>Espécie do pet. Obrigatório, máximo 50 caracteres.</summary>
    public string Especie { get; private set; } = null!;

    /// <summary>Raça do pet. Opcional, máximo 100 caracteres.</summary>
    public string? Raca { get; private set; }

    /// <summary>Sexo do pet: M ou F.</summary>
    public SexoPetEnum Sexo { get; private set; }

    /// <summary>Idade do pet em anos.</summary>
    public double Idade { get; private set; }

    /// <summary>Peso do pet em kg.</summary>
    public double Peso { get; private set; }

    /// <summary>Data de cadastro. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataCadastro { get; private set; }

    /// <summary>Tutor responsável pelo pet. FK para TB_TUTOR. 1:1</summary>
    public Tutor Tutor { get; private set; } = null!;

    /// <summary>Clínica associada ao pet. FK para TB_CLINICA. 1:1</summary>
    public Clinica Clinica { get; private set; } = null!;

    /// <summary>Lista de eventos clínicos. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<EventoClinico> Eventos { get; private set; }

    /// <summary>Lista de notificações. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<Notificacao> Notificacoes { get; private set; }

    /// <summary>Histórico de scores. Não serializado no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<ScoreHistorico> Scores { get; private set; }

    /// <summary>Histórico de BCS. Não serializado no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<BCSHistorico> BcsHistoricos { get; private set; }

    /// <summary>Leituras do collar IoT. Não serializadas no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<CollarLeitura> CollarLeituras { get; private set; }

    /// <summary>Alertas do pet. Não serializados no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<Alerta> Alertas { get; private set; }

    /// <summary>Protocolos preventivos. Não serializados no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public List<ProtocoloPreventivo> Protocolos { get; private set; }

    public Pet(
        string nome,
        string especie,
        string? raca,
        SexoPetEnum sexo,
        double idade,
        double peso,
        Tutor tutor,
        Clinica clinica)
    {
        UpdateNome(nome);
        UpdateEspecie(especie);
        UpdateRaca(raca);
        Sexo = sexo;
        UpdateIdade(idade);
        UpdatePeso(peso);
        UpdateTutor(tutor);
        UpdateClinica(clinica);
    }

    /// <summary>Atualiza todos os campos editáveis do pet. Usado no PUT.</summary>
    public void Transferir(
        string nome,
        string especie,
        string? raca,
        SexoPetEnum sexo,
        double idade,
        double peso,
        Tutor tutor,
        Clinica clinica)
    {
        UpdateNome(nome);
        UpdateEspecie(especie);
        UpdateRaca(raca);
        Sexo = sexo;
        UpdateIdade(idade);
        UpdatePeso(peso);
        UpdateTutor(tutor);
        UpdateClinica(clinica);
    }

    /// <summary>Atualiza o nome. Não pode ser vazio, máximo 100 caracteres.</summary>
    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome não pode ser vazio.");
        if (nome.Length > 100)
            throw new Exception("O nome deve ter no máximo 100 caracteres.");
        Nome = nome.Trim();
    }

    /// <summary>Atualiza a espécie. Não pode ser vazia, máximo 50 caracteres.</summary>
    public void UpdateEspecie(string especie)
    {
        if (string.IsNullOrWhiteSpace(especie))
            throw new Exception("A espécie não pode ser vazia.");
        if (especie.Length > 50)
            throw new Exception("A espécie deve ter no máximo 50 caracteres.");
        Especie = especie.Trim();
    }

    /// <summary>Atualiza a raça. Opcional, máximo 100 caracteres.</summary>
    public void UpdateRaca(string? raca)
    {
        if (raca != null && raca.Length > 100)
            throw new Exception("A raça deve ter no máximo 100 caracteres.");
        Raca = raca;
    }

    /// <summary>Atualiza a idade. Deve ser maior ou igual a zero.</summary>
    public void UpdateIdade(double idade)
    {
        if (idade < 0)
            throw new Exception("A idade não pode ser negativa.");
        Idade = idade;
    }

    /// <summary>Atualiza o peso. Deve ser maior que zero.</summary>
    public void UpdatePeso(double peso)
    {
        if (peso <= 0)
            throw new Exception("O peso deve ser maior que zero.");
        Peso = peso;
    }

    /// <summary>Atualiza o tutor responsável. Não pode ser nulo.</summary>
    public void UpdateTutor(Tutor tutor)
    {
        if (tutor is null)
            throw new Exception("O tutor não pode ser nulo.");
        Tutor = tutor;
    }

    /// <summary>Atualiza a clínica associada. Não pode ser nula.</summary>
    public void UpdateClinica(Clinica clinica)
    {
        if (clinica is null)
            throw new Exception("A clínica não pode ser nula.");
        Clinica = clinica;
    }

    public Pet()
    {
        Eventos = [];
        Notificacoes = [];
        Scores = [];
        BcsHistoricos = [];
        CollarLeituras = [];
        Alertas = [];
        Protocolos = [];
    }
}