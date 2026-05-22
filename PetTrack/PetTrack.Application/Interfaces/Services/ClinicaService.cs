using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface IClinicaService
{
    ClinicaResponse Create(ClinicaRequest request);
    ClinicaResponse? GetById(long id);
    IReadOnlyList<ClinicaResponse> GetAll();
    IReadOnlyList<ClinicaResponse> GetByNomeOuCnpj(string busca);
    IReadOnlyList<ClinicaResponse> GetByNomePet(string nomePet);
    ClinicaResponse Update(long id, ClinicaRequest request);
    bool Delete(long id);
}

public class ClinicaService : IClinicaService
{
    private readonly IClinicaRepository _clinicaRepository;

    public ClinicaService(IClinicaRepository clinicaRepository)
    {
        _clinicaRepository = clinicaRepository;
    }

    public ClinicaResponse Create(ClinicaRequest request)
    {
        var clinica = request.ToEntity();
        _clinicaRepository.Add(clinica);
        return ClinicaResponse.ToDTO(clinica);
    }

    public ClinicaResponse? GetById(long id)
    {
        var clinica = _clinicaRepository.GetById(id);
        return clinica is null ? null : ClinicaResponse.ToDTO(clinica);
    }

    public IReadOnlyList<ClinicaResponse> GetAll() =>
        _clinicaRepository.GetAll().Select(ClinicaResponse.ToDTO).ToList();

    public IReadOnlyList<ClinicaResponse> GetByNomeOuCnpj(string busca) =>
        _clinicaRepository.GetByNomeOuCnpj(busca).Select(ClinicaResponse.ToDTO).ToList();

    public IReadOnlyList<ClinicaResponse> GetByNomePet(string nomePet) =>
        _clinicaRepository.GetByNomePet(nomePet).Select(ClinicaResponse.ToDTO).ToList();

    public ClinicaResponse Update(long id, ClinicaRequest request)
    {
        var clinica = _clinicaRepository.GetById(id)
            ?? throw new KeyNotFoundException("Clínica não encontrada.");

        clinica.Transferir(request.Nome, request.Cnpj, request.Email, request.Telefone, request.Endereco);
        _clinicaRepository.SaveChanges();
        return ClinicaResponse.ToDTO(clinica);
    }

    public bool Delete(long id) => _clinicaRepository.Delete(id);
}
