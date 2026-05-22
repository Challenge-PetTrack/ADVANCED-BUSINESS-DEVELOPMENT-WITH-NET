using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PetTrack.Application.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetTrack.Swagger;

/// <summary>
/// Define exemplos de payload para modelos exibidos no Swagger.
/// </summary>
public class SwaggerExampleSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Aplica exemplos para os schemas conhecidos da API.
    /// </summary>
    /// <param name="schema">Schema OpenAPI em construção.</param>
    /// <param name="context">Contexto do tipo que está sendo processado.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(TutorRequest))
        {
            SetPropertyDescription(schema, "nome", "Nome completo do tutor.");
            SetPropertyDescription(schema, "email", "E-mail único do tutor.");
            SetPropertyDescription(schema, "telefone", "Telefone de contato do tutor.");
            SetPropertyDescription(schema, "endereco", "Endereço completo do tutor.");

            schema.Example = new OpenApiObject
            {
                ["nome"] = new OpenApiString("Ana Paula Silva"),
                ["email"] = new OpenApiString("ana.paula@email.com"),
                ["telefone"] = new OpenApiString("11975647387"),
                ["endereco"] = new OpenApiString("Rua Jose, 89, São Paulo - SP")
            };
            return;
        }

        if (context.Type == typeof(ClinicaRequest))
        {
            SetPropertyDescription(schema, "nome", "Nome da clínica veterinária.");
            SetPropertyDescription(schema, "cnpj", "CNPJ da clínica.");
            SetPropertyDescription(schema, "email", "E-mail de contato da clínica.");
            SetPropertyDescription(schema, "telefone", "Telefone da clínica.");
            SetPropertyDescription(schema, "endereco", "Endereço da clínica.");

            schema.Example = new OpenApiObject
            {
                ["nome"] = new OpenApiString("Clyvo Vet"),
                ["cnpj"] = new OpenApiString("12.345.678/0001-99"),
                ["email"] = new OpenApiString("contato@clyvovet.com.br"),
                ["telefone"] = new OpenApiString("1133334444"),
                ["endereco"] = new OpenApiString("Av. Paulista, 1000, São Paulo - SP")
            };
            return;
        }

        if (context.Type == typeof(PetRequest))
        {
            SetPropertyDescription(schema, "nome", "Nome do pet.");
            SetPropertyDescription(schema, "especie", "Espécie do pet (ex: Cão, Gato).");
            SetPropertyDescription(schema, "raca", "Raça do pet.");
            SetPropertyDescription(schema, "sexo", "Sexo do pet: M ou F.");
            SetPropertyDescription(schema, "idade", "Idade do pet em anos.");
            SetPropertyDescription(schema, "peso", "Peso do pet em kg.");
            SetPropertyDescription(schema, "tutor", "Objeto do tutor responsável.");
            SetPropertyDescription(schema, "clinica", "Objeto da clínica vinculada.");

            schema.Example = new OpenApiObject
            {
                ["nome"] = new OpenApiString("Rex"),
                ["especie"] = new OpenApiString("Cão"),
                ["raca"] = new OpenApiString("Golden Retriever"),
                ["sexo"] = new OpenApiString("M"),
                ["idade"] = new OpenApiDouble(3),
                ["peso"] = new OpenApiDouble(28.5)
            };
            return;
        }

        if (context.Type == typeof(EventoClinicoRequest))
        {
            SetPropertyDescription(schema, "tipo", "Tipo do evento: CONSULTA, CIRURGIA, EXAME ou RETORNO.");
            SetPropertyDescription(schema, "dataEvento", "Data do evento clínico.");
            SetPropertyDescription(schema, "diagnostico", "Diagnóstico registrado.");
            SetPropertyDescription(schema, "observacao", "Observações adicionais.");

            schema.Example = new OpenApiObject
            {
                ["tipo"] = new OpenApiString("CONSULTA"),
                ["dataEvento"] = new OpenApiString("2026-05-19"),
                ["diagnostico"] = new OpenApiString("Pet saudável, sem alterações."),
                ["observacao"] = new OpenApiString("Retorno em 6 meses.")
            };
            return;
        }

        if (context.Type == typeof(MedicamentoRequest))
        {
            SetPropertyDescription(schema, "nome", "Nome do medicamento.");
            SetPropertyDescription(schema, "dosagem", "Dosagem prescrita.");
            SetPropertyDescription(schema, "frequencia", "Frequência de administração.");
            SetPropertyDescription(schema, "dataInicio", "Data de início do tratamento.");
            SetPropertyDescription(schema, "dataFim", "Data de término do tratamento.");

            schema.Example = new OpenApiObject
            {
                ["nome"] = new OpenApiString("Amoxicilina"),
                ["dosagem"] = new OpenApiString("250mg"),
                ["frequencia"] = new OpenApiString("A cada 8 horas"),
                ["dataInicio"] = new OpenApiString("2026-05-19"),
                ["dataFim"] = new OpenApiString("2026-05-26")
            };
            return;
        }

        if (context.Type == typeof(AdesaoMedicamentoRequest))
        {
            SetPropertyDescription(schema, "dataDose", "Data em que a dose foi administrada.");
            SetPropertyDescription(schema, "status", "Indica se tomou: S ou N.");
            SetPropertyDescription(schema, "observacao", "Observação sobre a adesão.");

            schema.Example = new OpenApiObject
            {
                ["dataDose"] = new OpenApiString("2026-05-19"),
                ["status"] = new OpenApiString("S"),
                ["observacao"] = new OpenApiString("Pet tomou o medicamento sem dificuldade.")
            };
            return;
        }

        if (context.Type == typeof(AlertaRequest))
        {
            SetPropertyDescription(schema, "tipo", "Tipo do alerta: FEBRE, PESO, BCS_CRITICO, ADESAO ou SEDENTARISMO.");
            SetPropertyDescription(schema, "descricao", "Descrição do alerta.");
            SetPropertyDescription(schema, "valor", "Valor de referência que gerou o alerta.");
            SetPropertyDescription(schema, "status", "Status do alerta: S (resolvido) ou N (pendente).");

            schema.Example = new OpenApiObject
            {
                ["tipo"] = new OpenApiString("FEBRE"),
                ["descricao"] = new OpenApiString("Temperatura acima do limite: 39.5°C"),
                ["valor"] = new OpenApiDouble(39.5),
                ["status"] = new OpenApiString("N")
            };
            return;
        }

        if (context.Type == typeof(CollarLeituraRequest))
        {
            SetPropertyDescription(schema, "temperatura", "Temperatura corporal lida pelo collar (°C).");
            SetPropertyDescription(schema, "atividade", "Nível de atividade física registrado.");
            SetPropertyDescription(schema, "topicoMqtt", "Tópico MQTT de origem da leitura.");

            schema.Example = new OpenApiObject
            {
                ["temperatura"] = new OpenApiDouble(38.2),
                ["atividade"] = new OpenApiDouble(72.5),
                ["topicoMqtt"] = new OpenApiString("pettrack/collar/pet-1/leitura")
            };
            return;
        }

        if (context.Type == typeof(ProtocoloPreventivoRequest))
        {
            SetPropertyDescription(schema, "tipo", "Tipo do protocolo: VACINA, VERMIFUGO, ANTIPULGA ou CHECKUP.");
            SetPropertyDescription(schema, "nome", "Nome do protocolo.");
            SetPropertyDescription(schema, "dateAplicacao", "Data da última aplicação.");
            SetPropertyDescription(schema, "dateProxima", "Data da próxima aplicação.");
            SetPropertyDescription(schema, "status", "Status: PENDENTE, REALIZADO ou ATRASADO.");

            schema.Example = new OpenApiObject
            {
                ["tipo"] = new OpenApiString("VACINA"),
                ["nome"] = new OpenApiString("V10 - Polivalente"),
                ["dateAplicacao"] = new OpenApiString("2025-11-19"),
                ["dateProxima"] = new OpenApiString("2026-11-19"),
                ["status"] = new OpenApiString("REALIZADO")
            };
            return;
        }

        if (context.Type == typeof(ScoreHistoricoRequest))
        {
            SetPropertyDescription(schema, "score", "Score de saúde do pet (0 a 100).");
            SetPropertyDescription(schema, "observacao", "Observação sobre o score.");

            schema.Example = new OpenApiObject
            {
                ["score"] = new OpenApiDouble(85.5),
                ["observacao"] = new OpenApiString("Pet em ótima condição de saúde.")
            };
            return;
        }

        if (context.Type == typeof(BcsHistoricoRequest))
        {
            SetPropertyDescription(schema, "bcs", "Body Condition Score (1 a 9).");
            SetPropertyDescription(schema, "fotoUrl", "URL da foto utilizada na análise.");
            SetPropertyDescription(schema, "observacao", "Observação sobre o BCS.");

            schema.Example = new OpenApiObject
            {
                ["bcs"] = new OpenApiInteger(5),
                ["fotoUrl"] = new OpenApiString("https://storage.pettrack.com/bcs/foto-rex.jpg"),
                ["observacao"] = new OpenApiString("Condição corporal ideal.")
            };
            return;
        }

        if (context.Type == typeof(NotificacaoRequest))
        {
            SetPropertyDescription(schema, "tipo", "Tipo: ALERTA, INFO, LEMBRETE ou URGENTE.");
            SetPropertyDescription(schema, "titulo", "Título da notificação.");
            SetPropertyDescription(schema, "mensagem", "Corpo da mensagem.");
            SetPropertyDescription(schema, "status", "Lida: S ou N.");

            schema.Example = new OpenApiObject
            {
                ["tipo"] = new OpenApiString("LEMBRETE"),
                ["titulo"] = new OpenApiString("Vacina V10 vencendo"),
                ["mensagem"] = new OpenApiString("A vacina V10 do Rex vence em 7 dias. Agende a consulta!"),
                ["status"] = new OpenApiString("N")
            };
        }
    }

    private static void SetPropertyDescription(OpenApiSchema schema, string propertyName, string description)
    {
        if (schema.Properties.TryGetValue(propertyName, out var property))
            property.Description = description;
    }
}