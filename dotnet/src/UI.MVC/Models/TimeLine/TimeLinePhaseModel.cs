using System.ComponentModel.DataAnnotations;
using UI.MVC.Attributes;

namespace UI.MVC.Models.TimeLine;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model to create/edit a timeline phase.
/// </summary>
public class TimeLinePhaseModel : IValidatableObject
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// id.
    /// </summary>   
    public int? TimeLinePhaseId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The name of the phase.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A description giving more information about phase. 
    /// </summary>
    public string Description { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Date marking the start of the phase.
    /// </summary>
    public DateTime? BeginDate { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Foreign key for <see cref="DocReview" /> of this phase.
    /// </summary>    
    public int? DocReviewId { get; set; }
    
    // Constructor.
    public TimeLinePhaseModel()
    {
    }

    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        var date = BeginDate;
        if(date is null)
        {
            validationResult.Add(new("The BeginDate can not be empty.", new[] {nameof(BeginDate)}));
        }
        return validationResult;
    }
}