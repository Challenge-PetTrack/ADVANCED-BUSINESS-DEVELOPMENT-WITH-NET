using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface IScoreHistoricoService
{
    ScoreHistoricoResponse Create(ScoreHistoricoRequest request);
    ScoreHistoricoResponse? GetById(long id);
    IReadOnlyList<ScoreHistoricoResponse> GetAll();
    IReadOnlyList<ScoreHistoricoResponse> GetHistoricoByPet(long idPet);
    double GetMediaScoreByPet(long idPet);
    ScoreHistoricoResponse Update(long id, ScoreHistoricoRequest request);
    bool Delete(long id);
}

public class ScoreHistoricoService : IScoreHistoricoService
{
    private readonly IScoreHistoricoRepository _scoreRepository;

    public ScoreHistoricoService(IScoreHistoricoRepository scoreRepository)
    {
        _scoreRepository = scoreRepository;
    }

    public ScoreHistoricoResponse Create(ScoreHistoricoRequest request)
    {
        var score = request.ToEntity();
        _scoreRepository.Add(score);
        return ScoreHistoricoResponse.ToDTO(score);
    }

    public ScoreHistoricoResponse? GetById(long id)
    {
        var score = _scoreRepository.GetById(id);
        return score is null ? null : ScoreHistoricoResponse.ToDTO(score);
    }

    public IReadOnlyList<ScoreHistoricoResponse> GetAll() =>
        _scoreRepository.GetAll().Select(ScoreHistoricoResponse.ToDTO).ToList();

    public IReadOnlyList<ScoreHistoricoResponse> GetHistoricoByPet(long idPet) =>
        _scoreRepository.GetHistoricoByPet(idPet).Select(ScoreHistoricoResponse.ToDTO).ToList();

    public double GetMediaScoreByPet(long idPet) =>
        _scoreRepository.GetMediaScoreByPet(idPet);

    public ScoreHistoricoResponse Update(long id, ScoreHistoricoRequest request)
    {
        var score = _scoreRepository.GetById(id)
            ?? throw new KeyNotFoundException("Score não encontrado.");

        score.Transferir(request.Score, request.Observacao, request.Pet);
        _scoreRepository.SaveChanges();
        return ScoreHistoricoResponse.ToDTO(score);
    }

    public bool Delete(long id) => _scoreRepository.Delete(id);
}
