using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetTrack.Domain.Enum;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_PROTOCOLO_PREVENTIVO no Oracle DB.
/// </summary>
public class ProtocoloPreventivo
{
    /// <summary>Identificador. Gerado via SEQ_PROTOCOLO no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Tipo do protocolo: ANTIPULGA, CHECKUP, VACINA ou VERMIFUGO.</summary>
    public TipoProtocoloPreventivoEnum Tipo { get; private set; }

    /// <summary>Nome do protocolo preventivo. Obrigatório, máximo 200 caracteres.</summary>
    public string Nome { get; private set; } = null!;

    /// <summary>Data de aplicação do protocolo. Opcional.</summary>
    public DateTime? DateAplicacao { get; private set; }

    /// <summary>Data da próxima aplicação. Opcional.</summary>
    public DateTime? DateProxima { get; private set; }

    /// <summary>Status do protocolo: ATRASADO, PENDENTE ou REALIZADO.</summary>
    public StatusProtocoloPreventivoEnum Status { get; private set; }

    /// <summary>Pet associado ao protocolo. FK para TB_PET. 1:1</summary>
    public virtual Pet Pet { get; private set; } = null!;

    public ProtocoloPreventivo(
        TipoProtocoloPreventivoEnum tipo,
        string nome,
        DateTime? dateAplicacao,
        DateTime? dateProxima,
        StatusProtocoloPreventivoEnum status,
        Pet pet)
    {
        Tipo = tipo;
        UpdateNome(nome);
        UpdateDatas(dateAplicacao, dateProxima);
        Status = status;
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis do protocolo. Usado no PUT.</summary>
    public void Transferir(
        TipoProtocoloPreventivoEnum tipo,
        string nome,
        DateTime? dateAplicacao,
        DateTime? dateProxima,
        StatusProtocoloPreventivoEnum status,
        Pet pet)
    {
        Tipo = tipo;
        UpdateNome(nome);
        UpdateDatas(dateAplicacao, dateProxima);
        Status = status;
        UpdatePet(pet);
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

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("Pet não pode ser nulo.");
        Pet = pet;
    }
    
    /// <summary>Atualiza o as datas e verifica se a data proxima é depois da aplicada.</summary>
    public void UpdateDatas(DateTime? dateAplicacao, DateTime? dateProxima)
    {
        if (dateAplicacao.HasValue && dateProxima.HasValue && dateProxima <= dateAplicacao)
            throw new Exception("A próxima aplicação deve ser após a data de aplicação.");
        DateAplicacao = dateAplicacao;
        DateProxima = dateProxima;
    }
    
    public ProtocoloPreventivo() { }
}