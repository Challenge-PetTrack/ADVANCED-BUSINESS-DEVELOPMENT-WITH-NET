using System.Net;
using FluentAssertions;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Observability;

/// <summary>
/// Testes de integração do endpoint de métricas Prometheus (/metrics) via OpenTelemetry.
/// </summary>
public class MetricsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MetricsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMetrics_DeveRetornar200Ok_EExportarMetricasPrometheus()
    {
        // Act
        var response = await _client.GetAsync("/metrics");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrWhiteSpace();
        // Verifica que o formato de texto padrão do Prometheus/OpenTelemetry foi exportado
        (content.Contains("# TYPE") || content.Contains("# HELP") || content.Contains("http") || content.Contains("process") || content.Contains("pettrack")).Should().BeTrue();
    }
}
