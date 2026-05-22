using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;
using PetTrack.Domain.Enum;

namespace PetTrack.API.Controllers;

/// <summary>
/// CRUD de Notificações.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class NotificacoesController(INotificacaoService notificacaoService) : ControllerBase
{
    /// <summary>
    /// Cria uma nova Notificação.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(NotificacaoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] NotificacaoRequest request)
    {
        var notificacao = notificacaoService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = notificacao.Id }, notificacao);
    }

    /// <summary>
    /// Lista todas as Notificações.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificacaoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(notificacaoService.GetAll());
    }

    /// <summary>
    /// Retorna uma Notificação pelo identificador.
    /// </summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(NotificacaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var notificacao = notificacaoService.GetById(id);
        if (notificacao is null)
            return NotFound($"Notificação com id '{id}' não encontrada.");
        return Ok(notificacao);
    }

    /// <summary>
    /// Retorna Notificações pelo status.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(IEnumerable<NotificacaoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByStatus([FromQuery] SimNaoEnum status)
    {
        return Ok(notificacaoService.GetByStatus(status));
    }

    /// <summary>
    /// Retorna Notificações pelo tipo.
    /// </summary>
    [HttpGet("tipo")]
    [ProducesResponseType(typeof(IEnumerable<NotificacaoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetByTipo([FromQuery] TipoNotificacaoEnum tipo)
    {
        return Ok(notificacaoService.GetByTipo(tipo));
    }

    /// <summary>
    /// Retorna Notificações urgentes não lidas por Tutor.
    /// </summary>
    [HttpGet("urgentes/{idTutor:long}")]
    [ProducesResponseType(typeof(IEnumerable<NotificacaoResponse>), StatusCodes.Status200OK)]
    public IActionResult GetUrgentesNaoLidasByTutor(long idTutor)
    {
        return Ok(notificacaoService.GetUrgentesNaoLidasByTutor(idTutor));
    }

    /// <summary>
    /// Atualiza os dados de uma Notificação existente.
    /// </summary>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(NotificacaoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, [FromBody] NotificacaoRequest request)
    {
        var notificacao = notificacaoService.Update(id, request);
        return Ok(notificacao);
    }

    /// <summary>
    /// Remove uma Notificação pelo identificador.
    /// </summary>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(long id)
    {
        if (!notificacaoService.Delete(id))
            return NotFound($"Notificação com id '{id}' não encontrada.");
        return NoContent();
    }
}
