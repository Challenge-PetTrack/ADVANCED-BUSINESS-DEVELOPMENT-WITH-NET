using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTrack.Domain.Entities;

/// <summary>
/// Entidade que representa a tabela TB_COLLAR_LEITURA no Oracle DB.
/// Registra leituras do collar IoT do pet.
/// </summary>
public class CollarLeitura
{
    /// <summary>Identificador. Gerado via SEQ_COLLAR no Oracle.</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; private set; }

    /// <summary>Temperatura corporal lida pelo collar. Entre 30 e 45°C.</summary>
    public double Temperatura { get; private set; }

    /// <summary>Nível de atividade física registrado pelo collar.</summary>
    public double? Atividade { get; private set; }

    /// <summary>Data da leitura. Gerada automaticamente pelo Oracle via SYSDATE.</summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime DataLeitura { get; private set; }

    /// <summary>Tópico MQTT de origem da leitura. Opcional, máximo 200 caracteres.</summary>
    public string? TopicoMqtt { get; private set; }

    /// <summary>Pet associado à leitura. FK para TB_PET. 1:1</summary>
    public Pet Pet { get; private set; } = null!;

    public CollarLeitura(
        double temperatura,
        double? atividade,
        string? topicoMqtt,
        Pet pet)
    {
        UpdateTemperatura(temperatura);
        UpdateAtividade(atividade);
        UpdateTopicoMqtt(topicoMqtt);
        UpdatePet(pet);
    }

    /// <summary>Atualiza todos os campos editáveis da leitura. Usado no PUT.</summary>
    public void transferir(
        double temperatura,
        double? atividade,
        string? topicoMqtt)
    {
        UpdateTemperatura(temperatura);
        UpdateAtividade(atividade);
        UpdateTopicoMqtt(topicoMqtt);
    }

    /// <summary>Atualiza a temperatura. Deve estar entre 30 e 45°C.</summary>
    public void UpdateTemperatura(double temperatura)
    {
        if (temperatura < 30 || temperatura > 45)
            throw new Exception("Temperatura deve estar entre 30°C e 45°C.");
        Temperatura = temperatura;
    }

    /// <summary>Atualiza a atividade. Deve ser maior ou igual a zero.</summary>
    public void UpdateAtividade(double? atividade)
    {
        if (atividade < 0)
            throw new Exception("A atividade não pode ser negativa.");
        Atividade = atividade;
    }

    /// <summary>Atualiza o tópico MQTT. Opcional, máximo 200 caracteres.</summary>
    public void UpdateTopicoMqtt(string? topicoMqtt)
    {
        if (topicoMqtt != null && topicoMqtt.Length > 200)
            throw new Exception("O tópico MQTT deve ter no máximo 200 caracteres.");
        TopicoMqtt = topicoMqtt;
    }

    /// <summary>Atualiza o pet associado. Não pode ser nulo.</summary>
    public void UpdatePet(Pet pet)
    {
        if (pet is null)
            throw new Exception("O pet não pode ser nulo.");
        Pet = pet;
    }

    public CollarLeitura() { }
}