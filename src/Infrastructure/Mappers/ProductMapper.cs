using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Mappers;

/// <summary>
/// Proporciona métodos de extensión estáticos para la conversión bidireccional
/// entre la entidad de dominio <see cref="Product"/> y la entidad de persistencia <see cref="ProductoEntity"/>.
/// Diseñado para ser compatible con Native AOT y optimizado para rendimiento en .NET 10 (C# 14).
/// </summary>
public static class ProductMapper
{
    /// <summary>
    /// Convierte un <see cref="ProductoEntity"/> de persistencia a una entidad de dominio <see cref="Product"/>.
    /// </summary>
    /// <param name="entity">La instancia de <see cref="ProductoEntity"/> a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="Product"/> si la entidad no es nula; de lo contrario, <c>null</c>.</returns>
    public static Product? ToDomain(this ProductoEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }

        // Utiliza el constructor primario del Product para una inicialización eficiente.
        // Se asegura que los tipos numéricos (decimal) mantengan su fidelidad.
        return new Product(
            productId: entity.ProductoID,
            name: entity.Nombre,
            sku: entity.SKU,
            brand: entity.Marca,
            price: entity.Precio,
            stock: entity.Stock
        );
    }

    /// <summary>
    /// Convierte una entidad de dominio <see cref="Product"/> a una entidad de persistencia <see cref="ProductoEntity"/>.
    /// </summary>
    /// <param name="product">La instancia de <see cref="Product"/> a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="ProductoEntity"/> si el producto no es nulo; de lo contrario, <c>null</c>.</returns>
    public static ProductoEntity? ToEntity(this Product? product)
    {
        if (product is null)
        {
            return null;
        }

        // Utiliza el constructor primario del ProductoEntity para una inicialización eficiente.
        // Se asegura que los tipos numéricos (decimal) mantengan su fidelidad.
        return new ProductoEntity(
            productoId: product.ProductID,
            nombre: product.Name,
            sku: product.SKU,
            marca: product.Brand,
            precio: product.Price,
            stock: product.Stock
        );
    }
}