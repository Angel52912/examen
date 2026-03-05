## Implementación de SaleMapper.cs (C# 14, Deep Mapping, Native AOT)

### 1. Árbol de Directorios de la Capa de Infraestructura

```
C:\Users\angel\utmmarket\src\Infrastructure
├───Mappers
│   ├───ProductMapper.cs
│   └───SaleMapper.cs  <-- Archivo Creado
└───Models
    └───Data
        ├───DetalleVentaEntity.cs
        ├───ProductoEntity.cs
        └───VentaEntity.cs
```

### 2. Código Fuente Completo de 'SaleMapper.cs'

```csharp
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
```

### 3. Ejemplo de Uso en un Repositorio (Snippet)

Este snippet ilustra cómo un repositorio podría utilizar los métodos de extensión `ToDomain()` y `ToEntity()` de `SaleMapper` para gestionar la conversión entre las entidades de base de datos y las entidades de dominio, incluyendo sus detalles.

```csharp
using Dapper;
using System.Data;
using UtmMarket.Core.Entities;
using System.Collections.Generic;
using System.Linq;
// using UtmMarket.Core.Interfaces; // Descomentar si ISaleRepository está definido
using UtmMarket.Infrastructure.Mappers;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Repositories;

// Asumiendo una interfaz ISaleRepository en UtmMarket.Core.Interfaces
public class SaleRepository // : ISaleRepository
{
    private readonly IDbConnection _dbConnection;

    public SaleRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Obtiene una venta por su ID junto con sus detalles asociados, y la mapea al dominio.
    /// </summary>
    /// <param name="saleId">El ID de la venta.</param>
    /// <returns>La venta de dominio, o null si no se encuentra.</returns>
    public async Task<Sale?> GetByIdAsync(int saleId)
    {
        // Consulta para obtener la VentaEntity principal
        const string saleSql = "SELECT * FROM Ventas WHERE VentaID = @SaleId";
        var ventaEntity = await _dbConnection.QueryFirstOrDefaultAsync<VentaEntity>(saleSql, new { SaleId = saleId });

        if (ventaEntity is null)
        {
            return null;
        }

        // Consulta para obtener todos los DetalleVentaEntity asociados a esta venta
        const string detailSql = "SELECT * FROM DetalleVentas WHERE VentaID = @SaleId";
        var detalleEntities = (await _dbConnection.QueryAsync<DetalleVentaEntity>(detailSql, new { SaleId = saleId})).ToList();

        // Utiliza el método de extensión ToDomain para mapear la VentaEntity y sus detalles a Sale
        return ventaEntity.ToDomain(detalleEntities);
    }

    /// <summary>
    /// Añade una nueva venta al sistema, incluyendo sus detalles.
    /// </summary>
    /// <param name="sale">La venta de dominio a añadir.</param>
    public async Task AddAsync(Sale sale)
    {
        if (sale is null)
        {
            throw new ArgumentNullException(nameof(sale), "Cannot add a null sale.");
        }

        // Mapea la Sale de dominio a VentaEntity
        var ventaEntity = sale.ToEntity();
        if (ventaEntity is null)
        {
            throw new InvalidOperationException("Could not map Sale to VentaEntity.");
        }

        // Inserta la VentaEntity principal
        const string insertSaleSql = "INSERT INTO Ventas (VentaID, Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus) VALUES (@VentaID, @Folio, @FechaVenta, @TotalArticulos, @TotalVenta, @Estatus)";
        await _dbConnection.ExecuteAsync(insertSaleSql, ventaEntity);

        // Mapea los detalles de la venta a DetalleVentaEntity y los inserta
        var detalleEntities = sale.ToDetailEntities();
        if (detalleEntities.Any())
        {
            const string insertDetailSql = "INSERT INTO DetalleVentas (DetalleID, VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle) VALUES (@DetalleID, @VentaID, @ProductoID, @PrecioUnitario, @Cantidad, @TotalDetalle)";
            await _dbConnection.ExecuteAsync(insertDetailSql, detalleEntities);
        }
    }
}
```

### 4. Nota de Arquitectura: Beneficios de C# 14 Extension Members para Mapeo en "utmmarket"

El uso de "extension members" (métodos de extensión) de C# 14 en la implementación de `SaleMapper.cs` aporta ventajas arquitectónicas y de rendimiento cruciales para el proyecto "utmmarket", alineándose con los principios de Clean Architecture y la optimización para Native AOT:

*   **Coherencia con Clean Architecture:**
    *   **Separación Clara de Capas:** Los métodos de extensión permiten que las entidades de dominio (`Sale`, `SaleDetail`) y de persistencia (`VentaEntity`, `DetalleVentaEntity`) permanezcan "puras", sin depender directamente de la lógica de mapeo. La lógica de transformación reside en la capa de Infraestructura (`UtmMarket.Infrastructure.Mappers`), manteniendo una separación limpia de responsabilidades.
    *   **Acoplamiento Débil:** El mapeo se desacopla de las entidades. Cualquier cambio en la forma en que se realiza el mapeo no requiere modificar las entidades mismas, solo la clase `SaleMapper`.

*   **Optimización para Native AOT y Rendimiento:**
    *   **Cero Asignaciones de Instancia:** Los métodos de extensión, al ser estáticos, no requieren la instanciación de un objeto "mapper" para cada operación. Esto elimina la sobrecarga de creación de objetos y reduce la presión sobre el recolector de basura, lo que es fundamental para aplicaciones de alto rendimiento y esencial para la eficiencia del 'trimming' de Native AOT.
    *   **Ausencia de Reflexión:** La implementación utiliza asignaciones directas de propiedades y constructores primarios. La completa ausencia de reflexión es un requisito crítico para la compatibilidad con Native AOT, ya que la reflexión introduce llamadas dinámicas en tiempo de ejecución que no pueden ser optimizadas por el compilador AOT. Esto asegura que el código generado sea totalmente estático y compilable de forma nativa.
    *   **Mapeo Profundo Eficiente:** Incluso para el mapeo profundo de colecciones (detalles de la venta), se utilizan métodos LINQ y `foreach` para iterar y transformar los elementos de manera directa. Esto es intrínsecamente eficiente y compatible con AOT, ya que no introduce ninguna sobrecarga dinámica.

*   **Mejora de la Legibilidad y Usabilidad del Código:**
    *   **Sintaxis Natural y Fluida:** Permite expresiones de mapeo muy legibles como `ventaEntity.ToDomain(detalleEntities)` o `sale.ToEntity()`. Esto hace que el código que interactúa con las entidades de dominio y persistencia sea más expresivo y fácil de entender, ya que la operación de mapeo parece una capacidad natural del objeto.
    *   **Descubribilidad:** En entornos de desarrollo como Visual Studio, los métodos de extensión son fácilmente descubribles a través de IntelliSense, lo que mejora la productividad del desarrollador al proporcionar sugerencias de mapeo contextuales.
    *   **Consistencia:** Fomenta un patrón de mapeo consistente en todo el proyecto, lo que facilita el mantenimiento y la comprensión del codebase.

En resumen, los "extension members" no solo cumplen con los requisitos técnicos de rendimiento y AOT, sino que también refuerzan la adherencia a una arquitectura limpia, proporcionando una solución elegante y eficaz para la gestión de datos transaccionales en "utmmarket".
