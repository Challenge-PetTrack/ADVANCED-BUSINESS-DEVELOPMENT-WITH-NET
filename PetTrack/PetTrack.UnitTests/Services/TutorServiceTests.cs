using FluentAssertions;
using Moq;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Entities;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Services;

/// <summary>
/// Testes unitários para TutorService utilizando Moq e o padrão AAA.
/// </summary>
public class TutorServiceTests
{
    private readonly Mock<ITutorRepository> _tutorRepositoryMock;
    private readonly TutorService _tutorService;

    public TutorServiceTests()
    {
        _tutorRepositoryMock = new Mock<ITutorRepository>();
        _tutorService = new TutorService(_tutorRepositoryMock.Object);
    }

    [Fact]
    public void Create_ComDadosValidos_DeveChamarRepositoryERetornarTutorResponse()
    {
        // Arrange
        var request = new TutorRequest("Gabriel Sbrana", "gabriel@pettrack.com", "11988881111", "Av Paulista, 1000");

        // Act
        var result = _tutorService.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("Gabriel Sbrana");
        result.Email.Should().Be("gabriel@pettrack.com");
        result.Telefone.Should().Be("11988881111");
        _tutorRepositoryMock.Verify(r => r.Add(It.Is<Tutor>(t => t.Nome == "Gabriel Sbrana")), Times.Once);
    }

    [Fact]
    public void GetById_QuandoTutorExiste_DeveRetornarTutorResponse()
    {
        // Arrange
        const long tutorId = 1L;
        var tutor = PetTrackTestFixtures.CriarTutorValido(id: tutorId);
        _tutorRepositoryMock.Setup(r => r.GetById(tutorId)).Returns(tutor);

        // Act
        var result = _tutorService.GetById(tutorId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(tutorId);
        result.Nome.Should().Be(tutor.Nome);
        _tutorRepositoryMock.Verify(r => r.GetById(tutorId), Times.Once);
    }

    [Fact]
    public void GetById_QuandoTutorNaoExiste_DeveRetornarNull()
    {
        // Arrange
        const long tutorId = 999L;
        _tutorRepositoryMock.Setup(r => r.GetById(tutorId)).Returns((Tutor?)null);

        // Act
        var result = _tutorService.GetById(tutorId);

        // Assert
        result.Should().BeNull();
        _tutorRepositoryMock.Verify(r => r.GetById(tutorId), Times.Once);
    }

    [Fact]
    public void GetAll_QuandoChamado_DeveRetornarListaDeTutores()
    {
        // Arrange
        var tutores = new List<Tutor>
        {
            PetTrackTestFixtures.CriarTutorValido(id: 1, nome: "Moisés"),
            PetTrackTestFixtures.CriarTutorValido(id: 2, nome: "Gabriel")
        };
        _tutorRepositoryMock.Setup(r => r.GetAll()).Returns(tutores);

        // Act
        var result = _tutorService.GetAll();

        // Assert
        result.Should().HaveCount(2);
        _tutorRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetByNomeOuEmail_QuandoChamado_DeveRetornarTutoresFiltrados()
    {
        // Arrange
        const string busca = "moises";
        var tutores = new List<Tutor> { PetTrackTestFixtures.CriarTutorValido(id: 1, nome: "Moisés") };
        _tutorRepositoryMock.Setup(r => r.GetByNomeOuEmail(busca)).Returns(tutores);

        // Act
        var result = _tutorService.GetByNomeOuEmail(busca);

        // Assert
        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("Moisés");
        _tutorRepositoryMock.Verify(r => r.GetByNomeOuEmail(busca), Times.Once);
    }

    [Fact]
    public void GetByNomePet_QuandoChamado_DeveRetornarTutoresDoPet()
    {
        // Arrange
        const string nomePet = "Thor";
        var tutores = new List<Tutor> { PetTrackTestFixtures.CriarTutorValido(id: 1) };
        _tutorRepositoryMock.Setup(r => r.GetByNomePet(nomePet)).Returns(tutores);

        // Act
        var result = _tutorService.GetByNomePet(nomePet);

        // Assert
        result.Should().HaveCount(1);
        _tutorRepositoryMock.Verify(r => r.GetByNomePet(nomePet), Times.Once);
    }

    [Fact]
    public void Update_QuandoTutorExiste_DeveAtualizarERetornarTutorResponse()
    {
        // Arrange
        const long tutorId = 1L;
        var tutorExistente = PetTrackTestFixtures.CriarTutorValido(id: tutorId);
        var request = new TutorRequest("Moisés Silva", "moises.silva@pettrack.com", "11911112222", "Rua Nova, 50");
        _tutorRepositoryMock.Setup(r => r.GetById(tutorId)).Returns(tutorExistente);

        // Act
        var result = _tutorService.Update(tutorId, request);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("Moisés Silva");
        result.Email.Should().Be("moises.silva@pettrack.com");
        _tutorRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_QuandoTutorNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        const long tutorId = 999L;
        var request = new TutorRequest("Inexistente", "inexistente@pettrack.com", null, null);
        _tutorRepositoryMock.Setup(r => r.GetById(tutorId)).Returns((Tutor?)null);

        // Act
        var act = () => _tutorService.Update(tutorId, request);

        // Assert
        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("Tutor não encontrado.");
        _tutorRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Delete_QuandoChamado_DeveRetornarResultadoDoRepository()
    {
        // Arrange
        const long tutorId = 1L;
        _tutorRepositoryMock.Setup(r => r.Delete(tutorId)).Returns(true);

        // Act
        var result = _tutorService.Delete(tutorId);

        // Assert
        result.Should().BeTrue();
        _tutorRepositoryMock.Verify(r => r.Delete(tutorId), Times.Once);
    }
}
