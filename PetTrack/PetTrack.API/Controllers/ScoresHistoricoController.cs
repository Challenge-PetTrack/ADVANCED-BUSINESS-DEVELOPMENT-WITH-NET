using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Histórico de Scores de Saúde.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ScoresHistoricoController(IScoreHistoricoService scoreService) : ControllerBase
{
    /// <summary>
    /// Cria um novo Score Histórico.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ScoreHistoricoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] ScoreHistoricoRequest request)
    {
        var score = scoreService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = score.Id }, score);
    }

    /// <summary>
    /// Lista todos os Scores Históricos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ScoreHistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(scoreService.GetAll());
    }

    /// <summary>
    /// Retorna um Score Histórico pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ScoreHistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var score = scoreService.GetById(id);
        if (score is null)
            return NotFound($"Score com id '{id}' não encontrado.");
        return Ok(score);
    }

    /// <summary>
    /// Retorna histórico de Scores por pet.
    /// </summary>
    [HttpGet("historico/{idPet:long}")]
    [ProducesResponseType(typeof(IEnumerable<ScoreHistoricoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetHistoricoByPet(long idPet)
    {
        return Ok(scoreService.GetHistoricoByPet(idPet));
    }

    /// <summary>
    /// Retorna a média de Score por pet.
    /// </summary>
    [HttpGet("media/{idPet:long}")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    public IActionResult GetMediaByPet(long idPet)
    {
        return Ok(scoreService.GetMediaScoreByPet(idPet));
    }

    /// <summary>
    /// Atualiza os dados de um Score Histórico existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(ScoreHistoricoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] ScoreHistoricoRequest request)
    {
        var score = scoreService.Update(id, request);
        return Ok(score);
    }

    /// <summary>
    /// Remove um Score Histórico pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!scoreService.Delete(id))
            return NotFound($"Score com id '{id}' não encontrado.");
        return NoContent();
    }
}
