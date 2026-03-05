namespace UtmMarket.Infrastructure.Models.Data;

public partial class VentaEntity(int ventaId, string folio, DateTime fechaVenta, int totalArticulos, decimal totalVenta, byte estatus, int? clienteId = null)
{
    public int VentaID { get; init; } = ventaId;
    public string Folio { get; init; } = folio;
    public DateTime FechaVenta { get; init; } = fechaVenta;
    public int TotalArticulos { get; init; } = totalArticulos;
    public decimal TotalVenta { get; init; } = totalVenta;
    public int? ClienteID { get; init; } = clienteId;

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
