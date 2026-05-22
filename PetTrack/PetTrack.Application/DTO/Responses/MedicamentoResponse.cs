using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// Resposta com dados do Medicamento.
/// </summary>
/// <param name="Id">Identificador único do Medicamento gerado pelo Oracle.</param>
/// <param name="Nome">Nome do medicamento.</param>
/// <param name="Dosagem">Dosagem prescrita.</param>
/// <param name="Frequencia">Frequência de administração.</param>
/// <param name="DataInicio">Data de início do tratamento.</param>
/// <param name="DataFim">Data de término do tratamento. Opcional.</param>
/// <param name="Evento">Dados do evento clínico associado.</param>
public record MedicamentoResponse(
    long Id,
    string Nome,
    string Dosagem,
    string Frequencia,
    DateTime DataInicio,
    DateTime? DataFim,
    EventoClinicoResponse Evento)
{
    /// <summary>Converte a entidade Medicamento para o DTO de resposta.</summary>
    public static MedicamentoResponse ToDTO(Medicamento medicamento) => new(
        medicamento.Id,
        medicamento.Nome,
        medicamento.Dosagem,
        medicamento.Frequencia,
        medicamento.DataInicio,
        medicamento.DataFim,
        EventoClinicoResponse.ToDTO(medicamento.Evento));
}