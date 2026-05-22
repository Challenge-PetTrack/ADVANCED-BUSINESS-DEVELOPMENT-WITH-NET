using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IClinicaRepository
{
    /// <summary>Retorna todas as clínicas.</summary>
    IReadOnlyList<Clinica> GetAll();

    /// <summary>Retorna uma clínica pelo seu identificador.</summary>
    Clinica? GetById(long id);

    /// <summary>Retorna clínicas pelo nome ou CNPJ.</summary>
    IReadOnlyList<Clinica> GetByNomeOuCnpj(string busca);

    /// <summary>Retorna clínicas pelo nome de um pet cadastrado.</summary>
    IReadOnlyList<Clinica> GetByNomePet(string nomePet);

    /// <summary>Adiciona uma nova clínica.</summary>
    void Add(Clinica clinica);

    /// <summary>Atualiza uma clínica existente.</summary>
    void Update(Clinica clinica);

    /// <summary>Remove uma clínica pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}