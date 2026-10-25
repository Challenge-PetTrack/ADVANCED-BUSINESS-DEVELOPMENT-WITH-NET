using Microsoft.Extensions.Diagnostics.HealthChecks;
using PetTrack.Infrastructure.Persistence.Mongo;

namespace PetTrack.Health;

/// <summary>
/// Health Check responsável por verificar a integridade da conexão com a base NoSQL MongoDB.
/// Atende ao requisito do edital para validação contínua dos bancos Oracle e MongoDB.
/// </summary>
public class MongoDbHealthCheck : IHealthCheck
{
    private readonly PetTrackMongoContext _mongoContext;

    public MongoDbHealthCheck(PetTrackMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = await _mongoContext.PingAsync(cancellationToken);

        if (isHealthy)
        {
            return HealthCheckResult.Healthy(
                "Conexão com o banco NoSQL MongoDB (pettrack_sprint4) ativa e respondendo.",
                new Dictionary<string, object>
                {
                    ["database"] = "pettrack_sprint4",
                    ["host"] = "127.0.0.1:27017",
                    ["status"] = "Connected"
                });
        }

        return HealthCheckResult.Unhealthy("Não foi possível conectar ao banco NoSQL MongoDB.");
    }
}
