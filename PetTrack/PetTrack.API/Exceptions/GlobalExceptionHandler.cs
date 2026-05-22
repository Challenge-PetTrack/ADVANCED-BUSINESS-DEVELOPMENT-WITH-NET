using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace PetTrack.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Excecao nao tratada: {Message}", exception.Message);

        var (statusCode, title, detail) = MapException(exception, environment);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = title,
            Status = statusCode,
            Detail = environment.IsDevelopment() ? detail : string.Empty,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }

    private static (int StatusCode, string Title, string? Detail) MapException(
        Exception exception,
        IHostEnvironment environment)
    {
        return exception switch
        {
            ArgumentNullException e       => (StatusCodes.Status400BadRequest,   "Requisicao invalida",                  e.Message),
            ArgumentException e           => (StatusCodes.Status400BadRequest,   "Requisicao invalida",                  e.Message),
            InvalidOperationException e   => (StatusCodes.Status400BadRequest,   "Nao foi possivel concluir a operacao", e.Message),
            KeyNotFoundException e        => (StatusCodes.Status404NotFound,     "Recurso nao encontrado",               e.Message),
            UnauthorizedAccessException e => (StatusCodes.Status401Unauthorized, "Nao autorizado",                       e.Message),
            OracleException e             => (StatusCodes.Status502BadGateway,   "Banco de dados indisponivel",          e.Message),
            _                             => MapUnhandled(environment, exception)
        };
    }

    private static (int StatusCode, string Title, string? Detail) MapUnhandled(
        IHostEnvironment environment,
        Exception exception)
    {
        return (
            StatusCodes.Status500InternalServerError,
            "Erro interno do servidor",
            "Ocorreu um erro inesperado. Tente novamente mais tarde.");
    }
}