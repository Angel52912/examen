using System;
using UtmMarket.Core.Entities; // For SaleStatus

namespace UtmMarket.Core.Filters;

/// <summary>
/// Represents the filtering criteria for searching sales.
/// </summary>
public record SaleFilter(
    int? SaleId = null,
    string? Folio = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    SaleStatus? Status = null
)
{
    /// <summary>
    /// Gets the start date for filtering sales.
    /// </summary>
    public DateTime? StartDate { get; init; } = StartDate;

    /// <summary>
    /// Gets the end date for filtering sales. Ensures EndDate is not before StartDate.
    /// </summary>
    public DateTime? EndDate
    {
        get => field;
        init
        {
            if (value.HasValue && StartDate.HasValue && value.Value < StartDate.Value)
            {
                throw new ArgumentException("EndDate cannot be before StartDate.", nameof(EndDate));
            }
            field = value;
        }
    } = EndDate; // Initialize from primary constructor parameter
}