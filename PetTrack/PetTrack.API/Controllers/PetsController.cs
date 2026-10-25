using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Common;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Pets com suporte a paginação, filtros, ordenação, HATEOAS e autenticação JWT.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PetsController(IPetService petService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Pet (requer autenticação JWT).
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Create([FromBody] PetRequest request)
    {
        var pet = petService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    /// <summary>
    /// Lista todos os Pets com links HATEOAS.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(petService.GetAll());
    }

    /// <summary>
    /// Lista pets com paginação, ordenação configurável, filtros dinâmicos e links HATEOAS.
    /// </summary>
    /// <param name="filters">Filtros de busca (nome, espécie, raça, sexo, clínica, tutor) e parâmetros de paginação.</param>
    /// <response code="200">Resultado paginado com metadados e hipermídia HATEOAS.</response>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResult<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetPaged([FromQuery] PetFilterParams filters)
    {
        return Ok(petService.GetPaged(filters));
    }

    /// <summary>
    /// Retorna um Pet pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var pet = petService.GetById(id);
        if (pet is null)
            return NotFound($"Pet com id '{id}' não encontrado.");
        return Ok(pet);
    }

    /// <summary>
    /// Retorna Pets pela clínica.
    /// </summary>
    [HttpGet("clinica/{idClinica:long}")]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByClinica(long idClinica)
    {
        return Ok(petService.GetByClinicaId(idClinica));
    }

    /// <summary>
    /// Retorna Pets pelo sexo.
    /// </summary>
    [HttpGet("sexo")]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetBySexo([FromQuery] SexoPetEnum sexo)
    {
        return Ok(petService.GetBySexo(sexo));
    }

    /// <summary>
    /// Retorna Pets pelo nome ou espécie.
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNomeOuEspecie([FromQuery] string busca)
    {
        return Ok(petService.GetByNomeOuEspecie(busca));
    }

    /// <summary>
    /// Retorna Pets com alertas pendentes.
    /// </summary>
    [HttpGet("alertasPendentes")]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetPetsComAlertasPendentes()
    {
        return Ok(petService.GetPetsComAlertasPendentes());
    }

    /// <summary>
    /// Atualiza os dados de um Pet existente (requer autenticação JWT).
    /// </summary>
    [HttpPut("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] PetRequest request)
    {
        var pet = petService.Update(id, request);
        return Ok(pet);
    }

    /// <summary>
    /// Remove um Pet pelo identificador (requer autenticação JWT).
    /// </summary>
    [HttpDelete("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!petService.Delete(id))
            return NotFound($"Pet com id '{id}' não encontrado.");
        return NoContent();
    }
}
