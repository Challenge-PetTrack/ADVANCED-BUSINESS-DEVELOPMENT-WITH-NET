using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// Controlador de operações NoSQL com MongoDB.
/// Gerencia a coleção 'pets' na base de documentos de alta performance 'pettrack_sprint4'.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MongoPetsController : ControllerBase
{
    private readonly IPetMongoService _mongoService;
    private readonly ILogger<MongoPetsController> _logger;

    public MongoPetsController(IPetMongoService mongoService, ILogger<MongoPetsController> logger)
    {
        _mongoService = mongoService;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os documentos de Pets persistidos no MongoDB com links HATEOAS.
    /// </summary>
    /// <response code="200">Lista de documentos NoSQL recuperada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetMongoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Consultando todos os documentos de pets no MongoDB.");
        var pets = await _mongoService.GetAllAsync();
        return Ok(pets);
    }

    /// <summary>
    /// Retorna um documento de Pet pelo seu identificador primário no MongoDB.
    /// </summary>
    /// <param name="id">Identificador do documento (_id / idPet).</param>
    /// <response code="200">Documento encontrado e retornado com links HATEOAS.</response>
    /// <response code="404">Documento não encontrado no MongoDB.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(PetMongoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var pet = await _mongoService.GetByIdAsync(id);
        if (pet is null)
            return NotFound(new { message = $"Pet com id '{id}' não encontrado no MongoDB." });

        return Ok(pet);
    }

    /// <summary>
    /// Consulta documentos de pets no MongoDB vinculados a um tutor específico.
    /// </summary>
    /// <param name="tutorId">Identificador do tutor.</param>
    [HttpGet("tutor/{tutorId:long}")]
    [ProducesResponseType(typeof(IEnumerable<PetMongoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTutor(long tutorId)
    {
        var pets = await _mongoService.GetByTutorIdAsync(tutorId);
        return Ok(pets);
    }

    /// <summary>
    /// Consulta documentos de pets no MongoDB vinculados a uma clínica veterinária.
    /// </summary>
    /// <param name="clinicaId">Identificador da clínica.</param>
    [HttpGet("clinica/{clinicaId:long}")]
    [ProducesResponseType(typeof(IEnumerable<PetMongoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClinica(long clinicaId)
    {
        var pets = await _mongoService.GetByClinicaIdAsync(clinicaId);
        return Ok(pets);
    }

    /// <summary>
    /// Insere um novo documento NoSQL na coleção 'pets' do MongoDB (requer autenticação JWT).
    /// </summary>
    /// <param name="request">Dados do pet desnormalizado para o MongoDB.</param>
    /// <response code="201">Documento inserido com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Não autenticado.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(PetMongoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] PetMongoRequest request)
    {
        var pet = await _mongoService.CreateAsync(request);
        _logger.LogInformation("Novo pet inserido no MongoDB com ID {Id}.", pet.Id);
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    /// <summary>
    /// Atualiza um documento existente no MongoDB (requer autenticação JWT).
    /// </summary>
    /// <param name="id">Identificador do documento a atualizar.</param>
    /// <param name="request">Novos dados do documento.</param>
    /// <response code="200">Documento atualizado com sucesso.</response>
    /// <response code="401">Não autenticado.</response>
    /// <response code="404">Documento não encontrado.</response>
    [HttpPut("{id:long}")]
    [Authorize]
    [ProducesResponseType(typeof(PetMongoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, [FromBody] PetMongoRequest request)
    {
        var pet = await _mongoService.UpdateAsync(id, request);
        return Ok(pet);
    }

    /// <summary>
    /// Remove um documento da coleção do MongoDB (requer autenticação JWT).
    /// </summary>
    /// <param name="id">Identificador do documento a excluir.</param>
    /// <response code="204">Documento removido com sucesso.</response>
    /// <response code="401">Não autenticado.</response>
    /// <response code="404">Documento não encontrado.</response>
    [HttpDelete("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await _mongoService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Pet com id '{id}' não encontrado no MongoDB." });

        _logger.LogInformation("Pet com ID {Id} removido do MongoDB.", id);
        return NoContent();
    }

    /// <summary>
    /// Sincroniza dados da base relacional (Oracle) para o banco de documentos NoSQL (MongoDB) (requer autenticação JWT).
    /// </summary>
    /// <response code="200">Quantidade de documentos sincronizados.</response>
    [HttpPost("sync")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SyncRelationalToMongo()
    {
        _logger.LogInformation("Iniciando sincronização relacional -> NoSQL MongoDB.");
        var total = await _mongoService.SyncRelationalToMongoAsync();
        return Ok(new
        {
            message = "Sincronização concluída com sucesso.",
            documentosSincronizados = total
        });
    }
}
