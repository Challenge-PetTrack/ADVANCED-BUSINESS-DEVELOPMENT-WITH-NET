using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IScoreHistoricoRepository
{
    /// <summary>Retorna todos os scores históricos.</summary>
    IReadOnlyList<ScoreHistorico> GetAll();

    /// <summary>Retorna um score histórico pelo seu identificador.</summary>
    ScoreHistorico? GetById(long id);

    /// <summary>Retorna histórico de scores por pet.</summary>
    IReadOnlyList<ScoreHistorico> GetHistoricoByPet(long idPet);

    /// <summary>Retorna a média de score por pet.</summary>
    double GetMediaScoreByPet(long idPet);

    /// <summary>Adiciona um novo score histórico.</summary>
    void Add(ScoreHistorico score);

    /// <summary>Atualiza um score histórico existente.</summary>
    void Update(ScoreHistorico score);

    /// <summary>Remove um score histórico pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}