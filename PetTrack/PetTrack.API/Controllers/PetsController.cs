using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Pets.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PetsController(IPetService petService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Pet.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] PetRequest request)
    {
        var pet = petService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    /// <summary>
    /// Lista todos os Pets.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(petService.GetAll());
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
    /// Atualiza os dados de um Pet existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] PetRequest request)
    {
        var pet = petService.Update(id, request);
        return Ok(pet);
    }

    /// <summary>
    /// Remove um Pet pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!petService.Delete(id))
            return NotFound($"Pet com id '{id}' não encontrado.");
        return NoContent();
    }
}
