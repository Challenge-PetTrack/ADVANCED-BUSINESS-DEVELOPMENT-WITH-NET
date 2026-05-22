using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;

namespace PetTrack.Application.Interfaces.Services;

public interface ITutorService
{
    TutorResponse Create(TutorRequest request);
    TutorResponse? GetById(long id);
    IReadOnlyList<TutorResponse> GetAll();
    IReadOnlyList<TutorResponse> GetByNomeOuEmail(string busca);
    IReadOnlyList<TutorResponse> GetByNomePet(string nomePet);
    TutorResponse Update(long id, TutorRequest request);
    bool Delete(long id);
}

public class TutorService : ITutorService
{
    private readonly ITutorRepository _tutorRepository;

    public TutorService(ITutorRepository tutorRepository)
    {
        _tutorRepository = tutorRepository;
    }

    public TutorResponse Create(TutorRequest request)
    {
        var tutor = request.ToEntity();
        _tutorRepository.Add(tutor);
        return TutorResponse.ToDTO(tutor);
    }

    public TutorResponse? GetById(long id)
    {
        var tutor = _tutorRepository.GetById(id);
        return tutor is null ? null : TutorResponse.ToDTO(tutor);
    }

    public IReadOnlyList<TutorResponse> GetAll() =>
        _tutorRepository.GetAll().Select(TutorResponse.ToDTO).ToList();

    public IReadOnlyList<TutorResponse> GetByNomeOuEmail(string busca) =>
        _tutorRepository.GetByNomeOuEmail(busca).Select(TutorResponse.ToDTO).ToList();

    public IReadOnlyList<TutorResponse> GetByNomePet(string nomePet) =>
        _tutorRepository.GetByNomePet(nomePet).Select(TutorResponse.ToDTO).ToList();

    public TutorResponse Update(long id, TutorRequest request)
    {
        var tutor = _tutorRepository.GetById(id)
            ?? throw new KeyNotFoundException("Tutor não encontrado.");

        tutor.Transferir(request.Nome, request.Email, request.Telefone, request.Endereco);
        _tutorRepository.SaveChanges();
        return TutorResponse.ToDTO(tutor);
    }

    public bool Delete(long id) => _tutorRepository.Delete(id);
}
