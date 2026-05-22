using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Evento Clínico.
/// </summary>
/// <param name="Tipo">Tipo do evento: CIRURGIA, CONSULTA, EXAME ou RETORNO.</param>
/// <param name="DataEvento">Data em que ocorreu o evento.</param>
/// <param name="Diagnostico">Diagnóstico registrado. Opcional.</param>
/// <param name="Observacao">Observações clínicas. Opcional.</param>
/// <param name="Pet">Pet associado ao evento.</param>
/// <param name="Clinica">Clínica onde ocorreu o evento.</param>
public record EventoClinicoRequest(
    TipoEventoClinicoEnum Tipo,
    DateTime DataEvento,
    string? Diagnostico,
    string? Observacao,
    Pet Pet,
    Clinica Clinica)
{
    /// <summary>Converte o DTO para a entidade de domínio EventoClinico.</summary>
    public EventoClinico ToEntity() => new EventoClinico(Tipo, DataEvento, Diagnostico, Observacao, Pet, Clinica);
}