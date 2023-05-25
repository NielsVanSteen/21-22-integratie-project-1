using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.Survey;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model for the survey options amount.
/// </summary>
public class SurveyOptionsStatistics
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The option text for the <see cref="Survey"/>
    /// </summary>
    [Required]
    public string Option { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Optional description explaining the option.
    /// </summary>
    public string Description { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The amount of times that option was selected.
    /// </summary>
    public int Amount { get; set; }
}