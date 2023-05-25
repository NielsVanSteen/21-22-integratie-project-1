using System.ComponentModel.DataAnnotations;

namespace Domain.DocReview;
using User;

/// <summary>
/// This class keeps track of the changes of a <see cref="DocReview"/> over time.
/// </summary>
public class DocReviewHistory
{
    //Properties
    
    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int DocReviewHistoryId { get; set; }
   
    /// <summary>
    /// User who changed the <see cref="Domain.DocReview.DocReviewStatus"/>
    /// </summary>
    public User Editor { get; set; }
    
    /// <summary>
    /// <see cref="Editor"/>
    /// </summary>
    public string EditorId { get; set; }
    
    /// <summary>
    /// The date the <see cref="DocReview"/> was changed.
    /// </summary>
    [Required]
    public DateTime EditedOn { get; set; }

    /// <summary>
    /// The DocReview.
    /// </summary>
    public DocReview DocReview { get; set; }

    /// <summary>
    /// <see cref="DocReview"/>
    /// </summary>
    public int DocReviewId { get; set; }
    
    /// <summary>
    /// The <see cref="Domain.DocReview.DocReviewStatus"/> the <see cref="DocReview"/> changed to.
    /// </summary>
    [Required]
    public DocReviewStatus DocReviewStatus { get; set; }

    //Constructor
    public DocReviewHistory()
    {
    }
}