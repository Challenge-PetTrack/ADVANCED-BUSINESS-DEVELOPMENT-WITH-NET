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
/// Testes unitários para AdesaoMedicamentoService utilizando Moq e AAA.
/// </summary>
public class AdesaoMedicamentoServiceTests
{
    private readonly Mock<IAdesaoMedicamentoRepository> _adesaoRepositoryMock;
    private readonly AdesaoMedicamentoService _adesaoService;

    public AdesaoMedicamentoServiceTests()
    {
        _adesaoRepositoryMock = new Mock<IAdesaoMedicamentoRepository>();
        _adesaoService = new AdesaoMedicamentoService(_adesaoRepositoryMock.Object);
    }

    [Fact]
    public void Create_ComDadosValidos_DeveChamarRepositoryERetornarResponse()
    {
        // Arrange
        var medicamento = PetTrackTestFixtures.CriarMedicamentoValido();
        var dataDose = DateTime.Now.AddHours(-2);
        var request = new AdesaoMedicamentoRequest(dataDose, SimNaoEnum.S, "Dose administrada", medicamento);

        // Act
        var result = _adesaoService.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(SimNaoEnum.S);
        result.Observacao.Should().Be("Dose administrada");
        _adesaoRepositoryMock.Verify(r => r.Add(It.IsAny<AdesaoMedicamento>()), Times.Once);
    }

    [Fact]
    public void GetById_QuandoExiste_DeveRetornarResponse()
    {
        // Arrange
        const long adesaoId = 1L;
        var adesao = PetTrackTestFixtures.CriarAdesaoValida(id: adesaoId);
        _adesaoRepositoryMock.Setup(r => r.GetById(adesaoId)).Returns(adesao);

        // Act
        var result = _adesaoService.GetById(adesaoId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(adesaoId);
        _adesaoRepositoryMock.Verify(r => r.GetById(adesaoId), Times.Once);
    }

    [Fact]
    public void GetById_QuandoNaoExiste_DeveRetornarNull()
    {
        // Arrange
        const long adesaoId = 999L;
        _adesaoRepositoryMock.Setup(r => r.GetById(adesaoId)).Returns((AdesaoMedicamento?)null);

        // Act
        var result = _adesaoService.GetById(adesaoId);

        // Assert
        result.Should().BeNull();
        _adesaoRepositoryMock.Verify(r => r.GetById(adesaoId), Times.Once);
    }

    [Fact]
    public void GetAll_QuandoChamado_DeveRetornarLista()
    {
        // Arrange
        var lista = new List<AdesaoMedicamento>
        {
            PetTrackTestFixtures.CriarAdesaoValida(id: 1),
            PetTrackTestFixtures.CriarAdesaoValida(id: 2)
        };
        _adesaoRepositoryMock.Setup(r => r.GetAll()).Returns(lista);

        // Act
        var result = _adesaoService.GetAll();

        // Assert
        result.Should().HaveCount(2);
        _adesaoRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void Update_QuandoExiste_DeveAtualizarERetornarResponse()
    {
        // Arrange
        const long adesaoId = 1L;
        var adesaoExistente = PetTrackTestFixtures.CriarAdesaoValida(id: adesaoId);
        var medicamento = PetTrackTestFixtures.CriarMedicamentoValido();
        var request = new AdesaoMedicamentoRequest(DateTime.Now.AddHours(-1), SimNaoEnum.N, "Pet recusou o medicamento", medicamento);
        _adesaoRepositoryMock.Setup(r => r.GetById(adesaoId)).Returns(adesaoExistente);

        // Act
        var result = _adesaoService.Update(adesaoId, request);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(SimNaoEnum.N);
        result.Observacao.Should().Be("Pet recusou o medicamento");
        _adesaoRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_QuandoNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        const long adesaoId = 999L;
        var medicamento = PetTrackTestFixtures.CriarMedicamentoValido();
        var request = new AdesaoMedicamentoRequest(DateTime.Now.AddHours(-1), SimNaoEnum.S, "Ok", medicamento);
        _adesaoRepositoryMock.Setup(r => r.GetById(adesaoId)).Returns((AdesaoMedicamento?)null);

        // Act
        var act = () => _adesaoService.Update(adesaoId, request);

        // Assert
        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("Adesão não encontrada.");
        _adesaoRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Delete_QuandoChamado_DeveRetornarResultadoDoRepository()
    {
        // Arrange
        const long adesaoId = 1L;
        _adesaoRepositoryMock.Setup(r => r.Delete(adesaoId)).Returns(true);

        // Act
        var result = _adesaoService.Delete(adesaoId);

        // Assert
        result.Should().BeTrue();
        _adesaoRepositoryMock.Verify(r => r.Delete(adesaoId), Times.Once);
    }
}
