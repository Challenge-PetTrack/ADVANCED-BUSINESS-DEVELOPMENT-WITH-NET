using System.Reflection;
using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.UnitTests.Fixtures;

/// <summary>
/// Provedor centralizado de instâncias e dados fake para testes unitários com padrão AAA.
/// </summary>
public static class PetTrackTestFixtures
{
    public static Tutor CriarTutorValido(
        long id = 1,
        string nome = "Moisés Waidemann",
        string email = "moises@pettrack.com",
        string? telefone = "11999998888",
        string? endereco = "Av Paulista, 1000")
    {
        var tutor = new Tutor(nome, email, telefone, endereco);
        SetId(tutor, id);
        return tutor;
    }

    public static Clinica CriarClinicaValida(
        long id = 1,
        string nome = "Clyvo Vet Care",
        string cnpj = "12345678000199",
        string? email = "contato@clyvovet.com",
        string? telefone = "1133334444",
        string? endereco = "Rua Augusta, 500")
    {
        var clinica = new Clinica(nome, cnpj, email, telefone, endereco);
        SetId(clinica, id);
        return clinica;
    }

    public static Pet CriarPetValido(
        long id = 1,
        string nome = "Thor",
        string especie = "Canina",
        string? raca = "Golden Retriever",
        SexoPetEnum sexo = SexoPetEnum.M,
        double idade = 3.5,
        double peso = 28.5,
        Tutor? tutor = null,
        Clinica? clinica = null)
    {
        tutor ??= CriarTutorValido();
        clinica ??= CriarClinicaValida();

        var pet = new Pet(nome, especie, raca, sexo, idade, peso, tutor, clinica);
        SetId(pet, id);
        return pet;
    }

    public static EventoClinico CriarEventoClinicoValido(
        long id = 1,
        TipoEventoClinicoEnum tipo = TipoEventoClinicoEnum.CONSULTA,
        DateTime? dataEvento = null,
        string? diagnostico = "Consulta de rotina",
        string? observacao = "Pet em ótimo estado geral",
        Pet? pet = null,
        Clinica? clinica = null)
    {
        pet ??= CriarPetValido();
        clinica ??= CriarClinicaValida();
        var evento = new EventoClinico(
            tipo,
            dataEvento ?? DateTime.Now.AddDays(-1),
            diagnostico,
            observacao,
            pet,
            clinica);
        SetId(evento, id);
        return evento;
    }

    public static Medicamento CriarMedicamentoValido(
        long id = 1,
        string nome = "Apoquel 16mg",
        string dosagem = "1 comprimido",
        string frequencia = "A cada 24 horas",
        DateTime? dataInicio = null,
        DateTime? dataFim = null,
        EventoClinico? evento = null)
    {
        evento ??= CriarEventoClinicoValido();
        var inicio = dataInicio ?? DateTime.Now.AddDays(-5);
        var fim = dataFim ?? DateTime.Now.AddDays(5);
        var medicamento = new Medicamento(nome, dosagem, frequencia, inicio, fim, evento);
        SetId(medicamento, id);
        return medicamento;
    }

    public static Alerta CriarAlertaValido(
        long id = 1,
        TipoAlertaEnum tipo = TipoAlertaEnum.FEBRE,
        string? descricao = "Temperatura corporal elevada detectada via colar IoT Clyvo",
        double? valor = 39.8,
        SimNaoEnum status = SimNaoEnum.N,
        Pet? pet = null)
    {
        pet ??= CriarPetValido();
        var alerta = new Alerta(tipo, descricao, valor, status, pet);
        SetId(alerta, id);
        return alerta;
    }

    public static AdesaoMedicamento CriarAdesaoValida(
        long id = 1,
        DateTime? dataDose = null,
        SimNaoEnum status = SimNaoEnum.S,
        string? observacao = "Dose administrada pontualmente",
        Medicamento? medicamento = null)
    {
        medicamento ??= CriarMedicamentoValido();
        var adesao = new AdesaoMedicamento(dataDose ?? DateTime.Now.AddHours(-1), status, observacao, medicamento);
        SetId(adesao, id);
        return adesao;
    }

    public static CollarLeitura CriarCollarLeituraValida(
        long id = 1,
        double temperatura = 38.5,
        double? atividade = 450.0,
        string? topicoMqtt = "clyvo/collar/pet01/telemetria",
        Pet? pet = null)
    {
        pet ??= CriarPetValido();
        var leitura = new CollarLeitura(temperatura, atividade, topicoMqtt, pet);
        SetId(leitura, id);
        return leitura;
    }

    private static void SetId(object entity, long id)
    {
        var prop = entity.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        prop?.SetValue(entity, id);
    }
}
