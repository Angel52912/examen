namespace UtmMarket.Core.Filters;

/// <summary>
/// Representa los criterios de filtro para buscar productos.
/// </summary>
public record ProductFilter
{
    /// <summary>
    /// Obtiene o establece el nombre del producto a buscar (búsqueda parcial o exacta).
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Obtiene o establece el SKU del producto a buscar (búsqueda parcial o exacta).
    /// </summary>
    public string? SKU { get; init; }

    /// <summary>
    /// Obtiene o establece la marca del producto a buscar (búsqueda parcial o exacta).
    /// </summary>
    public string? Brand { get; init; }

    /// <summary>
    /// Obtiene o establece el precio mínimo del producto.
    /// </summary>
    public decimal? MinPrice { get; init; }

    /// <summary>
    /// Obtiene o establece el precio máximo del producto.
    /// </summary>
    public decimal? MaxPrice { get; init; }

    /// <summary>
    /// Obtiene o establece el stock mínimo del producto.
    /// </summary>
    public int? MinStock { get; init; }

    /// <summary>
    /// Obtiene o establece el stock máximo del producto.
    /// </summary>
    public int? MaxStock { get; init; }
}
