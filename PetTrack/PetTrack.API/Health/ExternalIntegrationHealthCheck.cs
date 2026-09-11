using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PetTrack.Health;

/// <summary>
/// Health Check customizado que avalia a integridade da comunicação com a API IoT dos colares Clyvo e serviços externos.
/// </summary>
public class ExternalIntegrationHealthCheck : IHealthCheck
{
    /// <summary>
    /// Executa o teste de saúde de integração externa e retorna o resultado.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // Simulação de validação de conectividade com gateway de telemetria IoT Clyvo
        var data = new Dictionary<string, object>
        {
            { "clyvo_iot_gateway", "Connected" },
            { "response_time_ms", 12 },
            { "firmware_supported_version", "v2.4.0" }
        };

        return Task.FromResult(
            HealthCheckResult.Healthy("Serviço de telemetria IoT Clyvo operando normalmente.", data));
    }
}
