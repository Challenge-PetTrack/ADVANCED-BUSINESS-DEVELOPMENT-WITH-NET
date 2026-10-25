using PetTrack.Application.DTO.Common;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Repositories;
using PetTrack.Application.Metrics;
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
    PagedResult<PetResponse> GetPaged(PetFilterParams filters);
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

    public static PetResponse EnrichWithLinks(PetResponse pet)
    {
        pet.Links =
        [
            new LinkDto($"/api/pets/{pet.Id}", "self", "GET"),
            new LinkDto($"/api/pets/{pet.Id}", "update", "PUT"),
            new LinkDto($"/api/pets/{pet.Id}", "delete", "DELETE"),
            new LinkDto("/api/pets", "all", "GET"),
            new LinkDto("/api/pets/paged", "paged", "GET")
        ];
        return pet;
    }

    public PetResponse Create(PetRequest request)
    {
        var pet = request.ToEntity();
        _petRepository.Add(pet);
        PetTrackMetrics.PetsCreatedCounter.Add(1);
        return EnrichWithLinks(PetResponse.ToDTO(pet));
    }

    public PetResponse? GetById(long id)
    {
        var pet = _petRepository.GetById(id);
        return pet is null ? null : EnrichWithLinks(PetResponse.ToDTO(pet));
    }

    public IReadOnlyList<PetResponse> GetAll() =>
        _petRepository.GetAll().Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();

    public IReadOnlyList<PetResponse> GetByClinicaId(long idClinica) =>
        _petRepository.GetByClinicaId(idClinica).Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();

    public IReadOnlyList<PetResponse> GetBySexo(SexoPetEnum sexo) =>
        _petRepository.GetBySexo(sexo).Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();

    public IReadOnlyList<PetResponse> GetByNomeOuEspecie(string busca) =>
        _petRepository.GetByNomeOuEspecie(busca).Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();

    public IReadOnlyList<PetResponse> GetPetsComAlertasPendentes() =>
        _petRepository.GetPetsComAlertasPendentes().Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();

    public PagedResult<PetResponse> GetPaged(PetFilterParams filters)
    {
        var (items, total) = _petRepository.GetPaged(
            filters.Nome,
            filters.Especie,
            filters.Raca,
            filters.Sexo,
            filters.ClinicaId,
            filters.TutorId,
            filters.SortBy,
            filters.SortDirection,
            filters.PageNumber,
            filters.PageSize);

        var dtos = items.Select(PetResponse.ToDTO).Select(EnrichWithLinks).ToList();
        var pagedResult = new PagedResult<PetResponse>(dtos, total, filters.PageNumber, filters.PageSize);

        var queryBase = $"/api/pets/paged?pageSize={filters.PageSize}&sortBy={filters.SortBy}&sortDirection={filters.SortDirection}";
        if (!string.IsNullOrWhiteSpace(filters.Nome)) queryBase += $"&nome={Uri.EscapeDataString(filters.Nome)}";
        if (!string.IsNullOrWhiteSpace(filters.Especie)) queryBase += $"&especie={Uri.EscapeDataString(filters.Especie)}";
        if (filters.Sexo.HasValue) queryBase += $"&sexo={filters.Sexo.Value}";

        pagedResult.Links.Add(new LinkDto($"{queryBase}&pageNumber={filters.PageNumber}", "self", "GET"));
        pagedResult.Links.Add(new LinkDto($"{queryBase}&pageNumber=1", "first", "GET"));
        if (pagedResult.TotalPages > 0)
            pagedResult.Links.Add(new LinkDto($"{queryBase}&pageNumber={pagedResult.TotalPages}", "last", "GET"));
        if (pagedResult.HasNextPage)
            pagedResult.Links.Add(new LinkDto($"{queryBase}&pageNumber={filters.PageNumber + 1}", "next", "GET"));
        if (pagedResult.HasPreviousPage)
            pagedResult.Links.Add(new LinkDto($"{queryBase}&pageNumber={filters.PageNumber - 1}", "prev", "GET"));
        pagedResult.Links.Add(new LinkDto("/api/pets", "create", "POST"));

        return pagedResult;
    }

    public PetResponse Update(long id, PetRequest request)
    {
        var pet = _petRepository.GetById(id)
            ?? throw new KeyNotFoundException("Pet não encontrado.");

        pet.Transferir(request.Nome, request.Especie, request.Raca, request.Sexo, request.Idade, request.Peso, request.Tutor, request.Clinica);
        _petRepository.SaveChanges();
        return EnrichWithLinks(PetResponse.ToDTO(pet));
    }

    public bool Delete(long id) => _petRepository.Delete(id);
}
