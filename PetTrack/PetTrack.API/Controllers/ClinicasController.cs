using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Clínicas Veterinárias.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClinicasController(IClinicaService clinicaService) : ControllerBase
{
    /// <summary>
    /// Cria uma nova Clínica.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClinicaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ClinicaRequest request)
    {
        var clinica = clinicaService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = clinica.Id }, clinica);
    }

    /// <summary>
    /// Lista todas as Clínicas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClinicaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(clinicaService.GetAll());
    }

    /// <summary>
    /// Retorna uma Clínica pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ClinicaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var clinica = clinicaService.GetById(id);
        if (clinica is null)
            return NotFound($"Clínica com id '{id}' não encontrada.");
        return Ok(clinica);
    }

    /// <summary>
    /// Retorna Clínicas pelo nome ou CNPJ.
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<ClinicaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNomeOuCnpj([FromQuery] string busca)
    {
        return Ok(clinicaService.GetByNomeOuCnpj(busca));
    }

    /// <summary>
    /// Retorna Clínicas pelo nome de um pet cadastrado.
    /// </summary>
    [HttpGet("nomePet")]
    [ProducesResponseType(typeof(IEnumerable<ClinicaResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNomePet([FromQuery] string nomePet)
    {
        return Ok(clinicaService.GetByNomePet(nomePet));
    }

    /// <summary>
    /// Atualiza os dados de uma Clínica existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ClinicaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] ClinicaRequest request)
    {
        var clinica = clinicaService.Update(id, request);
        return Ok(clinica);
    }

    /// <summary>
    /// Remove uma Clínica pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!clinicaService.Delete(id))
            return NotFound($"Clínica com id '{id}' não encontrada.");
        return NoContent();
    }
}
