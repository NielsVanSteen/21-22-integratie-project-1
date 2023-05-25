using System.ComponentModel.DataAnnotations;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// A single timeline phase. Each phase can be linked to maximum one <see cref="Domain.DocReview.DocReview"/>.
/// </summary>
public class TimeLinePhase
{
    // Properties. 
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Id for timelinePhase.
    /// </summary>
    [Required]
    [Key]
    public int TimeLinePhaseId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Timeline the phase belongs to.
    /// </summary>
    public TimeLine TimeLine { get; set; }

    /// <author>Brian Nys</author>
    /// <summary>
    /// Foreign key for <see cref="TimeLine" /> of this phase.
    /// </summary>    
    public int? TimeLineId { get; set; }

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
    [Required]
    public DateOnly BeginDate { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="Domain.DocReview.DocReview"/> linked to the phase.
    /// </summary>
    public DocReview.DocReview DocReview { get; set; }

    /// <author>Brian Nys</author>
    /// <summary>
    /// Foreign key for <see cref="DocReview" /> of this phase.
    /// </summary>    
    public int? DocReviewId { get; set; }

    // Constructor.
    public TimeLinePhase() { }
}