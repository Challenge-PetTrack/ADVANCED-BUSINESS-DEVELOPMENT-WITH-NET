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
/// Testes unitários para AlertaService utilizando Moq e AAA.
/// </summary>
public class AlertaServiceTests
{
    private readonly Mock<IAlertaRepository> _alertaRepositoryMock;
    private readonly AlertaService _alertaService;

    public AlertaServiceTests()
    {
        _alertaRepositoryMock = new Mock<IAlertaRepository>();
        _alertaService = new AlertaService(_alertaRepositoryMock.Object);
    }

    [Fact]
    public void Create_ComDadosValidos_DeveChamarRepositoryERetornarAlertaResponse()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();
        var request = new AlertaRequest(TipoAlertaEnum.FEBRE, "Febre detectada", 39.5, SimNaoEnum.N, pet);

        // Act
        var result = _alertaService.Create(request);

        // Assert
        result.Should().NotBeNull();
        result.Tipo.Should().Be(TipoAlertaEnum.FEBRE);
        result.Descricao.Should().Be("Febre detectada");
        result.Valor.Should().Be(39.5);
        _alertaRepositoryMock.Verify(r => r.Add(It.IsAny<Alerta>()), Times.Once);
    }

    [Fact]
    public void GetById_QuandoAlertaExiste_DeveRetornarAlertaResponse()
    {
        // Arrange
        const long alertaId = 1L;
        var alerta = PetTrackTestFixtures.CriarAlertaValido(id: alertaId);
        _alertaRepositoryMock.Setup(r => r.GetById(alertaId)).Returns(alerta);

        // Act
        var result = _alertaService.GetById(alertaId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(alertaId);
        _alertaRepositoryMock.Verify(r => r.GetById(alertaId), Times.Once);
    }

    [Fact]
    public void GetById_QuandoAlertaNaoExiste_DeveRetornarNull()
    {
        // Arrange
        const long alertaId = 999L;
        _alertaRepositoryMock.Setup(r => r.GetById(alertaId)).Returns((Alerta?)null);

        // Act
        var result = _alertaService.GetById(alertaId);

        // Assert
        result.Should().BeNull();
        _alertaRepositoryMock.Verify(r => r.GetById(alertaId), Times.Once);
    }

    [Fact]
    public void GetAll_QuandoChamado_DeveRetornarListaDeAlertas()
    {
        // Arrange
        var alertas = new List<Alerta>
        {
            PetTrackTestFixtures.CriarAlertaValido(id: 1),
            PetTrackTestFixtures.CriarAlertaValido(id: 2, tipo: TipoAlertaEnum.SEDENTARISMO)
        };
        _alertaRepositoryMock.Setup(r => r.GetAll()).Returns(alertas);

        // Act
        var result = _alertaService.GetAll();

        // Assert
        result.Should().HaveCount(2);
        _alertaRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetByTipo_QuandoChamado_DeveRetornarAlertasDoTipo()
    {
        // Arrange
        const TipoAlertaEnum tipo = TipoAlertaEnum.FEBRE;
        var alertas = new List<Alerta> { PetTrackTestFixtures.CriarAlertaValido(tipo: tipo) };
        _alertaRepositoryMock.Setup(r => r.GetByTipo(tipo)).Returns(alertas);

        // Act
        var result = _alertaService.GetByTipo(tipo);

        // Assert
        result.Should().HaveCount(1);
        result.First().Tipo.Should().Be(TipoAlertaEnum.FEBRE);
        _alertaRepositoryMock.Verify(r => r.GetByTipo(tipo), Times.Once);
    }

    [Fact]
    public void GetPendentesByPet_QuandoChamado_DeveRetornarAlertasPendentes()
    {
        // Arrange
        const long petId = 1L;
        var alertas = new List<Alerta> { PetTrackTestFixtures.CriarAlertaValido(status: SimNaoEnum.N) };
        _alertaRepositoryMock.Setup(r => r.GetPendentesByPet(petId)).Returns(alertas);

        // Act
        var result = _alertaService.GetPendentesByPet(petId);

        // Assert
        result.Should().HaveCount(1);
        _alertaRepositoryMock.Verify(r => r.GetPendentesByPet(petId), Times.Once);
    }

    [Fact]
    public void Update_QuandoAlertaExiste_DeveAtualizarERetornarAlertaResponse()
    {
        // Arrange
        const long alertaId = 1L;
        var alertaExistente = PetTrackTestFixtures.CriarAlertaValido(id: alertaId);
        var pet = PetTrackTestFixtures.CriarPetValido();
        var request = new AlertaRequest(TipoAlertaEnum.ADESAO, "Adesão abaixo do esperado", 65.0, SimNaoEnum.S, pet);
        _alertaRepositoryMock.Setup(r => r.GetById(alertaId)).Returns(alertaExistente);

        // Act
        var result = _alertaService.Update(alertaId, request);

        // Assert
        result.Should().NotBeNull();
        result.Tipo.Should().Be(TipoAlertaEnum.ADESAO);
        result.Status.Should().Be(SimNaoEnum.S);
        _alertaRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Update_QuandoAlertaNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        const long alertaId = 999L;
        var pet = PetTrackTestFixtures.CriarPetValido();
        var request = new AlertaRequest(TipoAlertaEnum.BCS_CRITICO, "BCS crítico", 9.0, SimNaoEnum.N, pet);
        _alertaRepositoryMock.Setup(r => r.GetById(alertaId)).Returns((Alerta?)null);

        // Act
        var act = () => _alertaService.Update(alertaId, request);

        // Assert
        act.Should().Throw<KeyNotFoundException>()
            .WithMessage("Alerta não encontrado.");
        _alertaRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }

    [Fact]
    public void Delete_QuandoChamado_DeveRetornarResultadoDoRepository()
    {
        // Arrange
        const long alertaId = 1L;
        _alertaRepositoryMock.Setup(r => r.Delete(alertaId)).Returns(true);

        // Act
        var result = _alertaService.Delete(alertaId);

        // Assert
        result.Should().BeTrue();
        _alertaRepositoryMock.Verify(r => r.Delete(alertaId), Times.Once);
    }
}
