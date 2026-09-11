using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para a API PetsController utilizando WebApplicationFactory e InMemory Database.
/// Valida contratos HTTP (200, 201, 204, 400, 404) e serialização JSON.
/// </summary>
public class PetsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PetsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200Ok_EListaDePets()
    {
        // Act
        var response = await _client.GetAsync("/api/pets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pets = await response.Content.ReadFromJsonAsync<List<PetResponse>>();
        pets.Should().NotBeNull();
        pets.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_QuandoPetExiste_DeveRetornar200Ok_EObjetoPet()
    {
        // Act
        var response = await _client.GetAsync("/api/pets/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pet = await response.Content.ReadFromJsonAsync<PetResponse>();
        pet.Should().NotBeNull();
        pet!.Id.Should().Be(1L);
        pet.Nome.Should().Be("Thor");
    }

    [Fact]
    public async Task GetById_QuandoPetNaoExiste_DeveRetornar404NotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/pets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ComPayloadValido_DeveRetornar201Created_ELocationHeader()
    {
        // Arrange
        var tutor = new Tutor("Gabriel Sbrana", "gabriel@pettrack.com", "11988887777", "Rua A");
        var clinica = new Clinica("Clyvo Vet Care", "12345678000199", "contato@clyvo.com", "1133334444", "Rua B");
        var request = new PetRequest("Mel", "Felina", "Siamês", SexoPetEnum.F, 2.0, 4.0, tutor, clinica);

        // Act
        var response = await _client.PostAsJsonAsync("/api/pets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var petCriado = await response.Content.ReadFromJsonAsync<PetResponse>();
        petCriado.Should().NotBeNull();
        petCriado!.Nome.Should().Be("Mel");
        petCriado.Especie.Should().Be("Felina");
    }

    [Fact]
    public async Task Delete_QuandoPetNaoExiste_DeveRetornar404NotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/pets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPetsComAlertasPendentes_DeveRetornar200Ok()
    {
        // Act
        var response = await _client.GetAsync("/api/pets/alertasPendentes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pets = await response.Content.ReadFromJsonAsync<List<PetResponse>>();
        pets.Should().NotBeNull();
    }
}
