using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Attributes;

/// <author> Niels Van Steen </author>
/// <summary>
/// Checks whether a given date is before the current date.
/// </summary>
public class DateBeforeCurrentDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime d = Convert.ToDateTime(value);
        return d >= DateTime.Now;
    } // IsValid.
}