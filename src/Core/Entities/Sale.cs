using System.Collections.ObjectModel;

namespace UtmMarket.Core.Entities;

public class Sale(int saleId, string folio, SaleStatus status, int? customerId = null, string? customerName = null)
{
    public int SaleID { get; init; } = saleId;
    public string Folio { get; init; } = folio;
    public DateTime SaleDate { get; init; } = DateTime.Now; // Auto-initialized
    public SaleStatus Status { get; set; } = status;
    public int? CustomerID { get; init; } = customerId;
    public string? CustomerName { get; set; } = customerName;

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
