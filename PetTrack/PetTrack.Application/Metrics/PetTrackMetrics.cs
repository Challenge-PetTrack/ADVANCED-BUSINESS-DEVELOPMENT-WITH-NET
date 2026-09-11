using System.Diagnostics.Metrics;

namespace PetTrack.Application.Metrics;

/// <summary>
/// Provedor centralizado de métricas customizadas de negócio do PetTrack para Prometheus e OpenTelemetry.
/// </summary>
public static class PetTrackMetrics
{
    public const string MeterName = "PetTrack.Metrics";

    private static readonly Meter Meter = new(MeterName, "1.0.0");

    /// <summary>Contador de pets cadastrados.</summary>
    public static readonly Counter<long> PetsCreatedCounter = Meter.CreateCounter<long>(
        "pettrack_pets_created_total",
        unit: "pets",
        description: "Total de pets cadastrados no ecossistema PetTrack");

    /// <summary>Contador de tutores cadastrados.</summary>
    public static readonly Counter<long> TutoresCreatedCounter = Meter.CreateCounter<long>(
        "pettrack_tutores_created_total",
        unit: "tutores",
        description: "Total de tutores cadastrados");

    /// <summary>Contador de alertas clínicos e preventivos gerados.</summary>
    public static readonly Counter<long> AlertasCreatedCounter = Meter.CreateCounter<long>(
        "pettrack_alertas_created_total",
        unit: "alertas",
        description: "Total de alertas gerados para pets");

    /// <summary>Contador de leituras telemétricas do colar IoT processadas.</summary>
    public static readonly Counter<long> CollarReadingsCounter = Meter.CreateCounter<long>(
        "pettrack_collar_readings_total",
        unit: "leituras",
        description: "Total de leituras de sensores IoT de colares processadas");

    /// <summary>Contador de registros de adesão a medicamentos.</summary>
    public static readonly Counter<long> AdesaoMedicamentoCounter = Meter.CreateCounter<long>(
        "pettrack_adesao_medicamento_total",
        unit: "doses",
        description: "Total de registros de adesão e administração de medicamentos");
}
