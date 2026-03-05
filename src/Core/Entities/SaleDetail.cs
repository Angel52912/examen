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
