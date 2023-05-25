using Domain.DocReview;

namespace Domain.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// Is part of <see cref="SurveyStatistic"/>.
/// Contains 1 survey option with the amount of times it has been chosen on the survey.
/// </summary>
public class OptionAmount
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The survey option.
    /// </summary>
    public SurveyOption SurveyOption { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The amount of times it was chosen on the survey.
    /// </summary>
    public int Amount { get; set; }
    
    // Constructor.
    public OptionAmount()
    {
    }
}