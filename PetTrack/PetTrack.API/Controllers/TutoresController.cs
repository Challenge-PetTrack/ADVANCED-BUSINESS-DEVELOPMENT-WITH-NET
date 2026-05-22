using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Tutores (responsáveis pelos pets).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TutoresController(ITutorService tutorService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Tutor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] TutorRequest request)
    {
        var tutor = tutorService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, tutor);
    }

    /// <summary>
    /// Lista todos os Tutores.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(tutorService.GetAll());
    }

    /// <summary>
    /// Retorna um Tutor pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var tutor = tutorService.GetById(id);
        if (tutor is null)
            return NotFound($"Tutor com id '{id}' não encontrado.");
        return Ok(tutor);
    }

    /// <summary>
    /// Retorna Tutores pelo nome ou email.
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<TutorResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNomeOuEmail([FromQuery] string busca)
    {
        return Ok(tutorService.GetByNomeOuEmail(busca));
    }

    /// <summary>
    /// Retorna Tutores pelo nome de um pet cadastrado.
    /// </summary>
    [HttpGet("nomePet")]
    [ProducesResponseType(typeof(IEnumerable<TutorResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNomePet([FromQuery] string nomePet)
    {
        return Ok(tutorService.GetByNomePet(nomePet));
    }

    /// <summary>
    /// Atualiza os dados de um Tutor existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] TutorRequest request)
    {
        var tutor = tutorService.Update(id, request);
        return Ok(tutor);
    }

    /// <summary>
    /// Remove um Tutor pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!tutorService.Delete(id))
            return NotFound($"Tutor com id '{id}' não encontrado.");
        return NoContent();
    }
}
