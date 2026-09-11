using FluentAssertions;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Domain;

/// <summary>
/// Testes unitários da entidade Alerta no padrão AAA.
/// </summary>
public class AlertaDomainTests
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveInstanciarAlertaComSucesso()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();
        const TipoAlertaEnum tipo = TipoAlertaEnum.FEBRE;
        const string descricao = "Alerta de temperatura alta";
        const double valor = 39.7;
        const SimNaoEnum status = SimNaoEnum.N;

        // Act
        var alerta = new Alerta(tipo, descricao, valor, status, pet);

        // Assert
        alerta.Tipo.Should().Be(TipoAlertaEnum.FEBRE);
        alerta.Descricao.Should().Be("Alerta de temperatura alta");
        alerta.Valor.Should().Be(39.7);
        alerta.Status.Should().Be(SimNaoEnum.N);
        alerta.Pet.Should().Be(pet);
    }

    [Fact]
    public void UpdateDescricao_ComMaisDe1000Caracteres_DeveLancarExcecao()
    {
        // Arrange
        var alerta = PetTrackTestFixtures.CriarAlertaValido();
        var descricaoLonga = new string('A', 1001);

        // Act
        var act = () => alerta.UpdateDescricao(descricaoLonga);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A descrição deve ter no máximo 1000 caracteres.");
    }

    [Fact]
    public void UpdatePet_ComPetNulo_DeveLancarExcecao()
    {
        // Arrange
        var alerta = PetTrackTestFixtures.CriarAlertaValido();

        // Act
        var act = () => alerta.UpdatePet(null!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O pet não pode ser nulo.");
    }

    [Fact]
    public void Transferir_ComNovosDados_DeveAtualizarAlerta()
    {
        // Arrange
        var alerta = PetTrackTestFixtures.CriarAlertaValido();
        var novoPet = PetTrackTestFixtures.CriarPetValido(id: 2, nome: "Mel");

        // Act
        alerta.Transferir(TipoAlertaEnum.SEDENTARISMO, "Baixo nível de atividade física", 150.0, SimNaoEnum.S, novoPet);

        // Assert
        alerta.Tipo.Should().Be(TipoAlertaEnum.SEDENTARISMO);
        alerta.Descricao.Should().Be("Baixo nível de atividade física");
        alerta.Valor.Should().Be(150.0);
        alerta.Status.Should().Be(SimNaoEnum.S);
        alerta.Pet.Should().Be(novoPet);
    }
}
