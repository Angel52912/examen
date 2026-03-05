## Contrato de Repositorio para Productos (IProductRepository.cs)

### 1. Estructura de Namespaces Recomendada

\`\`\`
UtmMarket.Core
├── Entities
│   └── Product.cs
├── Filters
│   └── ProductFilter.cs
└── Repositories
    └── IProductRepository.cs  <-- Interfaz Creada
\`\`\`

### 2. Código Fuente Completo de 'IProductRepository.cs'

\`\`\`csharp
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
\`\`\`

### 3. Definición de 'ProductFilter'

\`\`\`csharp
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
\`\`\`

### 4. Explicación Técnica: Por qué 'IAsyncEnumerable' es Superior para una Aplicación de Consola en .NET 10

En el contexto de una aplicación de consola en .NET 10, el uso de `IAsyncEnumerable<T>` para operaciones de consulta como `GetAllAsync` y `FindAsync` en `IProductRepository` representa una mejora significativa sobre los enfoques tradicionales que retornan `Task<List<T>>` o `Task<IEnumerable<T>>`. Las principales razones son:

*   **Streaming de Datos (Lazy Loading Asíncrono):**
    *   `IAsyncEnumerable<T>` permite un "pull-based" streaming asíncrono de datos. Esto significa que los elementos se producen y se consumen uno por uno o en pequeños lotes a medida que están disponibles, en lugar de cargar toda la colección en memoria de golpe.
    *   Para una aplicación de consola que podría necesitar procesar grandes volúmenes de productos (por ejemplo, para generar reportes, realizar migraciones o análisis de datos), esto es crucial. Evita cargar toda la base de datos en la memoria RAM de la consola, previniendo `OutOfMemoryExceptions` y reduciendo el consumo de recursos.

*   **Optimización del Uso de Memoria:**
    *   Al no materializar toda la colección en memoria, el uso de `IAsyncEnumerable<T>` reduce drásticamente la huella de memoria, lo cual es especialmente valioso en entornos con recursos limitados o al trabajar con datasets masivos. El recolector de basura puede liberar memoria de los elementos ya procesados mientras se siguen recuperando nuevos.

*   **Mejora de la Responsividad (para el Usuario o Sistema de Lotes):**
    *   Aunque es una aplicación de consola, si tiene una interfaz de usuario básica o si es parte de un proceso por lotes que reporta progreso, el streaming permite que los primeros resultados se procesen y muestren o actúen sobre ellos mucho antes de que se complete la recuperación de todos los datos. Esto puede mejorar la percepción de rendimiento y permitir acciones tempranas.

*   **Compatibilidad con Native AOT en .NET 10:**
    *   `IAsyncEnumerable<T>` es una característica fundamental del ecosistema .NET moderno y es completamente compatible con Native AOT. Su implementación se basa en características del lenguaje C# (como `yield return` en métodos asíncronos) que no dependen de reflexión o de generación de código en tiempo de ejecución, lo que lo hace ideal para la compilación anticipada.
    *   Esto asegura que las optimizaciones de Native AOT, como el menor tiempo de inicio y la menor huella de memoria del ejecutable, se apliquen eficazmente a las operaciones de acceso a datos que utilizan este patrón.

*   **Flexibilidad en el Consumo:**
    *   Se consume fácilmente usando la instrucción `await foreach`, que es muy idiomática y legible en C#. Esto permite un bucle asíncrono que espera la disponibilidad de cada elemento antes de procesarlo, sin bloquear el hilo principal.

En resumen, `IAsyncEnumerable<T>` en .NET 10 y C# 14 no es solo una característica de moda; es una herramienta poderosa para construir aplicaciones de consola eficientes, escalables y responsivas, especialmente cuando se enfrentan a grandes volúmenes de datos, y es perfectamente adecuada para el proyecto "utmmarket".