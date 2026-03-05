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
