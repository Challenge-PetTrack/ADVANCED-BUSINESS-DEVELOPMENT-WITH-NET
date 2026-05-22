using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Eventos Clínicos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EventosClinicosController(IEventoClinicoService eventoService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Evento Clínico.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EventoClinicoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] EventoClinicoRequest request)
    {
        var evento = eventoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    /// <summary>
    /// Lista todos os Eventos Clínicos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventoClinicoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(eventoService.GetAll());
    }

    /// <summary>
    /// Retorna um Evento Clínico pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(EventoClinicoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var evento = eventoService.GetById(id);
        if (evento is null)
            return NotFound($"Evento clínico com id '{id}' não encontrado.");
        return Ok(evento);
    }

    /// <summary>
    /// Retorna Eventos Clínicos pelo tipo.
    /// </summary>
    [HttpGet("tipo")]
    [ProducesResponseType(typeof(IEnumerable<EventoClinicoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTipo([FromQuery] TipoEventoClinicoEnum tipo)
    {
        return Ok(eventoService.GetByTipo(tipo));
    }

    /// <summary>
    /// Retorna Eventos Clínicos com medicamentos por pet.
    /// </summary>
    [HttpGet("medicamentos/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<EventoClinicoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetEventosComMedicamentos(long idPet)
    {
        return Ok(eventoService.GetEventosComMedicamentos(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um Evento Clínico existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(EventoClinicoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] EventoClinicoRequest request)
    {
        var evento = eventoService.Update(id, request);
        return Ok(evento);
    }

    /// <summary>
    /// Remove um Evento Clínico pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!eventoService.Delete(id))
            return NotFound($"Evento clínico com id '{id}' não encontrado.");
        return NoContent();
    }
}
