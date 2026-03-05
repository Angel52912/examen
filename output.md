## Implementación de ProductMapper.cs (C# 14, Native AOT)

### 1. Árbol de Directorios Actualizado

```
C:\Users\angel\utmmarket
├───src
│   ├───Core
│   │   └───Entities
│   │       ├───Product.cs
│   │       └───Sale.cs
│   │       ├───SaleDetail.cs
│   │       └───SaleStatus.cs
│   └───Infrastructure
│       ├───Mappers
│       │   └───ProductMapper.cs  <-- Archivo Creado
│       └───Models
│           └───Data
│               ├───DetalleVentaEntity.cs
│               ├───ProductoEntity.cs
│               └───VentaEntity.cs
```

### 2. Código Fuente Completo de 'ProductMapper.cs'

```csharp
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
```

### 3. Ejemplo de Uso en un Repositorio (Snippet)

Este snippet ilustra cómo un repositorio podría utilizar los métodos de extensión `ToDomain()` y `ToEntity()` para gestionar la conversión entre las entidades de base de datos y las entidades de dominio.

```csharp
using Dapper;
using System.Data;
using UtmMarket.Core.Entities;
// using UtmMarket.Core.Interfaces; // Descomentar si IProductRepository está definido
using UtmMarket.Infrastructure.Mappers;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Repositories;

// Asumiendo una interfaz IProductRepository en UtmMarket.Core.Interfaces
public class ProductRepository // : IProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    /// <summary>
    /// Obtiene un producto por su ID de la base de datos y lo mapea al dominio.
    /// </summary>
    /// <param name="productId">El ID del producto.</param>
    /// <returns>El producto de dominio, o null si no se encuentra.</returns>
    public async Task<Product?> GetByIdAsync(int productId)
    {
        const string sql = "SELECT * FROM Productos WHERE ProductoID = @ProductId";
        var productoEntity = await _dbConnection.QueryFirstOrDefaultAsync<ProductoEntity>(sql, new { ProductId = productId });
        return productoEntity.ToDomain(); // Uso del método de extensión para mapear a dominio
    }

    /// <summary>
    /// Añade un nuevo producto a la base de datos desde una entidad de dominio.
    /// </summary>
    /// <param name="product">El producto de dominio a añadir.</param>
    public async Task AddAsync(Product product)
    {
        // El mapeo ocurre aquí, transformando el Product de dominio en un ProductoEntity
        var productoEntity = product.ToEntity(); // Uso del método de extensión para mapear a entidad

        if (productoEntity is null)
        {
            throw new ArgumentNullException(nameof(product), "Cannot add a null product.");
        }

        const string sql = "INSERT INTO Productos (ProductoID, Nombre, SKU, Marca, Precio, Stock) VALUES (@ProductoID, @Nombre, @SKU, @Marca, @Precio, @Stock)";
        await _dbConnection.ExecuteAsync(sql, productoEntity);
    }
}
```

### 4. Justificación del Uso de 'Extension Blocks' (C# 14)

El uso de 'extension blocks' (también conocidos como 'extension methods') en C# 14, tal como se implementa en `ProductMapper.cs`, ofrece mejoras significativas en rendimiento y legibilidad para el proyecto "utmmarket", especialmente en el contexto de Clean Architecture y Native AOT:

*   **Rendimiento y Compatibilidad AOT:**
    *   **Cero Asignaciones de Objeto Mapper:** Al ser métodos estáticos, no se requiere instanciar una clase `ProductMapper`. Esto elimina la sobrecarga de asignación de memoria y recolección de basura asociada a la creación de objetos para cada operación de mapeo, lo cual es crucial para aplicaciones de alto rendimiento y para la eficiencia del 'Trimming' de Native AOT.
    *   **Evita Reflexión:** La implementación utiliza asignaciones directas de propiedades y constructores primarios, evitando completamente la reflexión. Esto es un requisito fundamental para la compatibilidad con Native AOT, ya que la reflexión es incompatible con la compilación anticipada debido a su naturaleza dinámica en tiempo de ejecución. El compilador AOT puede optimizar el código de mapeo de manera agresiva.
    *   **Menor Overhead de Llamada:** Las llamadas a métodos de extensión se resuelven en tiempo de compilación a llamadas estáticas directas, lo que introduce un overhead mínimo en comparación con interfaces o invocaciones de delegados.

*   **Legibilidad y Ergonomía del Código:**
    *   **Sintaxis Fluida y Natural:** Permite una sintaxis de llamada "fluida", como `productoEntity.ToDomain()`, que es muy intuitiva y mejora la legibilidad del código en los repositorios o casos de uso. El mapeo se "siente" como una capacidad inherente del objeto que se está mapeando.
    *   **Separación de Preocupaciones:** Aunque los métodos son estáticos, el concepto de extensión permite mantener la lógica de mapeo separada de las entidades mismas, adhiriéndose a los principios de Clean Architecture. Las entidades de dominio y persistencia permanecen puras, y el mapeo se encapsula en una capa de infraestructura dedicada.
    *   **Descubribilidad:** Con IDEs modernos, los métodos de extensión son fácilmente descubribles a través de IntelliSense, lo que facilita a los desarrolladores encontrar y utilizar las funciones de mapeo disponibles.

En resumen, los 'extension blocks' proporcionan una solución de mapeo robusta, eficiente y elegante que se alinea perfectamente con los objetivos de rendimiento, mantenibilidad y la arquitectura limpia de "utmmarket", aprovechando al máximo las capacidades de C# 14 y .NET 10.
