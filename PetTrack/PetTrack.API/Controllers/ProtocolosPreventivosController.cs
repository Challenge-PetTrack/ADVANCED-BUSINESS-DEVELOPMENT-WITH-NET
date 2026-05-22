using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Protocolos Preventivos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProtocolosPreventivosController(IProtocoloPreventivoService protocoloService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Protocolo Preventivo.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProtocoloPreventivoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ProtocoloPreventivoRequest request)
    {
        var protocolo = protocoloService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = protocolo.Id }, protocolo);
    }

    /// <summary>
    /// Lista todos os Protocolos Preventivos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProtocoloPreventivoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(protocoloService.GetAll());
    }

    /// <summary>
    /// Retorna um Protocolo Preventivo pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ProtocoloPreventivoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var protocolo = protocoloService.GetById(id);
        if (protocolo is null)
            return NotFound($"Protocolo com id '{id}' não encontrado.");
        return Ok(protocolo);
    }

    /// <summary>
    /// Retorna Protocolos Preventivos pelo tipo.
    /// </summary>
    [HttpGet("tipo")]
    [ProducesResponseType(typeof(IEnumerable<ProtocoloPreventivoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTipo([FromQuery] TipoProtocoloPreventivoEnum tipo)
    {
        return Ok(protocoloService.GetByTipo(tipo));
    }

    /// <summary>
    /// Retorna Protocolos pendentes ou atrasados por pet.
    /// </summary>
    [HttpGet("pendentes/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<ProtocoloPreventivoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetPendentesOuAtrasadosByPet(long idPet)
    {
        return Ok(protocoloService.GetPendentesOuAtrasadosByPet(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um Protocolo Preventivo existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ProtocoloPreventivoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] ProtocoloPreventivoRequest request)
    {
        var protocolo = protocoloService.Update(id, request);
        return Ok(protocolo);
    }

    /// <summary>
    /// Remove um Protocolo Preventivo pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!protocoloService.Delete(id))
            return NotFound($"Protocolo com id '{id}' não encontrado.");
        return NoContent();
    }
}
