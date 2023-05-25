namespace UI.MVC.Models.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// The timespan to return the statistics for, counting back from the current date.
/// </summary>
public enum StatisticLengthEnum : byte
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Return all statistics for the past month.
    /// </summary>
    Month = 0,
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Return all the statistics for the past year.
    /// </summary>
    Year = 1,
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Return all the statistics.
    /// </summary>
    All = 2
}