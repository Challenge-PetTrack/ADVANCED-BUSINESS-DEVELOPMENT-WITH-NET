using PetTrack.Domain.Entities;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IMedicamentoRepository
{
    /// <summary>Retorna todos os medicamentos.</summary>
    IReadOnlyList<Medicamento> GetAll();

    /// <summary>Retorna um medicamento pelo seu identificador.</summary>
    Medicamento? GetById(long id);

    /// <summary>Retorna medicamentos pelo nome (busca parcial).</summary>
    IReadOnlyList<Medicamento> GetByNome(string nome);

    /// <summary>Retorna medicamentos ativos por pet.</summary>
    IReadOnlyList<Medicamento> GetMedicamentosAtivosByPet(long idPet);

    /// <summary>Adiciona um novo medicamento.</summary>
    void Add(Medicamento medicamento);

    /// <summary>Atualiza um medicamento existente.</summary>
    void Update(Medicamento medicamento);

    /// <summary>Remove um medicamento pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}