using FluentAssertions;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Domain;

/// <summary>
/// Testes unitários da entidade AdesaoMedicamento no padrão AAA.
/// </summary>
public class AdesaoMedicamentoDomainTests
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveInstanciarAdesaoComSucesso()
    {
        // Arrange
        var medicamento = PetTrackTestFixtures.CriarMedicamentoValido();
        var dataDose = DateTime.Now.AddMinutes(-30);
        const SimNaoEnum status = SimNaoEnum.S;
        const string observacao = "Administrado conforme prescrição";

        // Act
        var adesao = new AdesaoMedicamento(dataDose, status, observacao, medicamento);

        // Assert
        adesao.DataDose.Should().Be(dataDose);
        adesao.Status.Should().Be(SimNaoEnum.S);
        adesao.Observacao.Should().Be("Administrado conforme prescrição");
        adesao.Medicamento.Should().Be(medicamento);
    }

    [Fact]
    public void UpdateDataDose_ComDataFutura_DeveLancarExcecao()
    {
        // Arrange
        var adesao = PetTrackTestFixtures.CriarAdesaoValida();
        var dataFutura = DateTime.Now.AddDays(2);

        // Act
        var act = () => adesao.UpdateDataDose(dataFutura);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A data da dose não pode ser no futuro.");
    }

    [Fact]
    public void UpdateObservacao_ComMaisDe500Caracteres_DeveLancarExcecao()
    {
        // Arrange
        var adesao = PetTrackTestFixtures.CriarAdesaoValida();
        var obsLonga = new string('A', 501);

        // Act
        var act = () => adesao.UpdateObservacao(obsLonga);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A observação deve ter no máximo 500 caracteres.");
    }

    [Fact]
    public void UpdateMedicamento_ComMedicamentoNulo_DeveLancarExcecao()
    {
        // Arrange
        var adesao = PetTrackTestFixtures.CriarAdesaoValida();

        // Act
        var act = () => adesao.UpdateMedicamento(null!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O medicamento não pode ser nulo.");
    }
}
