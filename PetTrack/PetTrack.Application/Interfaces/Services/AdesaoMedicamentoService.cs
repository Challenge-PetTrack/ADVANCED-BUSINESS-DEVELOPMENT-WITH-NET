using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface IAdesaoMedicamentoService
{
    AdesaoMedicamentoResponse Create(AdesaoMedicamentoRequest request);
    AdesaoMedicamentoResponse? GetById(long id);
    IReadOnlyList<AdesaoMedicamentoResponse> GetAll();
    IReadOnlyList<AdesaoMedicamentoResponse> GetByMedicamentoId(long idMedicamento);
    IReadOnlyList<AdesaoMedicamentoResponse> GetByStatus(SimNaoEnum status);
    AdesaoMedicamentoResponse Update(long id, AdesaoMedicamentoRequest request);
    bool Delete(long id);
}

public class AdesaoMedicamentoService : IAdesaoMedicamentoService
{
    private readonly IAdesaoMedicamentoRepository _adesaoRepository;

    public AdesaoMedicamentoService(IAdesaoMedicamentoRepository adesaoRepository)
    {
        _adesaoRepository = adesaoRepository;
    }

    public AdesaoMedicamentoResponse Create(AdesaoMedicamentoRequest request)
    {
        var adesao = request.ToEntity();
        _adesaoRepository.Add(adesao);
        return AdesaoMedicamentoResponse.ToDTO(adesao);
    }

    public AdesaoMedicamentoResponse? GetById(long id)
    {
        var adesao = _adesaoRepository.GetById(id);
        return adesao is null ? null : AdesaoMedicamentoResponse.ToDTO(adesao);
    }

    public IReadOnlyList<AdesaoMedicamentoResponse> GetAll() =>
        _adesaoRepository.GetAll().Select(AdesaoMedicamentoResponse.ToDTO).ToList();

    public IReadOnlyList<AdesaoMedicamentoResponse> GetByMedicamentoId(long idMedicamento) =>
        _adesaoRepository.GetByMedicamentoId(idMedicamento).Select(AdesaoMedicamentoResponse.ToDTO).ToList();

    public IReadOnlyList<AdesaoMedicamentoResponse> GetByStatus(SimNaoEnum status) =>
        _adesaoRepository.GetByStatus(status).Select(AdesaoMedicamentoResponse.ToDTO).ToList();

    public AdesaoMedicamentoResponse Update(long id, AdesaoMedicamentoRequest request)
    {
        var adesao = _adesaoRepository.GetById(id)
            ?? throw new KeyNotFoundException("Adesão não encontrada.");

        adesao.Transferir(request.DataDose, request.Status, request.Observacao, request.Medicamento);
        _adesaoRepository.SaveChanges();
        return AdesaoMedicamentoResponse.ToDTO(adesao);
    }

    public bool Delete(long id) => _adesaoRepository.Delete(id);
}
