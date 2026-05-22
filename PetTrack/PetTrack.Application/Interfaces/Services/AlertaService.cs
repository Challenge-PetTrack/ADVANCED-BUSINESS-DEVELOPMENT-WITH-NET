using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface IAlertaService
{
    AlertaResponse Create(AlertaRequest request);
    AlertaResponse? GetById(long id);
    IReadOnlyList<AlertaResponse> GetAll();
    IReadOnlyList<AlertaResponse> GetByTipo(TipoAlertaEnum tipo);
    IReadOnlyList<AlertaResponse> GetPendentesByPet(long idPet);
    AlertaResponse Update(long id, AlertaRequest request);
    bool Delete(long id);
}

public class AlertaService : IAlertaService
{
    private readonly IAlertaRepository _alertaRepository;

    public AlertaService(IAlertaRepository alertaRepository)
    {
        _alertaRepository = alertaRepository;
    }

    public AlertaResponse Create(AlertaRequest request)
    {
        var alerta = request.ToEntity();
        _alertaRepository.Add(alerta);
        return AlertaResponse.ToDTO(alerta);
    }

    public AlertaResponse? GetById(long id)
    {
        var alerta = _alertaRepository.GetById(id);
        return alerta is null ? null : AlertaResponse.ToDTO(alerta);
    }

    public IReadOnlyList<AlertaResponse> GetAll() =>
        _alertaRepository.GetAll().Select(AlertaResponse.ToDTO).ToList();

    public IReadOnlyList<AlertaResponse> GetByTipo(TipoAlertaEnum tipo) =>
        _alertaRepository.GetByTipo(tipo).Select(AlertaResponse.ToDTO).ToList();

    public IReadOnlyList<AlertaResponse> GetPendentesByPet(long idPet) =>
        _alertaRepository.GetPendentesByPet(idPet).Select(AlertaResponse.ToDTO).ToList();

    public AlertaResponse Update(long id, AlertaRequest request)
    {
        var alerta = _alertaRepository.GetById(id)
            ?? throw new KeyNotFoundException("Alerta não encontrado.");

        alerta.Transferir(request.Tipo, request.Descricao, request.Valor, request.Status, request.Pet);
        _alertaRepository.SaveChanges();
        return AlertaResponse.ToDTO(alerta);
    }

    public bool Delete(long id) => _alertaRepository.Delete(id);
}
