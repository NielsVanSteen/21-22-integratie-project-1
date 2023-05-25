using System.ComponentModel.DataAnnotations;
using Domain.ProjectStatistics;
using UI.MVC.Extensions;

namespace UI.MVC.Models.Survey;

/// <author> Niels Van Steen </author>
/// <summary>
/// shows the statistics of a survey.
/// </summary>
public class SurveyStatisticsModel
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The id of the survey.
    /// </summary>
    public int SurveyId { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Title of the survey
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Can the user only select 1 option or multiple options.
    /// </summary>
    [Required]
    public bool AreMultipleOptionsAllowed { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// A short text of information of the survey
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The text the survey was about.
    /// </summary>
    [Required]
    public string SelectedText { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="SurveyOptionsStatistics"/>
    /// </summary>
    public IEnumerable<SurveyOptionsStatistics> OptionsStatistics { get; set; }

    // Constructors.
    public SurveyStatisticsModel()
    {
    }

    // Methods.
    public SurveyStatisticsModel(SurveyStatistic surveyStatistic)
    {
        SurveyId = surveyStatistic.Survey.SurveyId;
        Title = surveyStatistic.Survey.Title;
        Description = surveyStatistic.Survey.Description;
        SelectedText = surveyStatistic.Survey.DocReview.GetSelectedTextOfDocReview(surveyStatistic.Survey.BeginChar,
            surveyStatistic.Survey.EndChar, true);
        AreMultipleOptionsAllowed = surveyStatistic.Survey.AreMultipleOptionsAllowed;
        OptionsStatistics = surveyStatistic.OptionsAmount.Select(o => new SurveyOptionsStatistics()
        {
            Option = o.SurveyOption.Option,
            Amount = o.Amount,
            Description = o.SurveyOption.Description
        });
    }
}