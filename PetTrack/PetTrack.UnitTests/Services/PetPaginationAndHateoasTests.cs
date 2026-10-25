using FluentAssertions;
using Moq;
using PetTrack.Application.DTO.Common;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using Xunit;

namespace PetTrack.UnitTests.Services;

/// <summary>
/// Testes unitários para regras de paginação, filtros, ordenação e hipermídia HATEOAS (padrão AAA).
/// </summary>
public class PetPaginationAndHateoasTests
{
    private readonly Mock<IPetRepository> _repositoryMock;
    private readonly PetService _petService;

    public PetPaginationAndHateoasTests()
    {
        _repositoryMock = new Mock<IPetRepository>();
        _petService = new PetService(_repositoryMock.Object);
    }

    [Fact]
    public void EnrichWithLinks_DeveAdicionarTodosLinksHateoasNecessarios()
    {
        // Arrange
        var tutor = new Tutor("Moisés", "moises@pettrack.com", "11999998888", "Rua X");
        var clinica = new Clinica("Clyvo Vet", "12345678000199", "contato@clyvo.com", "1133334444", "Rua Y");
        var pet = new Pet("Thor", "Canina", "Golden Retriever", SexoPetEnum.M, 3.0, 25.0, tutor, clinica);
        var dto = PetResponse.ToDTO(pet);

        // Act
        var enriched = PetService.EnrichWithLinks(dto);

        // Assert
        enriched.Links.Should().NotBeEmpty();
        enriched.Links.Should().HaveCount(5);
        enriched.Links.Should().Contain(l => l.Rel == "self" && l.Href == $"/api/pets/{dto.Id}" && l.Method == "GET");
        enriched.Links.Should().Contain(l => l.Rel == "update" && l.Method == "PUT");
        enriched.Links.Should().Contain(l => l.Rel == "delete" && l.Method == "DELETE");
        enriched.Links.Should().Contain(l => l.Rel == "all" && l.Href == "/api/pets" && l.Method == "GET");
        enriched.Links.Should().Contain(l => l.Rel == "paged" && l.Href == "/api/pets/paged" && l.Method == "GET");
    }

    [Fact]
    public void GetPaged_ComParametrosValidos_DeveRetornarPaginacaoCorretaComLinksDeNavegacao()
    {
        // Arrange
        var tutor = new Tutor("Moisés", "moises@pettrack.com", "11999998888", "Rua X");
        var clinica = new Clinica("Clyvo Vet", "12345678000199", "contato@clyvo.com", "1133334444", "Rua Y");
        var pet1 = new Pet("Thor", "Canina", "Golden Retriever", SexoPetEnum.M, 3.0, 25.0, tutor, clinica);
        var pet2 = new Pet("Luna", "Canina", "Poodle", SexoPetEnum.F, 2.0, 8.0, tutor, clinica);

        var filters = new PetFilterParams
        {
            PageNumber = 1,
            PageSize = 2,
            SortBy = "Nome",
            SortDirection = "asc"
        };

        _repositoryMock.Setup(r => r.GetPaged(
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            It.IsAny<SexoPetEnum?>(),
            It.IsAny<long?>(),
            It.IsAny<long?>(),
            It.IsAny<string?>(),
            It.IsAny<string?>(),
            1,
            2))
            .Returns((new List<Pet> { pet1, pet2 }, 10));

        // Act
        var result = _petService.GetPaged(filters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalItems.Should().Be(10);
        result.TotalPages.Should().Be(5);
        result.HasNextPage.Should().BeTrue();
        result.HasPreviousPage.Should().BeFalse();
        result.Links.Should().Contain(l => l.Rel == "self");
        result.Links.Should().Contain(l => l.Rel == "next");
        result.Links.Should().Contain(l => l.Rel == "first");
        result.Links.Should().Contain(l => l.Rel == "last");
    }

    [Fact]
    public void PaginationParams_QuandoValoresInvalidos_DeveNormalizarParaPadrao()
    {
        // Arrange
        var p = new PaginationParams
        {
            PageNumber = -5,
            PageSize = 100
        };

        // Assert
        p.PageNumber.Should().Be(1);
        p.PageSize.Should().Be(50); // limitador máximo
    }

    [Theory]
    [InlineData(10, 5, 2)]
    [InlineData(11, 5, 3)]
    [InlineData(0, 5, 0)]
    [InlineData(5, 5, 1)]
    public void PagedResult_CalculoTotalPages_DeveCalcularCorretamente(int totalItems, int pageSize, int expectedPages)
    {
        // Act
        var paged = new PagedResult<string>(new List<string>(), totalItems, 1, pageSize);

        // Assert
        paged.TotalPages.Should().Be(expectedPages);
    }
}
