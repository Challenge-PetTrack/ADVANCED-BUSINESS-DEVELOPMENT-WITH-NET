using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Leituras do Collar IoT.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CollarLeiturasController(ICollarLeituraService collarService) : ControllerBase
{
    /// <summary>
    /// Cria uma nova Leitura do Collar.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CollarLeituraResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CollarLeituraRequest request)
    {
        var leitura = collarService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = leitura.Id }, leitura);
    }

    /// <summary>
    /// Lista todas as Leituras do Collar.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CollarLeituraResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(collarService.GetAll());
    }

    /// <summary>
    /// Retorna uma Leitura pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(CollarLeituraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var leitura = collarService.GetById(id);
        if (leitura is null)
            return NotFound($"Leitura com id '{id}' não encontrada.");
        return Ok(leitura);
    }

    /// <summary>
    /// Retorna Leituras por pet com temperatura acima de um valor.
    /// </summary>
    [HttpGet("temperatura")]
    [ProducesResponseType(typeof(IEnumerable<CollarLeituraResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTemperatura([FromQuery] long idPet, [FromQuery] double temperatura)
    {
        return Ok(collarService.GetByPetETemperaturaAcimaDe(idPet, temperatura));
    }

    /// <summary>
    /// Retorna a última Leitura registrada por pet.
    /// </summary>
    [HttpGet("ultima/{idPet:long}")]
    [ProducesResponseType(typeof(CollarLeituraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetUltimaByPet(long idPet)
    {
        var leitura = collarService.GetUltimaLeituraByPet(idPet);
        if (leitura is null)
            return NotFound($"Nenhuma leitura encontrada para o pet '{idPet}'.");
        return Ok(leitura);
    }

    /// <summary>
    /// Atualiza os dados de uma Leitura existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(CollarLeituraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] CollarLeituraRequest request)
    {
        var leitura = collarService.Update(id, request);
        return Ok(leitura);
    }

    /// <summary>
    /// Remove uma Leitura pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!collarService.Delete(id))
            return NotFound($"Leitura com id '{id}' não encontrada.");
        return NoContent();
    }
}
