using System.Net;
using FluentAssertions;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Observability;

/// <summary>
/// Testes de integração dos endpoints de monitoramento de integridade (Health Checks).
/// </summary>
public class HealthChecksIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthChecksIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_DeveRetornarStatusEFormatoJson()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrWhiteSpace();
        content.Should().Contain("status");
    }

    [Fact]
    public async Task GetHealthLive_DeveRetornar200Ok_ELivenessProbe()
    {
        // Act
        var response = await _client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Healthy");
        content.Should().Contain("self");
    }

    [Fact]
    public async Task GetHealthReady_DeveRetornarReadinessProbeFormatado()
    {
        // Act
        var response = await _client.GetAsync("/health/ready");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrWhiteSpace();
        content.Should().Contain("status");
    }
}
