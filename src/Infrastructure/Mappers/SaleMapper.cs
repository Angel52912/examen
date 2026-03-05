using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;
using System.Linq; // For LINQ operations like Select and ToList
using System.Collections.Generic; // For List

namespace UtmMarket.Infrastructure.Mappers;

/// <summary>
/// Proporciona métodos de extensión estáticos para la conversión bidireccional profunda
/// entre las entidades de dominio (<see cref="Sale"/>, <see cref="SaleDetail"/>)
/// y las entidades de persistencia (<see cref="VentaEntity"/>, <see cref="DetalleVentaEntity"/>).
/// Diseñado para ser compatible con Native AOT y optimizado para rendimiento en .NET 10 (C# 14).
/// </summary>
public static class SaleMapper
{
    // Mapeo de SaleDetail <-> DetalleVentaEntity
    // Estos métodos son internos porque están destinados a ser usados por el mapeo de Sale
    // y no directamente por componentes externos, manteniendo la encapsulación del mapeo profundo.

    /// <summary>
    /// Convierte un <see cref="DetalleVentaEntity"/> a una entidad de dominio <see cref="SaleDetail"/>.
    /// Asume que el <see cref="Product"/> asociado se cargará por separado si es necesario,
    /// o que un Product con solo su ID es suficiente para la inicialización.
    /// </summary>
    /// <param name="entity">La instancia de <see cref="DetalleVentaEntity"/> a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="SaleDetail"/> si la entidad no es nula; de lo contrario, <c>null</c>.</returns>
    internal static SaleDetail? ToDomain(this DetalleVentaEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }

        // Se requiere un objeto Product para el constructor de SaleDetail.
        // Para simplificar, creamos un Product dummy con solo el ProductID.
        // En una implementación real, este Product se recuperaría del dominio (ej. desde un repositorio de productos).
        var dummyProduct = new Product(entity.ProductoID, "Dummy Product", "", null, 0, 0);

        return new SaleDetail(
            detailId: entity.DetalleID,
            product: dummyProduct, // Placeholder product para cumplir con el constructor
            unitPrice: entity.PrecioUnitario,
            quantity: entity.Cantidad
        );
    }

    /// <summary>
    /// Convierte una entidad de dominio <see cref="SaleDetail"/> a un <see cref="DetalleVentaEntity"/>.
    /// </summary>
    /// <param name="saleDetail">La instancia de <see cref="SaleDetail"/> a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="DetalleVentaEntity"/> si el detalle no es nulo; de lo contrario, <c>null</c>.</returns>
    internal static DetalleVentaEntity? ToEntity(this SaleDetail? saleDetail)
    {
        if (saleDetail is null)
        {
            return null;
        }

        return new DetalleVentaEntity(
            detalleId: saleDetail.DetalleID,
            ventaId: saleDetail.VentaID,
            productoId: saleDetail.ProductID,
            precioUnitario: saleDetail.UnitPrice,
            cantidad: saleDetail.Quantity,
            totalDetalle: saleDetail.TotalDetalle // Utiliza el valor calculado
        );
    }

    // Mapeo de Sale <-> VentaEntity (con mapeo profundo de detalles)

    /// <summary>
    /// Convierte un <see cref="VentaEntity"/> de persistencia a una entidad de dominio <see cref="Sale"/>,
    /// incluyendo la conversión de sus detalles asociados.
    /// </summary>
    /// <param name="entity">La instancia de <see cref="VentaEntity"/> a convertir.</param>
    /// <param name="detalleEntities">La colección de <see cref="DetalleVentaEntity"/> asociados a esta venta. Puede ser nula.</param>
    /// <returns>Una nueva instancia de <see cref="Sale"/> si la entidad no es nula; de lo contrario, <c>null</c>.</returns>
    public static Sale? ToDomain(this VentaEntity? entity, IEnumerable<DetalleVentaEntity>? detalleEntities = null)
    {
        if (entity is null)
        {
            return null;
        }

        // Crear la instancia de Sale con el constructor primario.
        var sale = new Sale(
            saleId: entity.VentaID,
            folio: entity.Folio,
            status: (SaleStatus)entity.Estatus // Conversión explícita de byte a enum
        )
        {
            SaleDate = entity.FechaVenta
        };

        // Mapear y añadir detalles si se proporcionan
        if (detalleEntities?.Any() == true)
        {
            foreach (var detalleEntity in detalleEntities)
            {
                // Asegurarse de que el detalle pertenezca a esta venta antes de mapear y añadir
                if (detalleEntity.VentaID == entity.VentaID)
                {
                    var saleDetail = detalleEntity.ToDomain();
                    if (saleDetail is not null)
                    {
                        sale.AddDetail(saleDetail);
                    }
                }
            }
        }

        return sale;
    }

    /// <summary>
    /// Convierte una entidad de dominio <see cref="Sale"/> a un <see cref="VentaEntity"/> de persistencia,
    /// incluyendo la conversión de sus propiedades calculadas.
    /// </summary>
    /// <param name="sale">La instancia de <see cref="Sale"/> a convertir.</param>
    /// <returns>Una nueva instancia de <see cref="VentaEntity"/> si la venta no es nula; de lo contrario, <c>null</c>.</returns>
    public static VentaEntity? ToEntity(this Sale? sale)
    {
        if (sale is null)
        {
            return null;
        }

        return new VentaEntity(
            ventaId: sale.SaleID,
            folio: sale.Folio,
            fechaVenta: sale.SaleDate,
            totalArticulos: sale.TotalItems, // Utiliza el valor calculado de Sale
            totalVenta: sale.TotalSale,       // Utiliza el valor calculado de Sale
            estatus: (byte)sale.Status      // Conversión explícita de enum a byte
        );
    }

    /// <summary>
    /// Convierte la colección de detalles de una entidad de dominio <see cref="Sale"/>
    /// a una lista de <see cref="DetalleVentaEntity"/>.
    /// Este método es útil cuando se necesita obtener solo la colección de detalles para persistencia,
    /// por ejemplo, para una inserción/actualización en cascada.
    /// </summary>
    /// <param name="sale">La instancia de <see cref="Sale"/> cuyos detalles se van a convertir.</param>
    /// <returns>Una lista de <see cref="DetalleVentaEntity"/>. Devuelve una lista vacía si no hay detalles o la venta es nula.</returns>
    public static List<DetalleVentaEntity> ToDetailEntities(this Sale? sale)
    {
        if (sale?.SaleDetails is null)
        {
            return new List<DetalleVentaEntity>();
        }

        return sale.SaleDetails
            .Select(detail => detail.ToEntity()) // Utiliza el mapeo interno de SaleDetail a DetalleVentaEntity
            .Where(detailEntity => detailEntity is not null) // Filtra nulos si ToEntity puede devolverlos
            .ToList()!; // El '!' es para asegurar al compilador que no habrá nulos después del Where, dado el null check inicial.
    }
}
