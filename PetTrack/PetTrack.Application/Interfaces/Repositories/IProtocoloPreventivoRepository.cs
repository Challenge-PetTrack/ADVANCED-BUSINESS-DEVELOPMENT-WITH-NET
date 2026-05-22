using PetTrack.Domain.Entities;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Repositories;

public interface IProtocoloPreventivoRepository
{
    /// <summary>Retorna todos os protocolos preventivos.</summary>
    IReadOnlyList<ProtocoloPreventivo> GetAll();

    /// <summary>Retorna um protocolo preventivo pelo seu identificador.</summary>
    ProtocoloPreventivo? GetById(long id);

    /// <summary>Retorna protocolos preventivos pelo tipo.</summary>
    IReadOnlyList<ProtocoloPreventivo> GetByTipo(TipoProtocoloPreventivoEnum tipo);

    /// <summary>Retorna protocolos pendentes ou atrasados por pet.</summary>
    IReadOnlyList<ProtocoloPreventivo> GetPendentesOuAtrasadosByPet(long idPet);

    /// <summary>Adiciona um novo protocolo preventivo.</summary>
    void Add(ProtocoloPreventivo protocolo);

    /// <summary>Atualiza um protocolo preventivo existente.</summary>
    void Update(ProtocoloPreventivo protocolo);

    /// <summary>Remove um protocolo preventivo pelo identificador.</summary>
    bool Delete(long id);

    /// <summary>Persiste as alterações no banco de dados.</summary>
    void SaveChanges();
}