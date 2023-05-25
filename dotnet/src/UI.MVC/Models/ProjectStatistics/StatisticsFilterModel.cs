using System.ComponentModel.DataAnnotations;
using UI.MVC.Attributes;

namespace UI.MVC.Models.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// Filter model to retrieve the project statistics.
/// </summary>
public class StatisticsFilterModel
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of detail in the x-axis. (the amount of points on the x-axis).
    /// </summary>
    [Range(1, 100)]
    public int Detail { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// A begin date for the x-axis.
    /// </summary>
    public DateTime? BeginDate { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Whether to use the begin date or not.
    /// </summary>
    public bool UseBeginDate { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// An end date for the x-axis.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Whether to use the end date or not.
    /// </summary>
    public bool UseEndDate { get; set; }
    
    // Constructor.
    public StatisticsFilterModel()
    {
    }
}