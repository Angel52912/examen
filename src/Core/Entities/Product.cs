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
