using FluentAssertions;
using Moq;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using Xunit;

namespace PetTrack.UnitTests.Services;

/// <summary>
/// Testes unitários para o serviço NoSQL de Pets no MongoDB (padrão AAA).
/// </summary>
public class PetMongoServiceTests
{
    private readonly Mock<IPetMongoRepository> _mongoRepoMock;
    private readonly Mock<IPetRepository> _relationalRepoMock;
    private readonly PetMongoService _service;

    public PetMongoServiceTests()
    {
        _mongoRepoMock = new Mock<IPetMongoRepository>();
        _relationalRepoMock = new Mock<IPetRepository>();
        _service = new PetMongoService(_mongoRepoMock.Object, _relationalRepoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeDocumentosComLinksHateoas()
    {
        // Arrange
        var doc = new PetMongoDocument
        {
            Id = 1,
            IdPet = 1,
            Nome = "Thor",
            Especie = "Cão",
            Raca = "Golden Retriever",
            PesoKg = 12.0
        };
        _mongoRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PetMongoDocument> { doc });

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Nome.Should().Be("Thor");
        result[0].Links.Should().NotBeEmpty();
        result[0].Links.Should().Contain(l => l.Rel == "self" && l.Href == "/api/mongopets/1");
    }

    [Fact]
    public async Task GetByIdAsync_QuandoExiste_DeveRetornarDtoComLinks()
    {
        // Arrange
        var doc = new PetMongoDocument
        {
            Id = 1,
            IdPet = 1,
            Nome = "Thor",
            Especie = "Cão"
        };
        _mongoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(doc);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Nome.Should().Be("Thor");
        result.Links.Should().Contain(l => l.Rel == "self");
    }

    [Fact]
    public async Task GetByIdAsync_QuandoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        _mongoRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((PetMongoDocument?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ComDadosValidos_DeveChamarInsertERetornarDto()
    {
        // Arrange
        var request = new PetMongoRequest("Bob", "Canina", "Bulldog", 15.0, 1, "Ana Silva", 1, "VetLife");

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("Bob");
        result.Especie.Should().Be("Canina");
        _mongoRepoMock.Verify(r => r.InsertAsync(It.IsAny<PetMongoDocument>()), Times.Once);
    }

    [Theory]
    [InlineData("", "Canina")]
    [InlineData("Thor", "")]
    [InlineData(null, "Canina")]
    public async Task CreateAsync_ComDadosInvalidos_DeveLancarArgumentException(string? nome, string? especie)
    {
        // Arrange
        var request = new PetMongoRequest(nome!, especie!, "SRD", 5.0, 1, "Tutor", 1, "Clinica");

        // Act
        var act = async () => await _service.CreateAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_QuandoNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        _mongoRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((PetMongoDocument?)null);
        var request = new PetMongoRequest("Thor", "Canina", "Golden", 12.0, 1, "Tutor", 1, "Clinica");

        // Act
        var act = async () => await _service.UpdateAsync(999, request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_DeveChamarRepositorioERetornarResultado()
    {
        // Arrange
        _mongoRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mongoRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task SyncRelationalToMongoAsync_DeveLerRelacionalEInserirNoMongo()
    {
        // Arrange
        var tutor = new Tutor("Moisés", "moises@pettrack.com", "11999998888", "Rua X");
        var clinica = new Clinica("Clyvo Vet", "12345678000199", "contato@clyvo.com", "1133334444", "Rua Y");
        var pet = new Pet("Thor", "Canina", "Golden Retriever", SexoPetEnum.M, 3.0, 25.0, tutor, clinica);

        _relationalRepoMock.Setup(r => r.GetAll()).Returns(new List<Pet> { pet });
        _mongoRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((PetMongoDocument?)null);

        // Act
        var count = await _service.SyncRelationalToMongoAsync();

        // Assert
        count.Should().Be(1);
        _mongoRepoMock.Verify(r => r.InsertAsync(It.IsAny<PetMongoDocument>()), Times.Once);
    }
}
