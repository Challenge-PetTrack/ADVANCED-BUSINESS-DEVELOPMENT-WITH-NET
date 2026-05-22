using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IEventoClinicoRepository
{
    /// <summary>Retorna todos os eventos clínicos.</summary>
    IReadOnlyList<EventoClinico> GetAll();

    /// <summary>Retorna um evento clínico pelo seu identificador.</summary>
    EventoClinico? GetById(long id);

    /// <summary>Retorna eventos clínicos pelo tipo.</summary>
    IReadOnlyList<EventoClinico> GetByTipo(TipoEventoClinicoEnum tipo);

    /// <summary>Retorna eventos clínicos com medicamentos por pet.</summary>
    IReadOnlyList<EventoClinico> GetEventosComMedicamentos(long idPet);

    /// <summary>Adiciona um novo evento clínico.</summary>
    void Add(EventoClinico evento);

    /// <summary>Atualiza um evento clínico existente.</summary>
    void Update(EventoClinico evento);

    /// <summary>Remove um evento clínico pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}