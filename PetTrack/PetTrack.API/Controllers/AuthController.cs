using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTrack.Application.DTO.Requests;
using PetTrack.Application.DTO.Responses;
using PetTrack.Application.Interfaces.Services;

namespace PetTrack.API.Controllers;

/// <summary>
/// Controlador de autenticação JWT da API PetTrack.
/// Permite obter tokens de acesso para execução de operações protegidas.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Realiza a autenticação do usuário retornando o token JWT Bearer.
    /// </summary>
    /// <param name="request">Credenciais de acesso (email e senha).</param>
    /// <response code="200">Autenticação realizada com sucesso. Retorna o token JWT e dados do usuário.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Tentativa de login para o usuário: {Email}", request.Email);

        var response = _tokenService.Authenticate(request);
        if (response is null)
        {
            _logger.LogWarning("Falha na autenticação para o usuário: {Email}", request.Email);
            return Unauthorized(new { message = "E-mail ou senha inválidos." });
        }

        _logger.LogInformation("Usuário {Email} autenticado com sucesso. Perfil: {Role}", response.Email, response.Role);
        return Ok(response);
    }

    /// <summary>
    /// Retorna as informações do usuário autenticado no momento a partir do token JWT.
    /// </summary>
    /// <response code="200">Dados do perfil do usuário autenticado.</response>
    /// <response code="401">Token JWT ausente ou inválido.</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetMe()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        var name = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            Email = email,
            Name = name,
            Role = role,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
