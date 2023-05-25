using System.Globalization;

namespace UI.MVC.Extensions;

/// <author> Niels Van Steen</author>
/// <summary>
/// Extension class for <see cref="DateTime"/> used to display nicely formatted dates.
/// </summary>
public static class FormatExtensions
{
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Returns the day of the month in a string padded with a zero.
    /// </summary>
    public static string GetZeroPaddedDay(this DateTime dateTime)
    {
        return dateTime.ToString("dd");
    } // GetDayAndMonthAbbreviation.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Return the month abbreviation (1st 3 letters).
    /// </summary>
    public static string GetMonthAbbreviation(this DateTime dateTime)
    {
        var month = dateTime.ToString("MMMM")[..3];
        return month;
    } // GetDayAndMonthAbbreviation.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Return the zero padded day with the 1st 3 letters of the month.
    /// </summary>
    public static string GetDayAndMonthAbbreviation(this DateTime dateTime)
    {
        return $"{dateTime.GetZeroPaddedDay()} {dateTime.GetMonthAbbreviation()}";
    } // GetDayAndMonthAbbreviation.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Get the date in a format like, e.g., '09 Apr 2022'
    /// </summary>
    public static string GetDateWithAbbreviatedMonth(this DateTime dateTime)
    {
        return $"{dateTime.GetDayAndMonthAbbreviation()} {dateTime.Year}";
    } // GetDayAndMonthAbbreviation.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Get the time of the day in a 12 hour format with leading 0.
    /// E.g., 09:00 AM
    /// </summary>
    public static string GetTime12HourClock(this DateTime dateTime)
    {
        return dateTime.ToString("hh:mm tt");
    } // GetTime12HourClock.

    /// <summary>
    /// Return how long ago a given date was compared to the current date.
    /// E.g., '1 minute ago', '1 hour ago', '1 day ago', '2 weeks ago', '3 months ago', '4 years ago', ...
    /// </summary>
    /// <param name="dateTime">The datetime.</param>
    /// <param name="language">The language <see cref="Language"/></param>
    /// <returns></returns>
    public static string GetPostedOn(this DateTime dateTime, Language language)
    {
        var currentDate = DateTime.Now;
        var timeSpan = currentDate - dateTime;
        var ci = new CultureInfo("nl-NL");
        var dayOfWeek = ci.DateTimeFormat.DayNames[(int)DateTime.Today.DayOfWeek];
        
        // When less than an hour ago return e.g. '22 minutes ago'
        var minutes = timeSpan.TotalMinutes;
        if (minutes < 1)
            return new [] {"Moments ago", "Een paar seconde geleden"}[(int) language];
        if (minutes < 2)
            return new [] {"1 minute ago", "1 minuut geleden"}[(int) language];
        if (minutes < 60)
            return $"{(int)minutes} {new [] {" minutes ago", " minuten geleden"}[(int) language]}";

        // When less than a day ago return e.g., '1 hour ago'.
        var hours = timeSpan.TotalHours;
        if (hours < 1) 
            return new [] {"Less than an hour ago", "Minder dan 1 uur geleden"}[(int) language];
        if (hours < 24)
            return new [] {$"{(int)hours} hours ago", $"{(int)hours} uur geleden"}[(int) language];
        
        // When the days are between 1 and 7 return e.g., 'yesterday', 'wednesday', ...
        var days = timeSpan.TotalDays;
        if (days < 1)
            return new [] {"Today", "Vandaag"}[(int) language];

        if (days < 2)
            return new [] {"Yesterday", "Gisteren"}[(int) language];

        if (days < 7)
            return new [] {dateTime.DayOfWeek.ToString(), dayOfWeek}[(int) language];
        
        // When the weeks are between 1 and 4 return e.g., '1 week ago', '3 weeks ago'
        var weeks = timeSpan.TotalDays / 7;
        if (weeks < 1)
            return new [] {"Less than a week ago", "Minder dan een week geleden"}[(int) language];
        if (weeks < 4)
            return new [] {$"{(int)weeks} weeks ago", $"{(int)weeks} weken geleden"}[(int) language];
        
        // When the months are between 1 and 12 return e.g., '1 month ago', '3 months ago'
        var months = timeSpan.TotalDays / 30;
        if (months < 1)
            return new [] {"Less than a month ago", "Minder dan een maand geleden"}[(int) language];
        if (months < 12)
            return new [] {$"{(int)months} months ago", $"{(int)months} maanden geleden"}[(int) language];
        
        // When the years are between 1 and 4 return e.g., '1 year ago', '3 years ago'
        var years = timeSpan.TotalDays / 365;
        if (years < 1)
            return new [] {"Less than a year ago", "Minder dan een jaar geleden"}[(int) language];
        
        return new [] {$"{(int)years} years ago", $"{(int)years} jaar geleden"}[(int) language];
    } // GetPostedOn.

    public enum Language : byte
    {
        English = 0,
        Dutch = 1
    }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Formats numbers. E.g., 20,500.00 -> 20.5K. 13,600,000.00 -> 13.6M
    /// </summary>
    /// <param name="num">The unformatted number.</param>
    /// <returns></returns>
    public static string FormatNumber(this int num)
    {
        if (num >= 1000000000)
            return (num / 1000000000D).ToString("0.#B");

        if (num >= 100000000)
            return (num / 1000000D).ToString("0.#M");

        if (num >= 1000000)
            return (num / 1000000D).ToString("0.##M");

        if (num >= 100000)
            return (num / 1000D).ToString("0.#k");

        if (num >= 10000)
            return (num / 1000D).ToString("0.##k");

        return num.ToString("#,0");
    } // FormatNumber.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Does the same as <see cref="FormatNumber(int)"/> but fom a <see cref="decimal"/> number.
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string FormatNumber(this decimal num)
    {
        return ((int) num).FormatNumber();
    } // FormatNumber.
}