using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;

namespace UtmMarket.Core.Repositories;

/// <summary>
/// Define el contrato para las operaciones de acceso a datos relacionadas con la entidad <see cref="Product"/>.
/// Esta interfaz es parte de la capa de dominio y no debe depender de detalles de implementación de persistencia.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Recupera todos los productos de forma asíncrona.
    /// Utiliza <see cref="IAsyncEnumerable{T}"/> para permitir el streaming de datos,
    /// optimizando el uso de memoria para grandes conjuntos de resultados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una enumeración asíncrona de <see cref="Product"/>.</returns>
    IAsyncEnumerable<Product> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Recupera un producto específico por su identificador único de forma asíncrona.
    /// </summary>
    /// <param name="productId">El identificador único del producto.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una <see cref="Task{TResult}"/> que representa la operación asíncrona,
    /// conteniendo el <see cref="Product"/> encontrado o <c>null</c> si no existe.</returns>
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca productos que coincidan con los criterios especificados en el filtro.
    /// Utiliza <see cref="IAsyncEnumerable{T}"/> para el streaming de datos de los resultados.
    /// </summary>
    /// <param name="filter">Un objeto <see cref="ProductFilter"/> con los criterios de búsqueda.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una enumeración asíncrona de <see cref="Product"/> que cumplen con el filtro.</returns>
    IAsyncEnumerable<Product> FindAsync(ProductFilter filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega un nuevo producto al repositorio de forma asíncrona.
    /// </summary>
    /// <param name="product">El objeto <see cref="Product"/> a agregar. El ID del producto puede ser 0
    /// si es generado por la fuente de datos.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una <see cref="Task{TResult}"/> que representa la operación asíncrona,
    /// conteniendo el <see cref="Product"/> con cualquier ID generado por la persistencia.</returns>
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un producto existente en el repositorio de forma asíncrona.
    /// </summary>
    /// <param name="product">El objeto <see cref="Product"/> con los datos actualizados.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una <see cref="Task{TResult}"/> que representa la operación asíncrona,
    /// conteniendo <c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza de forma asíncrona solo la cantidad de stock de un producto específico.
    /// Este es un "Atomic Update" para minimizar colisiones y mejorar la eficiencia.
    /// </summary>
    /// <param name="productId">El identificador único del producto cuyo stock se va a actualizar.</param>
    /// <param name="newStock">La nueva cantidad de stock para el producto.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Una <see cref="Task{TResult}"/> que representa la operación asíncrona,
    /// conteniendo <c>true</c> si la actualización del stock fue exitosa; de lo contrario, <c>false</c>.</returns>
    Task<bool> UpdateStockAsync(int productId, int newStock, CancellationToken cancellationToken = default);
}
