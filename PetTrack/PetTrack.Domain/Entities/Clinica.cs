using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_CLINICA no Oracle DB.
/// </summary>
public class Clinica
{
    /// <summary>Identificador. Gerado via SEQ_CLINICA no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Nome da clínica. Obrigatório, máximo 200 caracteres.</summary>
    public string Nome { get; private set; } = null!;

    /// <summary>CNPJ da clínica. Obrigatório, único, máximo 18 caracteres.</summary>
    public string Cnpj { get; private set; } = null!;

    /// <summary>Email da clínica. Opcional, máximo 200 caracteres.</summary>
    public string? Email { get; private set; }

    /// <summary>Telefone da clínica. Opcional, máximo 20 caracteres.</summary>
    public string? Telefone { get; private set; }

    /// <summary>Endereço da clínica. Opcional, máximo 300 caracteres.</summary>
    public string? Endereco { get; private set; }

    /// <summary>Data de cadastro. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataCadastro { get; private set; }

    /// <summary>Lista de pets cadastrados na clínica. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public virtual List<Pet> Pets { get; private set; }

    /// <summary>Lista de eventos clínicos. Não serializada no JSON para evitar loop infinito. N:N</summary>
    [JsonIgnore]
    public virtual List<EventoClinico> Eventos { get; private set; }

    public Clinica(
        string nome,
        string cnpj,
        string? email,
        string? telefone,
        string? endereco)
    {
        UpdateNome(nome);
        UpdateCnpj(cnpj);
        UpdateEmail(email);
        UpdateTelefone(telefone);
        UpdateEndereco(endereco);
    }

    /// <summary>Atualiza todos os campos editáveis da clínica. Usado no PUT.</summary>
    public void Transferir(
        string nome,
        string cnpj,
        string? email,
        string? telefone,
        string? endereco)
    {
        UpdateNome(nome);
        UpdateCnpj(cnpj);
        UpdateEmail(email);
        UpdateTelefone(telefone);
        UpdateEndereco(endereco);
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

    /// <summary>Atualiza o CNPJ. Não pode ser vazio, máximo 18 caracteres.</summary>
    public void UpdateCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new Exception("O CNPJ não pode ser vazio.");
        if (cnpj.Length > 18)
            throw new Exception("O CNPJ deve ter no máximo 18 caracteres.");
        Cnpj = cnpj.Trim();
    }

    /// <summary>Atualiza o email. Opcional, deve conter '@', máximo 200 caracteres.</summary>
    public void UpdateEmail(string? email)
    {
        if (email != null && !email.Contains('@'))
            throw new Exception("Informe um email válido.");
        if (email != null && email.Length > 200)
            throw new Exception("O email deve ter no máximo 200 caracteres.");
        Email = email?.Trim().ToLower();
    }

    /// <summary>Atualiza o telefone. Opcional, máximo 20 caracteres.</summary>
    public void UpdateTelefone(string? telefone)
    {
        if (telefone != null && telefone.Length > 20)
            throw new Exception("O telefone deve ter no máximo 20 caracteres.");
        Telefone = telefone;
    }

    /// <summary>Atualiza o endereço. Opcional, máximo 300 caracteres.</summary>
     public void UpdateEndereco(string? endereco)
    {
        if (endereco != null && endereco.Length > 300)
            throw new Exception("O endereço deve ter no máximo 300 caracteres.");
        Endereco = endereco;
    }

    public Clinica()
    {
        Pets = [];
        Eventos = [];
    }
}