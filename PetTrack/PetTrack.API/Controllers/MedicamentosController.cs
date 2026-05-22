using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Medicamentos.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MedicamentosController(IMedicamentoService medicamentoService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Medicamento.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] MedicamentoRequest request)
    {
        var medicamento = medicamentoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = medicamento.Id }, medicamento);
    }

    /// <summary>
    /// Lista todos os Medicamentos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(medicamentoService.GetAll());
    }

    /// <summary>
    /// Retorna um Medicamento pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var medicamento = medicamentoService.GetById(id);
        if (medicamento is null)
            return NotFound($"Medicamento com id '{id}' não encontrado.");
        return Ok(medicamento);
    }

    /// <summary>
    /// Retorna Medicamentos pelo nome.
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<MedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByNome([FromQuery] string nome)
    {
        return Ok(medicamentoService.GetByNome(nome));
    }

    /// <summary>
    /// Retorna Medicamentos ativos por pet.
    /// </summary>
    [HttpGet("ativos/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<MedicamentoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetMedicamentosAtivosByPet(long idPet)
    {
        return Ok(medicamentoService.GetMedicamentosAtivosByPet(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um Medicamento existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(MedicamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] MedicamentoRequest request)
    {
        var medicamento = medicamentoService.Update(id, request);
        return Ok(medicamento);
    }

    /// <summary>
    /// Remove um Medicamento pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!medicamentoService.Delete(id))
            return NotFound($"Medicamento com id '{id}' não encontrado.");
        return NoContent();
    }
}
