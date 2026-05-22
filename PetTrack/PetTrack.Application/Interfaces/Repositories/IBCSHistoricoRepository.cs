using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IBCSHistoricoRepository
{
    /// <summary>Retorna todos os registros de BCS.</summary>
    IReadOnlyList<BCSHistorico> GetAll();

    /// <summary>Retorna um registro de BCS pelo seu identificador.</summary>
    BCSHistorico? GetById(long id);

    /// <summary>Retorna histórico de BCS por pet.</summary>
    IReadOnlyList<BCSHistorico> GetHistoricoByPet(long idPet);

    /// <summary>Retorna a média de BCS por pet.</summary>
    double GetMediaBcsByPet(long idPet);

    /// <summary>Adiciona um novo registro de BCS.</summary>
    void Add(BCSHistorico bcs);

    /// <summary>Atualiza um registro de BCS existente.</summary>
    void Update(BCSHistorico bcs);

    /// <summary>Remove um registro de BCS pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}