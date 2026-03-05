namespace UtmMarket.Core.Entities;

public class Customer(int customerId, string name, string email)
{
    public int CustomerID { get; init; } = customerId;
    public string Name { get; init; } = name;
    public DateTime RegistrationDate { get; init; } = DateTime.Now;

    public string Email
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            {
                throw new ArgumentException("El formato del correo electrónico es inválido.");
            }
            field = value.ToLower().Trim();
        }
    } = email; 
}
