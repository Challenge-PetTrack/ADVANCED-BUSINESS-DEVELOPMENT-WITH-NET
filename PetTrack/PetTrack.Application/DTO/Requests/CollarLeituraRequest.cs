using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar uma Leitura do Collar IoT.
/// </summary>
/// <param name="Temperatura">Temperatura corporal lida. Entre 30 e 45°C.</param>
/// <param name="Atividade">Nível de atividade física registrado. Opcional.</param>
/// <param name="TopicoMqtt">Tópico MQTT de origem da leitura. Opcional.</param>
/// <param name="Pet">Pet associado.</param>
public record CollarLeituraRequest(
    double Temperatura,
    double? Atividade,
    string? TopicoMqtt,
    Pet Pet)
{
    /// <summary>Converte o DTO para a entidade de domínio CollarLeitura.</summary>
    public CollarLeitura ToEntity() => new CollarLeitura(Temperatura, Atividade, TopicoMqtt, Pet);
}