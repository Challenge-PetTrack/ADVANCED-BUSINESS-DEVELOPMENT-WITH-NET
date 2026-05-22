using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a table TB_TUTOR.
/// </summary>
public class Tutor
{
    /// <summary>Identificador.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }
    /// <summary>Nome do tutor.</summary>
    public string Nome { get; private set; }
    /// <summary>Email do tutor.</summary>
    public string Email { get; private set; }
    /// <summary>Telefone do tutor.</summary>
    public string? Telefone { get; private set; }
    /// <summary>Endereço do tutor.</summary>
    public string? Endereco { get; private set; }
    /// <summary>Data de cadastro. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
    public DateTime DataCadastro { get; private set; }
    /// <summary>Representa fk de tutor na tabela de pet. N:N</summary>
    [JsonIgnore]
    public List<Pet> Pets { get; private set; }
    /// <summary>Representa fk de tutor na tabela de notificação. N:N</summary>
    [JsonIgnore]
    public List<Notificacao> Notificacoes { get; private set; }
    
    public Tutor(
        string nome,
        string email,
        string? telefone = null,
        string? endereco = null)
    {
        UpdateNome(nome);
        UpdateEmail(email);
        UpdateTelefone(telefone);
        UpdateEndereco(endereco);
    }
    
    /// <summary>Atualiza todos os campos editáveis do tutor (usado no PUT).</summary>
    public void Transferir(string nome, string email, string? telefone, string? endereco)
    {
        UpdateNome(nome);
        UpdateEmail(email);
        UpdateTelefone(telefone);
        UpdateEndereco(endereco);
    }
    
    /// <summary>Atualiza o telefone. Não permite valor vazio e tamanho maior que 20.</summary>
    public void UpdateTelefone(string? telefone)
    {
        if (telefone != null && telefone.Length > 20)
            throw new Exception("Telefone deve ter no máximo 20 caracteres.");
        Telefone = telefone;
    }

    /// <summary>Atualiza o endereco. Não permite valor vazio e tamanho maior que 300.</summary>
    public void UpdateEndereco(string? endereco)
    {
        if (endereco != null && endereco.Length > 300)
            throw new Exception("Endereço deve ter no máximo 300 caracteres.");
        Endereco = endereco;
    }

    /// <summary>Atualiza o nome. Não permite valor vazio.</summary>
    public void UpdateNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new Exception("O nome não pode ser vazio.");
        Nome = nome.Trim();
    }

    /// <summary>Atualiza o email. Deve ser válido e não vazio.</summary>
    public void UpdateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new Exception("O email não pode ser vazio.");
        if (!email.Contains('@'))
            throw new Exception("Informe um email válido.");
        Email = email.Trim().ToLower();
    }

    public Tutor()
    {
        Pets = [];
        Notificacoes = [];
    }
}