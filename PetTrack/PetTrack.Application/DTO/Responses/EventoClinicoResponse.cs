using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Evento Clínico.
/// </summary>
/// <param name="Id">Identificador único do Evento gerado pelo Oracle.</param>
/// <param name="Tipo">Tipo do evento: CIRURGIA, CONSULTA, EXAME ou RETORNO.</param>
/// <param name="DataEvento">Data em que ocorreu o evento.</param>
/// <param name="Diagnostico">Diagnóstico registrado. Opcional.</param>
/// <param name="Observacao">Observações clínicas. Opcional.</param>
/// <param name="Pet">Dados do pet associado.</param>
/// <param name="Clinica">Dados da clínica onde ocorreu o evento.</param>
public record EventoClinicoResponse(
    long Id,
    TipoEventoClinicoEnum Tipo,
    DateTime DataEvento,
    string? Diagnostico,
    string? Observacao,
    PetResponse Pet,
    ClinicaResponse Clinica)
{
    /// <summary>Converte a entidade EventoClinico para o DTO de resposta.</summary>
    public static EventoClinicoResponse ToDTO(EventoClinico evento) => new(
        evento.Id,
        evento.Tipo,
        evento.DataEvento,
        evento.Diagnostico,
        evento.Observacao,
        PetResponse.ToDTO(evento.Pet),
        ClinicaResponse.ToDTO(evento.Clinica));
}