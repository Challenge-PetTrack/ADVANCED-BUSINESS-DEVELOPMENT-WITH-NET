using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Alertas.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AlertasController(IAlertaService alertaService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Alerta.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AlertaRequest request)
    {
        var alerta = alertaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, alerta);
    }

    /// <summary>
    /// Lista todos os Alertas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlertaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(alertaService.GetAll());
    }

    /// <summary>
    /// Retorna um Alerta pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var alerta = alertaService.GetById(id);
        if (alerta is null)
            return NotFound($"Alerta com id '{id}' não encontrado.");
        return Ok(alerta);
    }

    /// <summary>
    /// Retorna Alertas pelo tipo.
    /// </summary>
    [HttpGet("tipo")]
    [ProducesResponseType(typeof(IEnumerable<AlertaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTipo([FromQuery] TipoAlertaEnum tipo)
    {
        return Ok(alertaService.GetByTipo(tipo));
    }

    /// <summary>
    /// Retorna Alertas pendentes por pet.
    /// </summary>
    [HttpGet("pendentes/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<AlertaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetPendentesByPet(long idPet)
    {
        return Ok(alertaService.GetPendentesByPet(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um Alerta existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(AlertaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] AlertaRequest request)
    {
        var alerta = alertaService.Update(id, request);
        return Ok(alerta);
    }

    /// <summary>
    /// Remove um Alerta pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!alertaService.Delete(id))
            return NotFound($"Alerta com id '{id}' não encontrado.");
        return NoContent();
    }
}
