using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para TutoresController.
/// </summary>
public class TutoresControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TutoresControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200Ok_EListaDeTutores()
    {
        // Act
        var response = await _client.GetAsync("/api/tutores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tutores = await response.Content.ReadFromJsonAsync<List<TutorResponse>>();
        tutores.Should().NotBeNull();
        tutores.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_QuandoTutorExiste_DeveRetornar200Ok_EObjetoTutor()
    {
        // Act
        var response = await _client.GetAsync("/api/tutores/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tutor = await response.Content.ReadFromJsonAsync<TutorResponse>();
        tutor.Should().NotBeNull();
        tutor!.Id.Should().Be(1L);
        tutor.Nome.Should().Be("Moisés Waidemann");
    }

    [Fact]
    public async Task GetById_QuandoTutorNaoExiste_DeveRetornar404NotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tutores/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ComPayloadValido_DeveRetornar201Created()
    {
        // Arrange
        var request = new TutorRequest("Richard Freitas", "richard.freitas@pettrack.com", "11966665555", "Rua das Flores, 123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/tutores", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var tutorCriado = await response.Content.ReadFromJsonAsync<TutorResponse>();
        tutorCriado.Should().NotBeNull();
        tutorCriado!.Nome.Should().Be("Richard Freitas");
        tutorCriado.Email.Should().Be("richard.freitas@pettrack.com");
    }

    [Fact]
    public async Task Delete_QuandoTutorNaoExiste_DeveRetornar404NotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tutores/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
