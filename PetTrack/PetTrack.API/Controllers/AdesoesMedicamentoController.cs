using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Adesões ao Medicamento.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AdesoesMedicamentoController(IAdesaoMedicamentoService adesaoService) : ControllerBase
{
    /// <summary>
    /// Cria uma nova Adesão ao Medicamento.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AdesaoMedicamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] AdesaoMedicamentoRequest request)
    {
        var adesao = adesaoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = adesao.Id }, adesao);
    }

    /// <summary>
    /// Lista todas as Adesões ao Medicamento.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AdesaoMedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(adesaoService.GetAll());
    }

    /// <summary>
    /// Retorna uma Adesão pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AdesaoMedicamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var adesao = adesaoService.GetById(id);
        if (adesao is null)
            return NotFound($"Adesão com id '{id}' não encontrada.");
        return Ok(adesao);
    }

    /// <summary>
    /// Retorna Adesões pelo id do Medicamento.
    /// </summary>
    [HttpGet("medicamento/{idMedicamento:long}")]
    [ProducesResponseType(typeof(IEnumerable<AdesaoMedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByMedicamento(long idMedicamento)
    {
        return Ok(adesaoService.GetByMedicamentoId(idMedicamento));
    }

    /// <summary>
    /// Retorna Adesões pelo status.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(IEnumerable<AdesaoMedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByStatus([FromQuery] SimNaoEnum status)
    {
        return Ok(adesaoService.GetByStatus(status));
    }

    /// <summary>
    /// Atualiza os dados de uma Adesão existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(AdesaoMedicamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] AdesaoMedicamentoRequest request)
    {
        var adesao = adesaoService.Update(id, request);
        return Ok(adesao);
    }

    /// <summary>
    /// Remove uma Adesão pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!adesaoService.Delete(id))
            return NotFound($"Adesão com id '{id}' não encontrada.");
        return NoContent();
    }
}
