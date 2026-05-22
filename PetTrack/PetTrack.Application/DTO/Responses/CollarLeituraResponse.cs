using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados da Leitura do Collar IoT.
/// </summary>
/// <param name="Id">Identificador único da Leitura gerado pelo Oracle.</param>
/// <param name="Temperatura">Temperatura corporal lida. Entre 30 e 45°C.</param>
/// <param name="Atividade">Nível de atividade física registrado. Opcional.</param>
/// <param name="DataLeitura">Data da leitura gerada automaticamente pelo Oracle.</param>
/// <param name="TopicoMqtt">Tópico MQTT de origem da leitura. Opcional.</param>
/// <param name="Pet">Dados do pet associado.</param>
public record CollarLeituraResponse(
    long Id,
    double Temperatura,
    double? Atividade,
    DateTime DataLeitura,
    string? TopicoMqtt,
    PetResponse Pet)
{
    /// <summary>Converte a entidade CollarLeitura para o DTO de resposta.</summary>
    public static CollarLeituraResponse ToDTO(CollarLeitura leitura) => new(
        leitura.Id,
        leitura.Temperatura,
        leitura.Atividade,
        leitura.DataLeitura,
        leitura.TopicoMqtt,
        PetResponse.ToDTO(leitura.Pet));
}
