using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface IMedicamentoService
{
    MedicamentoResponse Create(MedicamentoRequest request);
    MedicamentoResponse? GetById(long id);
    IReadOnlyList<MedicamentoResponse> GetAll();
    IReadOnlyList<MedicamentoResponse> GetByNome(string nome);
    IReadOnlyList<MedicamentoResponse> GetMedicamentosAtivosByPet(long idPet);
    MedicamentoResponse Update(long id, MedicamentoRequest request);
    bool Delete(long id);
}

public class MedicamentoService : IMedicamentoService
{
    private readonly IMedicamentoRepository _medicamentoRepository;

    public MedicamentoService(IMedicamentoRepository medicamentoRepository)
    {
        _medicamentoRepository = medicamentoRepository;
    }

    public MedicamentoResponse Create(MedicamentoRequest request)
    {
        var medicamento = request.ToEntity();
        _medicamentoRepository.Add(medicamento);
        return MedicamentoResponse.ToDTO(medicamento);
    }

    public MedicamentoResponse? GetById(long id)
    {
        var medicamento = _medicamentoRepository.GetById(id);
        return medicamento is null ? null : MedicamentoResponse.ToDTO(medicamento);
    }

    public IReadOnlyList<MedicamentoResponse> GetAll() =>
        _medicamentoRepository.GetAll().Select(MedicamentoResponse.ToDTO).ToList();

    public IReadOnlyList<MedicamentoResponse> GetByNome(string nome) =>
        _medicamentoRepository.GetByNome(nome).Select(MedicamentoResponse.ToDTO).ToList();

    public IReadOnlyList<MedicamentoResponse> GetMedicamentosAtivosByPet(long idPet) =>
        _medicamentoRepository.GetMedicamentosAtivosByPet(idPet).Select(MedicamentoResponse.ToDTO).ToList();

    public MedicamentoResponse Update(long id, MedicamentoRequest request)
    {
        var medicamento = _medicamentoRepository.GetById(id)
            ?? throw new KeyNotFoundException("Medicamento não encontrado.");

        medicamento.Transferir(request.Nome, request.Dosagem, request.Frequencia, request.DataInicio, request.DataFim, request.EventoClinico);
        _medicamentoRepository.SaveChanges();
        return MedicamentoResponse.ToDTO(medicamento);
    }

    public bool Delete(long id) => _medicamentoRepository.Delete(id);
}
