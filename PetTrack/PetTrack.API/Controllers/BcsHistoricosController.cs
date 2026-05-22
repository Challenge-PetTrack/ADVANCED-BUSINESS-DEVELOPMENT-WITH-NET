using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Histórico de BCS (Body Condition Score).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BcsHistoricosController(IBcsHistoricoService bcsService) : ControllerBase
{
    /// <summary>
    /// Cria um novo registro de BCS.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BcsHistoricoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] BCSHistoricoRequest request)
    {
        var bcs = bcsService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = bcs.Id }, bcs);
    }

    /// <summary>
    /// Lista todos os registros de BCS.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BcsHistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(bcsService.GetAll());
    }

    /// <summary>
    /// Retorna um registro de BCS pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(BcsHistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var bcs = bcsService.GetById(id);
        if (bcs is null)
            return NotFound($"BCS com id '{id}' não encontrado.");
        return Ok(bcs);
    }

    /// <summary>
    /// Retorna histórico de BCS por pet.
    /// </summary>
    [HttpGet("historico/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<BcsHistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetHistoricoByPet(long idPet)
    {
        return Ok(bcsService.GetHistoricoByPet(idPet));
    }

    /// <summary>
    /// Retorna a média de BCS por pet.
    /// </summary>
    [HttpGet("media/{idPet:long}")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    public IActionResult GetMediaByPet(long idPet)
    {
        return Ok(bcsService.GetMediaBcsByPet(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um registro de BCS existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(BcsHistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] BCSHistoricoRequest request)
    {
        var bcs = bcsService.Update(id, request);
        return Ok(bcs);
    }

    /// <summary>
    /// Remove um registro de BCS pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!bcsService.Delete(id))
            return NotFound($"BCS com id '{id}' não encontrado.");
        return NoContent();
    }
}
