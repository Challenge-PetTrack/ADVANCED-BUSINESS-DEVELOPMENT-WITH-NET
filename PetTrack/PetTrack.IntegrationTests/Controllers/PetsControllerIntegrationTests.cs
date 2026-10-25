using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PetTrack.Application.DTO.Common;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para a API PetsController utilizando WebApplicationFactory e InMemory Database.
/// Valida contratos HTTP (200, 201, 204, 400, 401, 404), Autenticação JWT, Paginação e HATEOAS.
/// </summary>
public class PetsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly HttpClient _authClient;

    public PetsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _authClient = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200Ok_EListaDePetsComLinksHateoas()
    {
        // Act
        var response = await _client.GetAsync("/api/pets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pets = await response.Content.ReadFromJsonAsync<List<PetResponse>>();
        pets.Should().NotBeNull();
        pets.Should().NotBeEmpty();
        pets![0].Links.Should().NotBeEmpty();
        pets[0].Links.Should().Contain(l => l.Rel == "self");
    }

    [Fact]
    public async Task GetById_QuandoPetExiste_DeveRetornar200Ok_EObjetoPetComLinksHateoas()
    {
        // Act
        var response = await _client.GetAsync("/api/pets/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pet = await response.Content.ReadFromJsonAsync<PetResponse>();
        pet.Should().NotBeNull();
        pet!.Id.Should().Be(1L);
        pet.Nome.Should().Be("Thor");
        pet.Links.Should().NotBeEmpty();
        pet.Links.Should().Contain(l => l.Rel == "self" && l.Href == "/api/pets/1");
        pet.Links.Should().Contain(l => l.Rel == "update" && l.Method == "PUT");
        pet.Links.Should().Contain(l => l.Rel == "delete" && l.Method == "DELETE");
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
    public async Task GetPaged_DeveRetornar200Ok_EObjetoPaginadoComMetadadosEHateoas()
    {
        // Act
        var response = await _client.GetAsync("/api/pets/paged?pageNumber=1&pageSize=5&sortBy=Nome&sortDirection=asc");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paged = await response.Content.ReadFromJsonAsync<PagedResult<PetResponse>>();
        paged.Should().NotBeNull();
        paged!.Items.Should().NotBeEmpty();
        paged.PageNumber.Should().Be(1);
        paged.PageSize.Should().Be(5);
        paged.TotalItems.Should().BeGreaterThan(0);
        paged.Links.Should().NotBeEmpty();
        paged.Links.Should().Contain(l => l.Rel == "self");
        paged.Links.Should().Contain(l => l.Rel == "first");
    }

    [Fact]
    public async Task Create_SemToken_DeveRetornar401Unauthorized()
    {
        // Arrange
        var tutor = new Tutor("Gabriel Sbrana", "gabriel@pettrack.com", "11988887777", "Rua A");
        var clinica = new Clinica("Clyvo Vet Care", "12345678000199", "contato@clyvo.com", "1133334444", "Rua B");
        var request = new PetRequest("Mel", "Felina", "Siamês", SexoPetEnum.F, 2.0, 4.0, tutor, clinica);

        // Act
        var response = await _client.PostAsJsonAsync("/api/pets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ComPayloadValidoEAutenticado_DeveRetornar201Created_ELocationHeader()
    {
        // Arrange
        var tutor = new Tutor("Gabriel Sbrana", "gabriel@pettrack.com", "11988887777", "Rua A");
        var clinica = new Clinica("Clyvo Vet Care", "12345678000199", "contato@clyvo.com", "1133334444", "Rua B");
        var request = new PetRequest("Mel", "Felina", "Siamês", SexoPetEnum.F, 2.0, 4.0, tutor, clinica);

        // Act
        var response = await _authClient.PostAsJsonAsync("/api/pets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var petCriado = await response.Content.ReadFromJsonAsync<PetResponse>();
        petCriado.Should().NotBeNull();
        petCriado!.Nome.Should().Be("Mel");
        petCriado.Especie.Should().Be("Felina");
        petCriado.Links.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_SemToken_DeveRetornar401Unauthorized()
    {
        // Act
        var response = await _client.DeleteAsync("/api/pets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_QuandoPetNaoExisteEAutenticado_DeveRetornar404NotFound()
    {
        // Act
        var response = await _authClient.DeleteAsync("/api/pets/99999");

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
