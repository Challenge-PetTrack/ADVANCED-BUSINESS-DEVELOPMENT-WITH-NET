using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface ICollarLeituraService
{
    CollarLeituraResponse Create(CollarLeituraRequest request);
    CollarLeituraResponse? GetById(long id);
    IReadOnlyList<CollarLeituraResponse> GetAll();
    IReadOnlyList<CollarLeituraResponse> GetByPetETemperaturaAcimaDe(long idPet, double temperatura);
    CollarLeituraResponse? GetUltimaLeituraByPet(long idPet);
    CollarLeituraResponse Update(long id, CollarLeituraRequest request);
    bool Delete(long id);
}

public class CollarLeituraService : ICollarLeituraService
{
    private readonly ICollarLeituraRepository _collarRepository;

    public CollarLeituraService(ICollarLeituraRepository collarRepository)
    {
        _collarRepository = collarRepository;
    }

    public CollarLeituraResponse Create(CollarLeituraRequest request)
    {
        var leitura = request.ToEntity();
        _collarRepository.Add(leitura);
        return CollarLeituraResponse.ToDTO(leitura);
    }

    public CollarLeituraResponse? GetById(long id)
    {
        var leitura = _collarRepository.GetById(id);
        return leitura is null ? null : CollarLeituraResponse.ToDTO(leitura);
    }

    public IReadOnlyList<CollarLeituraResponse> GetAll() =>
        _collarRepository.GetAll().Select(CollarLeituraResponse.ToDTO).ToList();

    public IReadOnlyList<CollarLeituraResponse> GetByPetETemperaturaAcimaDe(long idPet, double temperatura) =>
        _collarRepository.GetByPetETemperaturaAcimaDe(idPet, temperatura).Select(CollarLeituraResponse.ToDTO).ToList();

    public CollarLeituraResponse? GetUltimaLeituraByPet(long idPet)
    {
        var leitura = _collarRepository.GetUltimaLeituraByPet(idPet);
        return leitura is null ? null : CollarLeituraResponse.ToDTO(leitura);
    }

    public CollarLeituraResponse Update(long id, CollarLeituraRequest request)
    {
        var leitura = _collarRepository.GetById(id)
            ?? throw new KeyNotFoundException("Leitura não encontrada.");

        leitura.Transferir(request.Temperatura, request.Atividade, request.TopicoMqtt);
        _collarRepository.SaveChanges();
        return CollarLeituraResponse.ToDTO(leitura);
    }

    public bool Delete(long id) => _collarRepository.Delete(id);
}
