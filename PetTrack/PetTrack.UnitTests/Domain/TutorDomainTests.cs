using FluentAssertions;
using PetTrack.Domain.Entities;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Domain;

/// <summary>
/// Testes unitários da entidade Tutor no padrão AAA.
/// </summary>
public class TutorDomainTests
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveInstanciarTutorComSucesso()
    {
        // Arrange
        const string nome = "Richard Freitas";
        const string email = "richard@pettrack.com";
        const string telefone = "11988887777";
        const string endereco = "Rua Vergueiro, 1200";

        // Act
        var tutor = new Tutor(nome, email, telefone, endereco);

        // Assert
        tutor.Nome.Should().Be("Richard Freitas");
        tutor.Email.Should().Be("richard@pettrack.com");
        tutor.Telefone.Should().Be("11988887777");
        tutor.Endereco.Should().Be("Rua Vergueiro, 1200");
        tutor.Pets.Should().NotBeNull().And.BeEmpty();
        tutor.Notificacoes.Should().NotBeNull().And.BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateNome_ComNomeInvalido_DeveLancarExcecao(string? nomeInvalido)
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();

        // Act
        var act = () => tutor.UpdateNome(nomeInvalido!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateEmail_ComEmailVazio_DeveLancarExcecao(string? emailVazio)
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();

        // Act
        var act = () => tutor.UpdateEmail(emailVazio!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O email não pode ser vazio.");
    }

    [Theory]
    [InlineData("emailinvalido")]
    [InlineData("usuario.dominio.com")]
    public void UpdateEmail_SemArroba_DeveLancarExcecao(string emailInvalido)
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();

        // Act
        var act = () => tutor.UpdateEmail(emailInvalido);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("Informe um email válido.");
    }

    [Fact]
    public void UpdateTelefone_ComMaisDe20Caracteres_DeveLancarExcecao()
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();
        var telefoneLongo = new string('9', 21);

        // Act
        var act = () => tutor.UpdateTelefone(telefoneLongo);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("Telefone deve ter no máximo 20 caracteres.");
    }

    [Fact]
    public void UpdateEndereco_ComMaisDe300Caracteres_DeveLancarExcecao()
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();
        var enderecoLongo = new string('A', 301);

        // Act
        var act = () => tutor.UpdateEndereco(enderecoLongo);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("Endereço deve ter no máximo 300 caracteres.");
    }

    [Fact]
    public void Transferir_ComDadosValidos_DeveAtualizarTutor()
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();

        // Act
        tutor.Transferir("Thiago Rodrigues da Mota", "thiago@pettrack.com", "11977776666", "Av Brasil, 1500");

        // Assert
        tutor.Nome.Should().Be("Thiago Rodrigues da Mota");
        tutor.Email.Should().Be("thiago@pettrack.com");
        tutor.Telefone.Should().Be("11977776666");
        tutor.Endereco.Should().Be("Av Brasil, 1500");
    }
}
