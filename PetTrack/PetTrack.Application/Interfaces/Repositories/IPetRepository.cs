using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IPetRepository
{
    /// <summary>Retorna todos os pets.</summary>
    IReadOnlyList<Pet> GetAll();

    /// <summary>Retorna um pet pelo seu identificador.</summary>
    Pet? GetById(long id);

    /// <summary>Retorna pets pelo id da clínica.</summary>
    IReadOnlyList<Pet> GetByClinicaId(long idClinica);

    /// <summary>Retorna pets pelo sexo.</summary>
    IReadOnlyList<Pet> GetBySexo(SexoPetEnum sexo);

    /// <summary>Retorna pets pelo nome ou espécie.</summary>
    IReadOnlyList<Pet> GetByNomeOuEspecie(string busca);

    /// <summary>Retorna pets com alertas pendentes.</summary>
    IReadOnlyList<Pet> GetPetsComAlertasPendentes();

    /// <summary>Adiciona um novo pet.</summary>
    void Add(Pet pet);

    /// <summary>Atualiza um pet existente.</summary>
    void Update(Pet pet);

    /// <summary>Remove um pet pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}