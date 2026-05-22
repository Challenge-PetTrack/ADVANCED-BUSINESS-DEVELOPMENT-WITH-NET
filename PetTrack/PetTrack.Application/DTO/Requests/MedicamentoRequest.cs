using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

/// <summary>
/// Corpo para criar ou atualizar um Medicamento.
/// </summary>
/// <param name="Nome">Nome do medicamento.</param>
/// <param name="Dosagem">Dosagem prescrita.</param>
/// <param name="Frequencia">Frequência de administração.</param>
/// <param name="DataInicio">Data de início do tratamento.</param>
/// <param name="DataFim">Data de término do tratamento. Opcional.</param>
/// <param name="Evento">Evento clínico associado.</param>
public record MedicamentoRequest(
    string Nome,
    string Dosagem,
    string Frequencia,
    DateTime DataInicio,
    DateTime? DataFim,
    EventoClinico EventoClinico)
{
    /// <summary>Converte o DTO para a entidade de domínio Medicamento.</summary>
    public Medicamento ToEntity() => new Medicamento(Nome, Dosagem, Frequencia, DataInicio,  DataFim, EventoClinico);
}