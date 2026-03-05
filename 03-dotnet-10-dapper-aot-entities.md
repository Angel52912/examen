# .NET 10 Data Entities for Native AOT with C# 14

Este documento detalla el diseño y la implementación de las clases de entidad de base de datos para el proyecto UTM Market, con un enfoque en la compatibilidad total con Native AOT (Ahead-of-Time) en .NET 10 y C# 14. Las entidades replican directamente el esquema DDL de SQL Server 2022 y están diseñadas como POCOs simples para permitir un _trimming_ agresivo y evitar la reflexión en tiempo de ejecución.

## 1. Código Fuente Completo de las Entidades de Datos

Las siguientes clases representan el mapeo directo de las tablas de la base de datos, incluyendo validaciones de negocio básicas en sus _setters_ utilizando la palabra clave `field` de C# 14.

### `ProductoEntity.cs`

Clase que modela la tabla `dbo.Producto`.

```csharp
namespace UtmMarket.Infrastructure.Models.Data;

public partial class ProductoEntity(int productoId, string nombre, string sku, string? marca, decimal precio, int stock)
{
    public int ProductoID { get; init; } = productoId;
    public string Nombre { get; init; } = nombre;
    public string SKU { get; init; } = sku;
    public string? Marca { get; init; } = marca;

    public decimal Precio
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Precio), "Price cannot be negative.");
            }
            field = value;
        }
    }

    public int Stock
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Stock), "Stock cannot be negative.");
            }
            field = value;
        }
    }
}
```

### `VentaEntity.cs`

Clase que modela la tabla `dbo.Venta`.

```csharp
namespace UtmMarket.Infrastructure.Models.Data;

public partial class VentaEntity(int ventaId, string folio, DateTime fechaVenta, int totalArticulos, decimal totalVenta, byte estatus)
{
    public int VentaID { get; init; } = ventaId;
    public string Folio { get; init; } = folio;
    public DateTime FechaVenta { get; init; } = fechaVenta;
    public int TotalArticulos { get; init; } = totalArticulos;
    public decimal TotalVenta { get; init; } = totalVenta;

    public byte Estatus
    {
        get => field;
        init
        {
            if (value is not (1 or 2 or 3)) // Using C# 11 pattern matching for 'is not'
            {
                throw new ArgumentOutOfRangeException(nameof(Estatus), "Status must be 1 (Pending), 2 (Completed), or 3 (Cancelled).");
            }
            field = value;
        }
    }
}
```

### `DetalleVentaEntity.cs`

Clase que modela la tabla `dbo.DetalleVenta`.

```csharp
namespace UtmMarket.Infrastructure.Models.Data;

public partial class DetalleVentaEntity(int detalleId, int ventaId, int productoId, decimal precioUnitario, int cantidad, decimal totalDetalle)
{
    public int DetalleID { get; init; } = detalleId;
    public int VentaID { get; init; } = ventaId;
    public int ProductoID { get; init; } = productoId;

    public decimal PrecioUnitario
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(PrecioUnitario), "Unit price cannot be negative.");
            }
            field = value;
        }
    }

    public int Cantidad
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Cantidad), "Quantity cannot be negative.");
            }
            field = value;
        }
    }

    public decimal TotalDetalle
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TotalDetalle), "Total detail cannot be negative.");
            }
            field = value;
        }
    }
}
```

## 2. Nota Técnica: Rendimiento del Mapeo Manual vs. ORMs Dinámicos en .NET 10 (Native AOT)

En el contexto de .NET 10 y Native AOT, la elección de cómo se realiza el mapeo de datos entre las bases de datos y los objetos de la aplicación tiene un impacto crítico en el rendimiento y la compatibilidad.

### Desafíos de ORMs Dinámicos (ej., Dapper, Entity Framework con reflexión)

Los ORMs tradicionales como Dapper (en su uso común) o Entity Framework Core utilizan extensivamente la **reflexión en tiempo de ejecución** para descubrir propiedades de objetos, construir mapeos y generar código SQL dinámicamente. Si bien esto ofrece gran flexibilidad y reduce el _boilerplate_ en entornos JIT (_Just-In-Time_), presenta serios inconvenientes con Native AOT:

1.  **Incompatibilidad con _Trimming_:** El compilador Native AOT realiza un _trimming_ agresivo para eliminar código no utilizado y reducir el tamaño del binario. Si el código depende de reflexión para acceder a miembros que no son referenciados explícitamente, el _trimmer_ puede eliminarlos por error, provocando fallos en tiempo de ejecución (`MissingMethodException`, `MissingFieldException`).
2.  **Rendimiento en Inicio:** Aunque Dapper es "micro" y rápido, la primera vez que mapea un tipo, aún incurre en un costo de inicialización debido a la generación dinámica de código IL o la reflexión. En aplicaciones CLI o funciones serverless donde el tiempo de inicio es crítico, esto es una penalización.
3.  **Tamaño del Binario:** La inclusión de metadatos de reflexión y el código necesario para procesarlos aumenta el tamaño final del binario nativo, contraviniendo uno de los principales objetivos de AOT.

### Ventajas del Mapeo Manual (SqlDataReader) para Native AOT

El mapeo manual directo utilizando `SqlDataReader` (o un enfoque similar con ADO.NET puro) ofrece ventajas significativas cuando la compatibilidad con Native AOT y el rendimiento son primordiales:

1.  **Cero Reflexión:** El código de mapeo se escribe explícitamente, haciendo referencia directa a las propiedades de las entidades. Esto elimina cualquier necesidad de reflexión en tiempo de ejecución, lo que lo hace 100% compatible con el _trimming_ de Native AOT.
2.  **Rendimiento Máximo:** Al evitar la sobrecarga de la reflexión y la generación dinámica de código, el mapeo manual es intrínsecamente más rápido. No hay "calentamiento" inicial; el código es precompilado y ejecutado directamente como código máquina.
3.  **Binarios Más Pequeños:** Al no requerir los metadatos de reflexión y el _runtime_ JIT asociado, el binario nativo resultante es considerablemente más pequeño.
4.  **Control Total:** Ofrece un control granular sobre cómo se leen los datos y se asignan a los objetos, permitiendo optimizaciones muy específicas para el caso de uso.

### Conclusión

Para proyectos en .NET 10 que apuntan a Native AOT, especialmente aplicaciones CLI o microservicios donde el tiempo de inicio y el consumo de memoria son críticos, el mapeo manual de datos o el uso de bibliotecas de mapeo que generen código fuente en tiempo de compilación (Source Generators) son superiores a los ORMs dinámicos. Esto asegura que la aplicación se beneficie plenamente de las optimizaciones de AOT, resultando en binarios más pequeños, inicios más rápidos y un rendimiento máximo.

El presente diseño de entidades `*Entity` está pensado para ser utilizado con un mapeo manual o _source-generated_ que lea directamente de un `SqlDataReader`, garantizando la máxima eficiencia y compatibilidad con el ecosistema Native AOT de .NET 10.
