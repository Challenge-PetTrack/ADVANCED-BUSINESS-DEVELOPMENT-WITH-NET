using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IAlertaRepository
{
    /// <summary>Retorna todos os alertas.</summary>
    IReadOnlyList<Alerta> GetAll();

    /// <summary>Retorna um alerta pelo seu identificador.</summary>
    Alerta? GetById(long id);

    /// <summary>Retorna alertas pelo tipo.</summary>
    IReadOnlyList<Alerta> GetByTipo(TipoAlertaEnum tipo);

    /// <summary>Retorna alertas pendentes por pet.</summary>
    IReadOnlyList<Alerta> GetPendentesByPet(long idPet);

    /// <summary>Adiciona um novo alerta.</summary>
    void Add(Alerta alerta);

    /// <summary>Atualiza um alerta existente.</summary>
    void Update(Alerta alerta);

    /// <summary>Remove um alerta pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}