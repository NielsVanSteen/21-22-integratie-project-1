using Domain.DocReview;

namespace Domain.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// Class showing a survey statistic.
///
/// Contains a survey and a list containing each option and the amount of times that option has been chosen on the survey.
/// </summary>
public class SurveyStatistic
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The survey of the statistics.
    /// </summary>
    public Survey Survey { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// A list with a record for each survey option, contains the option and the amount of times that option has been chosen on the survey.
    /// </summary>
    public IEnumerable<OptionAmount> OptionsAmount { get; set; }
    
    // Constructor.
    public SurveyStatistic()
    {
    }
}