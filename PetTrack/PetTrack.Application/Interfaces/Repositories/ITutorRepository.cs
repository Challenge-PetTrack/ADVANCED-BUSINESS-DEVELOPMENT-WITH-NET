using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface ITutorRepository
{
    /// <summary>Retorna todos os tutores.</summary>
    IReadOnlyList<Tutor> GetAll();

    /// <summary>Retorna um tutor pelo seu identificador.</summary>
    Tutor? GetById(long id);

    /// <summary>Retorna tutores pelo nome ou email.</summary>
    IReadOnlyList<Tutor> GetByNomeOuEmail(string busca);

    /// <summary>Retorna tutores pelo nome de um pet cadastrado.</summary>
    IReadOnlyList<Tutor> GetByNomePet(string nomePet);

    /// <summary>Adiciona um novo tutor.</summary>
    void Add(Tutor tutor);

    /// <summary>Atualiza um tutor existente.</summary>
    void Update(Tutor tutor);

    /// <summary>Remove um tutor pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}