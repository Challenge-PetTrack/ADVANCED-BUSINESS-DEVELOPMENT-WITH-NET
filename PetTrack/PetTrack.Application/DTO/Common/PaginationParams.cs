namespace PetTrack.Application.DTO.Common;

/// <summary>
/// Parâmetros padronizados para consultas paginadas e ordenadas.
/// </summary>
public class PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    /// <summary>Número da página atual (1-based).</summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>Quantidade de itens por página (máximo 50).</summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 1 : value);
    }

    /// <summary>Campo de ordenação (ex: Nome, Id, Idade, Peso).</summary>
    public string? SortBy { get; set; } = "Nome";

    /// <summary>Direção da ordenação: 'asc' para ascendente ou 'desc' para descendente.</summary>
    public string? SortDirection { get; set; } = "asc";
}
