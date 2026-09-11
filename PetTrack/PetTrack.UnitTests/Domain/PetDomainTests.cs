using FluentAssertions;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;
using PetTrack.UnitTests.Fixtures;
using Xunit;

namespace PetTrack.UnitTests.Domain;

/// <summary>
/// Testes unitários de regras de negócio e invariantes da entidade Pet utilizando o padrão AAA (Arrange, Act, Assert).
/// </summary>
public class PetDomainTests
{
    [Fact]
    public void Construtor_ComDadosValidos_DeveInstanciarPetComSucesso()
    {
        // Arrange
        var tutor = PetTrackTestFixtures.CriarTutorValido();
        var clinica = PetTrackTestFixtures.CriarClinicaValida();
        const string nome = "Thor";
        const string especie = "Canina";
        const string raca = "Golden Retriever";
        const SexoPetEnum sexo = SexoPetEnum.M;
        const double idade = 3.5;
        const double peso = 28.5;

        // Act
        var pet = new Pet(nome, especie, raca, sexo, idade, peso, tutor, clinica);

        // Assert
        pet.Nome.Should().Be("Thor");
        pet.Especie.Should().Be("Canina");
        pet.Raca.Should().Be("Golden Retriever");
        pet.Sexo.Should().Be(SexoPetEnum.M);
        pet.Idade.Should().Be(3.5);
        pet.Peso.Should().Be(28.5);
        pet.Tutor.Should().Be(tutor);
        pet.Clinica.Should().Be(clinica);
        pet.Eventos.Should().NotBeNull().And.BeEmpty();
        pet.Alertas.Should().NotBeNull().And.BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateNome_ComNomeInvalido_DeveLancarExcecao(string? nomeInvalido)
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdateNome(nomeInvalido!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O nome não pode ser vazio.");
    }

    [Fact]
    public void UpdateNome_ComMaisDe100Caracteres_DeveLancarExcecao()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();
        var nomeLongo = new string('A', 101);

        // Act
        var act = () => pet.UpdateNome(nomeLongo);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O nome deve ter no máximo 100 caracteres.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateEspecie_ComEspecieInvalida_DeveLancarExcecao(string? especieInvalida)
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdateEspecie(especieInvalida!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A espécie não pode ser vazia.");
    }

    [Fact]
    public void UpdateIdade_ComIdadeNegativa_DeveLancarExcecao()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdateIdade(-1.0);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A idade não pode ser negativa.");
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-5.0)]
    public void UpdatePeso_ComPesoInvalido_DeveLancarExcecao(double pesoInvalido)
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdatePeso(pesoInvalido);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O peso deve ser maior que zero.");
    }

    [Fact]
    public void UpdateTutor_ComTutorNulo_DeveLancarExcecao()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdateTutor(null!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("O tutor não pode ser nulo.");
    }

    [Fact]
    public void UpdateClinica_ComClinicaNula_DeveLancarExcecao()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();

        // Act
        var act = () => pet.UpdateClinica(null!);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage("A clínica não pode ser nula.");
    }

    [Fact]
    public void Transferir_ComDadosValidos_DeveAtualizarTodasPropriedades()
    {
        // Arrange
        var pet = PetTrackTestFixtures.CriarPetValido();
        var novoTutor = PetTrackTestFixtures.CriarTutorValido(id: 2, nome: "Gabriel Sbrana");
        var novaClinica = PetTrackTestFixtures.CriarClinicaValida(id: 2, nome: "Vet 24 Horas");

        // Act
        pet.Transferir("Luna", "Felina", "Siamês", SexoPetEnum.F, 2.0, 4.2, novoTutor, novaClinica);

        // Assert
        pet.Nome.Should().Be("Luna");
        pet.Especie.Should().Be("Felina");
        pet.Raca.Should().Be("Siamês");
        pet.Sexo.Should().Be(SexoPetEnum.F);
        pet.Idade.Should().Be(2.0);
        pet.Peso.Should().Be(4.2);
        pet.Tutor.Should().Be(novoTutor);
        pet.Clinica.Should().Be(novaClinica);
    }
}
