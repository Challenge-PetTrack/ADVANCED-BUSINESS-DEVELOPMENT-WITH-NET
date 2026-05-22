using PetTrack.Domain.Entities;

namespace PetTrack.Application.DTO.Requests;

public record ScoreHistoricoRequest(double Score, string? Observacao, Pet Pet);

