namespace PetTrack.Application.DTO.Common;

/// <summary>
/// Resposta encapsulada para resultados paginados com metadados e suporte a HATEOAS.
/// </summary>
/// <typeparam name="T">Tipo do item retornado na página.</typeparam>
public class PagedResult<T>
{
    /// <summary>Itens da página atual.</summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>Página atual (1-based).</summary>
    public int PageNumber { get; init; }

    /// <summary>Tamanho da página.</summary>
    public int PageSize { get; init; }

    /// <summary>Total de registros encontrados.</summary>
    public int TotalItems { get; init; }

    /// <summary>Total de páginas calculadas.</summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalItems / (double)PageSize) : 0;

    /// <summary>Indica se existe página anterior.</summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>Indica se existe próxima página.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>Links HATEOAS para navegação entre páginas e ações.</summary>
    public List<LinkDto> Links { get; set; } = [];

    public PagedResult(IReadOnlyList<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
