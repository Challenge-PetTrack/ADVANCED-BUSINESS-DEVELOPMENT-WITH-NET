namespace PetTrack.Application.DTO.Responses;

/// <summary>
/// DTO padrão para respostas de erro da API.
/// </summary>
/// <param name="Message">Descrição do erro retornado para o cliente.</param>
public record ErrorResponse(string Message)
{
    /// <summary>
    /// Cria uma resposta de erro com a mensagem informada.
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    /// <returns>Instância de <see cref="ErrorResponse"/>.</returns>
    public static ErrorResponse FromMessage(string message) => new(message);
}
