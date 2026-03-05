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
