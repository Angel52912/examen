# .NET 10 Domain Entities Architecture with C# 14

Este documento detalla el diseño y la implementación de las Entidades de Dominio Puras para el proyecto UTM Market, siguiendo principios de Domain-Driven Design (DDD), Clean Architecture, y optimizaciones para Native AOT en .NET 10 con C# 14. El enfoque principal es encapsular la lógica de negocio directamente en las entidades, evitando dependencias de ORM y reflexión en tiempo de ejecución.

## 1. Definición del Enum de Estatus de Venta

Para gestionar el estado de las ventas de manera clara y controlada, se ha definido el siguiente `enum`:

```csharp
namespace UtmMarket.Core.Entities;

public enum SaleStatus : byte
{
    Pending = 1,
    Completed = 2,
    Cancelled = 3
}
```

## 2. Código Fuente Completo de las Entidades

### `Product.cs`

Esta entidad representa un producto disponible en el inventario. Incorpora validaciones de negocio en sus propiedades para asegurar la consistencia de los datos.

```csharp
namespace UtmMarket.Core.Entities;

public class Product(int productId, string name, string sku, string? brand, decimal price, int stock)
{
    public int ProductID { get; init; } = productId;
    public string Name { get; init; } = name;
    public string SKU { get; init; } = sku;
    public string? Brand { get; init; } = brand;

    public decimal Price
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
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

### `SaleDetail.cs`

Representa una línea de detalle dentro de una venta, asociando un producto y capturando el precio unitario en el momento de la venta para fines históricos. Incluye una propiedad calculada para el total de la línea.

```csharp
namespace UtmMarket.Core.Entities;

public class SaleDetail(int detailId, Product product, decimal unitPrice, int quantity)
{
    public int DetalleID { get; init; } = detailId;
    public int ProductID { get; init; } = product.ProductID; // Foreign Key to Product
    public int VentaID { get; internal set; } // Foreign Key to Sale, set by Sale entity

    public Product Product { get; init; } = product; // Association to Product

    public decimal UnitPrice
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(UnitPrice), "Unit price cannot be negative.");
            }
            field = value;
        }
    }

    public int Quantity
    {
        get => field;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity cannot be negative.");
            }
            field = value;
        }
    }

    public decimal TotalDetalle => UnitPrice * Quantity;
}
```

### `Sale.cs`

La entidad agregada "Sale" gestiona la colección de `SaleDetail` y encapsula la lógica de negocio relacionada con la venta, incluyendo el cálculo de totales y la gestión del estado.

```csharp
using System.Collections.ObjectModel;

namespace UtmMarket.Core.Entities;

public class Sale(int saleId, string folio, SaleStatus status)
{
    public int SaleID { get; init; } = saleId;
    public string Folio { get; init; } = folio;
    public DateTime SaleDate { get; init; } = DateTime.Now; // Auto-initialized
    public SaleStatus Status { get; set; } = status;

    private readonly ObservableCollection<SaleDetail> _saleDetails = new();
    public ReadOnlyObservableCollection<SaleDetail> SaleDetails => new(_saleDetails);

    public decimal TotalSale => _saleDetails.Sum(sd => sd.TotalDetalle);
    public int TotalItems => _saleDetails.Sum(sd => sd.Quantity);

    public void AddDetail(SaleDetail detail)
    {
        if (detail.VentaID != 0 && detail.VentaID != SaleID)
        {
            throw new ArgumentException("SaleDetail already belongs to another Sale or has an invalid VentaID.");
        }
        detail.VentaID = SaleID;
        _saleDetails.Add(detail);
    }
}
```

## 3. Explicación de la palabra clave `field` en Validaciones de Negocio

La introducción de la palabra clave `field` en C# 14 ha simplificado significativamente la implementación de validaciones de negocio y otras lógicas en los _accessors_ de propiedades autoimplementadas, reduciendo el _boilerplate_ y mejorando la legibilidad del código.

**Antes de `field`:** Para realizar una validación en un `setter` o `getter` de una propiedad, era necesario declarar explícitamente un campo de respaldo privado.

```csharp
private decimal _price;
public decimal Price
{
    get { return _price; }
    set
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
        }
        _price = value;
    }
}
```

**Con `field` (C# 14):** La palabra clave `field` permite referenciar el campo de respaldo implícito de una propiedad autoimplementada directamente dentro de sus `get` o `set` accessors. Esto elimina la necesidad de declarar el campo privado manualmente.

```csharp
public decimal Price
{
    get => field; // Accede al campo de respaldo implícito
    init // Usamos 'init' para inmutabilidad después de la construcción
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
        }
        field = value; // Asigna al campo de respaldo implícito
    }
}
```

**Beneficios en este diseño:**

*   **Menos _Boilerplate_:** Se elimina la necesidad de escribir y mantener campos de respaldo privados, haciendo el código más conciso.
*   **Mayor Legibilidad:** La lógica de validación reside directamente dentro de la propiedad, donde es más intuitivo buscarla, sin la distracción de un campo privado separado.
*   **Encapsulación Fuerte:** Refuerza la encapsulación de la entidad al mantener la lógica de validación co-ubicada con la definición de la propiedad, esencial para un diseño DDD.
*   **Compatibilidad AOT:** Al ser una característica puramente sintáctica del compilador de C#, no introduce ninguna dependencia de reflexión en tiempo de ejecución, lo que lo hace perfectamente compatible con las optimizaciones de Native AOT en .NET 10.

En resumen, el uso de `field` en C# 14 contribuye a la creación de entidades de dominio más limpias, seguras y eficientes, alineándose con los objetivos de Clean Architecture y la preparación para Native AOT.
