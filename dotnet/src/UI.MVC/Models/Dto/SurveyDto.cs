using System.ComponentModel.DataAnnotations;
using Domain.DocReview;

namespace UI.MVC.Models.Dto;

/// <author> Michiel Verschueren </author>
/// <summary>
/// DTO to create a survey via webapi
/// </summary>
public class SurveyDto
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Id of the <see cref="DocReview"/> of this survey
    /// </summary>
    public int DocReviewId { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Title of the survey
    /// <see cref="Survey.Title"/>
    /// </summary>
    [Required]
    public string Title { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// A short text of information of the survey
    /// <see cref="Survey.Description"/>
    /// </summary>
    [Required]
    public string Description { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Can the user only select 1 option or multiple options.
    /// <see cref="Survey.AreMultipleOptionsAllowed"/>
    /// </summary>
    [Required]
    public bool AreMultipleOptionsAllowed { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The <see cref="SurveyOption"/> the user can choose between.
    /// <see cref="Survey.SurveyOptions"/>
    /// </summary>
    public Dictionary<string, string> Options { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Html of the text of this survey;
    /// </summary>
    public string Quote { get; set; }

    
}