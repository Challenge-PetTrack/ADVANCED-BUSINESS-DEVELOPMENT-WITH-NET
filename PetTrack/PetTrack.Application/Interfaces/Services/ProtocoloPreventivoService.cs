using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface IProtocoloPreventivoService
{
    ProtocoloPreventivoResponse Create(ProtocoloPreventivoRequest request);
    ProtocoloPreventivoResponse? GetById(long id);
    IReadOnlyList<ProtocoloPreventivoResponse> GetAll();
    IReadOnlyList<ProtocoloPreventivoResponse> GetByTipo(TipoProtocoloPreventivoEnum tipo);
    IReadOnlyList<ProtocoloPreventivoResponse> GetPendentesOuAtrasadosByPet(long idPet);
    ProtocoloPreventivoResponse Update(long id, ProtocoloPreventivoRequest request);
    bool Delete(long id);
}

public class ProtocoloPreventivoService : IProtocoloPreventivoService
{
    private readonly IProtocoloPreventivoRepository _protocoloRepository;

    public ProtocoloPreventivoService(IProtocoloPreventivoRepository protocoloRepository)
    {
        _protocoloRepository = protocoloRepository;
    }

    public ProtocoloPreventivoResponse Create(ProtocoloPreventivoRequest request)
    {
        var protocolo = request.ToEntity();
        _protocoloRepository.Add(protocolo);
        return ProtocoloPreventivoResponse.ToDTO(protocolo);
    }

    public ProtocoloPreventivoResponse? GetById(long id)
    {
        var protocolo = _protocoloRepository.GetById(id);
        return protocolo is null ? null : ProtocoloPreventivoResponse.ToDTO(protocolo);
    }

    public IReadOnlyList<ProtocoloPreventivoResponse> GetAll() =>
        _protocoloRepository.GetAll().Select(ProtocoloPreventivoResponse.ToDTO).ToList();

    public IReadOnlyList<ProtocoloPreventivoResponse> GetByTipo(TipoProtocoloPreventivoEnum tipo) =>
        _protocoloRepository.GetByTipo(tipo).Select(ProtocoloPreventivoResponse.ToDTO).ToList();

    public IReadOnlyList<ProtocoloPreventivoResponse> GetPendentesOuAtrasadosByPet(long idPet) =>
        _protocoloRepository.GetPendentesOuAtrasadosByPet(idPet).Select(ProtocoloPreventivoResponse.ToDTO).ToList();

    public ProtocoloPreventivoResponse Update(long id, ProtocoloPreventivoRequest request)
    {
        var protocolo = _protocoloRepository.GetById(id)
            ?? throw new KeyNotFoundException("Protocolo não encontrado.");

        protocolo.Transferir(request.Tipo, request.Nome, request.DateAplicacao, request.DateProxima, request.Status, request.Pet);
        _protocoloRepository.SaveChanges();
        return ProtocoloPreventivoResponse.ToDTO(protocolo);
    }

    public bool Delete(long id) => _protocoloRepository.Delete(id);
}
