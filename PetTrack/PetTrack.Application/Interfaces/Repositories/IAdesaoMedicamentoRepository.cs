using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IAdesaoMedicamentoRepository
{
    /// <summary>Retorna todas as adesões ao medicamento.</summary>
    IReadOnlyList<AdesaoMedicamento> GetAll();

    /// <summary>Retorna uma adesão pelo seu identificador.</summary>
    AdesaoMedicamento? GetById(long id);

    /// <summary>Retorna adesões pelo id do medicamento.</summary>
    IReadOnlyList<AdesaoMedicamento> GetByMedicamentoId(long idMedicamento);

    /// <summary>Retorna adesões pelo status (S ou N).</summary>
    IReadOnlyList<AdesaoMedicamento> GetByStatus(SimNaoEnum status);

    /// <summary>Adiciona uma nova adesão.</summary>
    void Add(AdesaoMedicamento adesao);

    /// <summary>Atualiza uma adesão existente.</summary>
    void Update(AdesaoMedicamento adesao);

    /// <summary>Remove uma adesão pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}