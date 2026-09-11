using FluentAssertions;
using Moq;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Services;

/// <summary>
/// Testes unitários para PetService utilizando Moq e o padrão AAA (Arrange, Act, Assert).
/// </summary>
public class PetServiceTests
{
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly PetService _petService;

    public PetServiceTests()
    {
        _petRepositoryMock = new Mock<IPetRepository>();
        _petService = new PetService(_petRepositoryMock.Object);
    }

    [Fact]
    public void Create_ComDadosValidos_DeveChamarRepositoryERetornarPetResponse()
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();
        var clinica = PetTrackTestFixtures.CriarClinicaValida();
        var request = new PetRequest("Thor", "Canina", "Golden Retriever", SexoPetEnum.M, 3.0, 28.0, tutor, clinica);

        // Act
        var result = _petService.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("Thor");
        result.Especie.Should().Be("Canina");
        result.Raca.Should().Be("Golden Retriever");
        result.Sexo.Should().Be(SexoPetEnum.M);
        result.Idade.Should().Be(3.0);
        result.Peso.Should().Be(28.0);
        _petRepositoryMock.Verify(r => r.Add(It.Is<Pet>(p => p.Nome == "Thor")), Times.Once);
    }

    [Fact]
    public void GetById_QuandoPetExiste_DeveRetornarPetResponse()
    {
        // Arrange
        const long petId = 1L;
        var pet = PetTrackTestFixtures.CriarPetValido(id: petId);
        _petRepositoryMock.Setup(r => r.GetById(petId)).Returns(pet);

        // Act
        var result = _petService.GetById(petId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(petId);
        result.Nome.Should().Be(pet.Nome);
        _petRepositoryMock.Verify(r => r.GetById(petId), Times.Once);
    }

    [Fact]
    public void GetById_QuandoPetNaoExiste_DeveRetornarNull()
    {
        // Arrange
        const long petId = 999L;
        _petRepositoryMock.Setup(r => r.GetById(petId)).Returns((Pet?)null);

        // Act
        var result = _petService.GetById(petId);

        // Assert
        result.Should().BeNull();
        _petRepositoryMock.Verify(r => r.GetById(petId), Times.Once);
    }

    [Fact]
    public void GetAll_QuandoChamado_DeveRetornarListaDePets()
    {
        // Arrange
        var pets = new List<Pet>
        {
            PetTrackTestFixtures.CriarPetValido(id: 1, nome: "Thor"),
            PetTrackTestFixtures.CriarPetValido(id: 2, nome: "Luna")
        };
        _petRepositoryMock.Setup(r => r.GetAll()).Returns(pets);

        // Act
        var result = _petService.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result[0].Nome.Should().Be("Thor");
        result[1].Nome.Should().Be("Luna");
        _petRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetByClinicaId_QuandoExistemPets_DeveRetornarPetsDaClinica()
    {
        // Arrange
        const long clinicaId = 1L;
        var pets = new List<Pet> { PetTrackTestFixtures.CriarPetValido(id: 1, nome: "Thor") };
        _petRepositoryMock.Setup(r => r.GetByClinicaId(clinicaId)).Returns(pets);

        // Act
        var result = _petService.GetByClinicaId(clinicaId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Nome.Should().Be("Thor");
        _petRepositoryMock.Verify(r => r.GetByClinicaId(clinicaId), Times.Once);
    }

    [Fact]
    public void GetBySexo_QuandoChamado_DeveRetornarPetsDoSexo()
    {
        // Arrange
        const SexoPetEnum sexo = SexoPetEnum.M;
        var pets = new List<Pet> { PetTrackTestFixtures.CriarPetValido(id: 1, sexo: sexo) };
        _petRepositoryMock.Setup(r => r.GetBySexo(sexo)).Returns(pets);

        // Act
        var result = _petService.GetBySexo(sexo);

        // Assert
        result.Should().HaveCount(1);
        result.First().Sexo.Should().Be(SexoPetEnum.M);
        _petRepositoryMock.Verify(r => r.GetBySexo(sexo), Times.Once);
    }

    [Fact]
    public void GetByNomeOuEspecie_QuandoExisteCorrespondencia_DeveRetornarPetsFiltrados()
    {
        // Arrange
        const string busca = "Canina";
        var pets = new List<Pet> { PetTrackTestFixtures.CriarPetValido(id: 1, especie: "Canina") };
        _petRepositoryMock.Setup(r => r.GetByNomeOuEspecie(busca)).Returns(pets);

        // Act
        var result = _petService.GetByNomeOuEspecie(busca);

        // Assert
        result.Should().HaveCount(1);
        result.First().Especie.Should().Be("Canina");
        _petRepositoryMock.Verify(r => r.GetByNomeOuEspecie(busca), Times.Once);
    }

    [Fact]
    public void GetPetsComAlertasPendentes_QuandoChamado_DeveRetornarPetsComAlertas()
    {
        // Arrange
        var pets = new List<Pet> { PetTrackTestFixtures.CriarPetValido(id: 1, nome: "Thor") };
        _petRepositoryMock.Setup(r => r.GetPetsComAlertasPendentes()).Returns(pets);

        // Act
        var result = _petService.GetPetsComAlertasPendentes();

        // Assert
        result.Should().HaveCount(1);
        _petRepositoryMock.Verify(r => r.GetPetsComAlertasPendentes(), Times.Once);
    }

    [Fact]
    public void Update_QuandoPetExiste_DeveAtualizarERetornarPetResponse()
    {
        // Arrange
        const long petId = 1L;
        var petExistente = PetTrackTestFixtures.CriarPetValido(id: petId, nome: "Thor");
        var novoTutor = PetTrackTestFixtures.CriarTutorValido(id: 2);
        var novaClinica = PetTrackTestFixtures.CriarClinicaValida(id: 2);
        var request = new PetRequest("Thor Atualizado", "Canina", "Labrador", SexoPetEnum.M, 4.0, 30.0, novoTutor, novaClinica);

        _petRepositoryMock.Setup(r => r.GetById(petId)).Returns(petExistente);

        // Act
        var result = _petService.Update(petId, request);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("Thor Atualizado");
        result.Raca.Should().Be("Labrador");
        result.Idade.Should().Be(4.0);
        result.Peso.Should().Be(30.0);
        _petRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_QuandoPetNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        const long petId = 999L;
        var tutor = PetTrackTestFixtures.CriarTutorValido();
        var clinica = PetTrackTestFixtures.CriarClinicaValida();
        var request = new PetRequest("Inexistente", "Canina", "Poodle", SexoPetEnum.F, 2.0, 5.0, tutor, clinica);

        _petRepositoryMock.Setup(r => r.GetById(petId)).Returns((Pet?)null);

        // Act
        var act = () => _petService.Update(petId, request);

        // Assert
        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("Pet não encontrado.");
        _petRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Delete_QuandoPetExiste_DeveRetornarTrue()
    {
        // Arrange
        const long petId = 1L;
        _petRepositoryMock.Setup(r => r.Delete(petId)).Returns(true);

        // Act
        var result = _petService.Delete(petId);

        // Assert
        result.Should().BeTrue();
        _petRepositoryMock.Verify(r => r.Delete(petId), Times.Once);
    }

    [Fact]
    public void Delete_QuandoPetNaoExiste_DeveRetornarFalse()
    {
        // Arrange
        const long petId = 999L;
        _petRepositoryMock.Setup(r => r.Delete(petId)).Returns(false);

        // Act
        var result = _petService.Delete(petId);

        // Assert
        result.Should().BeFalse();
        _petRepositoryMock.Verify(r => r.Delete(petId), Times.Once);
    }
}
