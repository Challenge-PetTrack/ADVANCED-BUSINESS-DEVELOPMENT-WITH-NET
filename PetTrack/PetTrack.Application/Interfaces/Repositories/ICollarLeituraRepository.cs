using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface ICollarLeituraRepository
{
    /// <summary>Retorna todas as leituras do collar.</summary>
    IReadOnlyList<CollarLeitura> GetAll();

    /// <summary>Retorna uma leitura pelo seu identificador.</summary>
    CollarLeitura? GetById(long id);

    /// <summary>Retorna leituras por pet com temperatura acima de um valor.</summary>
    IReadOnlyList<CollarLeitura> GetByPetETemperaturaAcimaDe(long idPet, double temperatura);

    /// <summary>Retorna a última leitura registrada por pet.</summary>
    CollarLeitura? GetUltimaLeituraByPet(long idPet);

    /// <summary>Adiciona uma nova leitura.</summary>
    void Add(CollarLeitura leitura);

    /// <summary>Atualiza uma leitura existente.</summary>
    void Update(CollarLeitura leitura);

    /// <summary>Remove uma leitura pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}