using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface IBcsHistoricoService
{
    BcsHistoricoResponse Create(BCSHistoricoRequest request);
    BcsHistoricoResponse? GetById(long id);
    IReadOnlyList<BcsHistoricoResponse> GetAll();
    IReadOnlyList<BcsHistoricoResponse> GetHistoricoByPet(long idPet);
    double GetMediaBcsByPet(long idPet);
    BcsHistoricoResponse Update(long id, BCSHistoricoRequest request);
    bool Delete(long id);
}

public class BcsHistoricoService : IBcsHistoricoService
{
    private readonly IBCSHistoricoRepository _bcsRepository;

    public BcsHistoricoService(IBCSHistoricoRepository bcsRepository)
    {
        _bcsRepository = bcsRepository;
    }

    public BcsHistoricoResponse Create(BCSHistoricoRequest request)
    {
        var bcs = request.ToEntity();
        _bcsRepository.Add(bcs);
        return BcsHistoricoResponse.ToDTO(bcs);
    }

    public BcsHistoricoResponse? GetById(long id)
    {
        var bcs = _bcsRepository.GetById(id);
        return bcs is null ? null : BcsHistoricoResponse.ToDTO(bcs);
    }

    public IReadOnlyList<BcsHistoricoResponse> GetAll() =>
        _bcsRepository.GetAll().Select(BcsHistoricoResponse.ToDTO).ToList();

    public IReadOnlyList<BcsHistoricoResponse> GetHistoricoByPet(long idPet) =>
        _bcsRepository.GetHistoricoByPet(idPet).Select(BcsHistoricoResponse.ToDTO).ToList();

    public double GetMediaBcsByPet(long idPet) =>
        _bcsRepository.GetMediaBcsByPet(idPet);

    public BcsHistoricoResponse Update(long id, BCSHistoricoRequest request)
    {
        var bcs = _bcsRepository.GetById(id)
            ?? throw new KeyNotFoundException("BCS não encontrado.");

        bcs.Transferir(request.Bcs, request.FotoUrl, request.Observacao);
        _bcsRepository.SaveChanges();
        return BcsHistoricoResponse.ToDTO(bcs);
    }

    public bool Delete(long id) => _bcsRepository.Delete(id);
}
