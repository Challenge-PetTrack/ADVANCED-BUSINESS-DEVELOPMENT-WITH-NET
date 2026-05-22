using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Domain.Enum;

namespace PetTrack.Application.Interfaces.Services;

public interface IPetService
{
    PetResponse Create(PetRequest request);
    PetResponse? GetById(long id);
    IReadOnlyList<PetResponse> GetAll();
    IReadOnlyList<PetResponse> GetByClinicaId(long idClinica);
    IReadOnlyList<PetResponse> GetBySexo(SexoPetEnum sexo);
    IReadOnlyList<PetResponse> GetByNomeOuEspecie(string busca);
    IReadOnlyList<PetResponse> GetPetsComAlertasPendentes();
    PetResponse Update(long id, PetRequest request);
    bool Delete(long id);
}

public class PetService : IPetService
{
    private readonly IPetRepository _petRepository;

    public PetService(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public PetResponse Create(PetRequest request)
    {
        var pet = request.ToEntity();
        _petRepository.Add(pet);
        return PetResponse.ToDTO(pet);
    }

    public PetResponse? GetById(long id)
    {
        var pet = _petRepository.GetById(id);
        return pet is null ? null : PetResponse.ToDTO(pet);
    }

    public IReadOnlyList<PetResponse> GetAll() =>
        _petRepository.GetAll().Select(PetResponse.ToDTO).ToList();

    public IReadOnlyList<PetResponse> GetByClinicaId(long idClinica) =>
        _petRepository.GetByClinicaId(idClinica).Select(PetResponse.ToDTO).ToList();

    public IReadOnlyList<PetResponse> GetBySexo(SexoPetEnum sexo) =>
        _petRepository.GetBySexo(sexo).Select(PetResponse.ToDTO).ToList();

    public IReadOnlyList<PetResponse> GetByNomeOuEspecie(string busca) =>
        _petRepository.GetByNomeOuEspecie(busca).Select(PetResponse.ToDTO).ToList();

    public IReadOnlyList<PetResponse> GetPetsComAlertasPendentes() =>
        _petRepository.GetPetsComAlertasPendentes().Select(PetResponse.ToDTO).ToList();

    public PetResponse Update(long id, PetRequest request)
    {
        var pet = _petRepository.GetById(id)
            ?? throw new KeyNotFoundException("Pet não encontrado.");

        pet.Transferir(request.Nome, request.Especie, request.Raca, request.Sexo, request.Idade, request.Peso, request.Tutor, request.Clinica);
        _petRepository.SaveChanges();
        return PetResponse.ToDTO(pet);
    }

    public bool Delete(long id) => _petRepository.Delete(id);
}
