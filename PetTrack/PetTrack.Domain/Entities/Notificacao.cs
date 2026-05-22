using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_NOTIFICACAO no Oracle DB.
/// </summary>
public class Notificacao
{
    /// <summary>Identificador. Gerado via SEQ_NOTIFICACAO no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Tipo da notificação: ALERTA, INFO, LEMBRETE ou URGENTE.</summary>
    public TipoNotificacaoEnum Tipo { get; private set; }

    /// <summary>Título da notificação. Obrigatório, máximo 200 caracteres.</summary>
    public string Titulo { get; private set; } = null!;

    /// <summary>Mensagem da notificação. Opcional, máximo 2000 caracteres.</summary>
    public string? Mensagem { get; private set; }

    /// <summary>Data de envio. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataEnvio { get; private set; }

    /// <summary>Status de leitura: S (lida) ou N (não lida).</summary>
    public SimNaoEnum Status { get; private set; }

    /// <summary>Tutor associado à notificação. FK para TB_TUTOR. 1:1</summary>
    public Tutor Tutor { get; private set; } = null!;

    /// <summary>Pet associado à notificação. FK para TB_PET. 1:1</summary>
    public Pet Pet { get; private set; } = null!;

    public Notificacao(
        TipoNotificacaoEnum tipo,
        string titulo,
        string? mensagem,
        SimNaoEnum status,
        Tutor tutor,
        Pet pet)
    {
        Tipo = tipo;
        UpdateTitulo(titulo);
        UpdateMensagem(mensagem);
        Status = status;
        UpdateTutor(tutor);
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis da notificação. Usado no PUT.</summary>
    public void Transferir(
        TipoNotificacaoEnum tipo,
        string titulo,
        string? mensagem,
        SimNaoEnum status,
        Tutor tutor,
        Pet pet)
    {
        Tipo = tipo;
        UpdateTitulo(titulo);
        UpdateMensagem(mensagem);
        Status = status;
        UpdateTutor(tutor);
        UpdatePet(pet);
    }

    /// <summary>Atualiza o título. Não pode ser vazio, máximo 200 caracteres.</summary>
   public void UpdateTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("O título não pode ser vazio.");
        if (titulo.Length > 200)
            throw new Exception("O título deve ter no máximo 200 caracteres.");
        Titulo = titulo.Trim();
    }

    /// <summary>Atualiza a mensagem. Opcional, máximo 2000 caracteres.</summary>
    public void UpdateMensagem(string? mensagem)
    {
        if (mensagem != null && mensagem.Length > 2000)
            throw new Exception("A mensagem deve ter no máximo 2000 caracteres.");
        Mensagem = mensagem;
    }

    /// <summary>Atualiza o tutor associado. Não pode ser nulo.</summary>
    public void UpdateTutor(Tutor tutor)
    {
        if (tutor is null)
            throw new Exception("O tutor não pode ser nulo.");
        Tutor = tutor;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("O pet não pode ser nulo.");
        Pet = pet;
    }

     public Notificacao() { }
}