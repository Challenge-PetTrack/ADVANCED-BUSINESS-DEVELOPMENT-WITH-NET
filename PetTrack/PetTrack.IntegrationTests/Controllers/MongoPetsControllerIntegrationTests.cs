using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.IntegrationTests.Fixtures;
using Xunit;

namespace PetTrack.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para operações NoSQL com MongoDB via MongoPetsController.
/// </summary>
public class MongoPetsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly HttpClient _authClient;

    public MongoPetsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _authClient = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200Ok_EListaDeDocumentosMongo()
    {
        // Act
        var response = await _client.GetAsync("/api/mongopets");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pets = await response.Content.ReadFromJsonAsync<List<PetMongoResponse>>();
        pets.Should().NotBeNull();
        pets.Should().NotBeEmpty();
        pets![0].Links.Should().NotBeEmpty();
        pets[0].Links.Should().Contain(l => l.Rel == "self");
    }

    [Fact]
    public async Task GetById_QuandoExiste_DeveRetornar200Ok_EObjetoComLinks()
    {
        // Act
        var response = await _client.GetAsync("/api/mongopets/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var pet = await response.Content.ReadFromJsonAsync<PetMongoResponse>();
        pet.Should().NotBeNull();
        pet!.Id.Should().Be(1);
        pet.Nome.Should().Be("Thor");
        pet.Links.Should().NotBeEmpty();
        pet.Links.Should().Contain(l => l.Rel == "self" && l.Href == "/api/mongopets/1");
    }

    [Fact]
    public async Task GetById_QuandoNaoExiste_DeveRetornar404NotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/mongopets/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_SemToken_DeveRetornar401Unauthorized()
    {
        // Arrange
        var request = new PetMongoRequest("Max", "Canina", "SRD", 10.3, 2, "Carlos Santos", 1, "VetLife");

        // Act
        var response = await _client.PostAsJsonAsync("/api/mongopets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ComTokenValido_DeveRetornar201Created_ELocationHeader()
    {
        // Arrange
        var request = new PetMongoRequest("Max", "Canina", "SRD", 10.3, 2, "Carlos Santos", 1, "VetLife");

        // Act
        var response = await _authClient.PostAsJsonAsync("/api/mongopets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var criado = await response.Content.ReadFromJsonAsync<PetMongoResponse>();
        criado.Should().NotBeNull();
        criado!.Nome.Should().Be("Max");
    }

    [Fact]
    public async Task Update_ComTokenValido_DeveRetornar200Ok()
    {
        // Arrange (atualiza ID 2)
        var request = new PetMongoRequest("Luna Atualizada", "Canina", "Poodle", 11.0, 2, "Carlos Santos", 1, "VetLife");

        // Act
        var response = await _authClient.PutAsJsonAsync("/api/mongopets/2", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var atualizado = await response.Content.ReadFromJsonAsync<PetMongoResponse>();
        atualizado.Should().NotBeNull();
        atualizado!.Nome.Should().Be("Luna Atualizada");
    }

    [Fact]
    public async Task Delete_SemToken_DeveRetornar401Unauthorized()
    {
        // Act
        var response = await _client.DeleteAsync("/api/mongopets/3");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Delete_ComTokenValido_DeveRetornar204NoContent()
    {
        // Act (remove ID 3)
        var response = await _authClient.DeleteAsync("/api/mongopets/3");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task SyncRelationalToMongo_ComTokenValido_DeveRetornar200Ok()
    {
        // Act
        var response = await _authClient.PostAsync("/api/mongopets/sync", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
