namespace UtmMarket.Infrastructure.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "ConnectionStrings";

    public string DefaultConnection
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Connection string cannot be null or whitespace.", nameof(value));
            }
            field = value;
        }
    } = string.Empty;
}