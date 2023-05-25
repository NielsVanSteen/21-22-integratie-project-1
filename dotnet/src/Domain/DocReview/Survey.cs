using System.ComponentModel.DataAnnotations;

namespace Domain.DocReview;

public class Survey
{
    //Properties

    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int SurveyId { get; set; }

    /// <summary>
    /// Title of the survey
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// The <see cref="SurveyOption"/> the user can choose between.
    /// </summary>
    [Required]
    public ICollection<SurveyOption> SurveyOptions { get; set; }

    /// <summary>
    /// Can the user only select 1 option or multiple options.
    /// </summary>
    [Required]
    public bool AreMultipleOptionsAllowed { get; set; }

    /// <summary>
    /// A short text of information of the survey
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// This keeps track of the starting characters id
    /// </summary>
    [Required]
    public int BeginChar { get; set; }
    /// <summary>
    /// This keeps track of the final characters id
    /// </summary>
    [Required]
    public int EndChar { get; set; }

    /// <summary>
    /// The doc-review the survey belongs to.
    /// </summary>
    public DocReview DocReview { get; set; }

    /// <summary>
    /// <see cref="DocReview"/>
    /// </summary>
    public int DocReviewId { get; set; }

    //Constructor
    public Survey()
    {
    }
}