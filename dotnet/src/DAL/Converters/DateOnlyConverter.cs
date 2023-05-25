using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Converters;

/// <author>Brian Nys</author>
/// <summary>
/// Class that converts a <see cref="DateOnly" /> to <see cref="DateTime" /> and vice versa.
/// </summary>
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    /// <summary>
    /// Creates a new instance of this converter.
    /// </summary>
    public DateOnlyConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d)
    )
    { }
}