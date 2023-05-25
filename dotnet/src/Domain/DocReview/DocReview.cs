using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.DocReview;
using Domain.Project;

namespace Domain.DocReview;

using User;

public class DocReview
{
    //Properties

    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int DocReviewId { get; set; }

    /// <summary>
    /// The project the DocReview belongs to.
    /// </summary>
    public Project.Project Project { get; set; }

    /// <summary>
    /// <see cref="Project"/>
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// All the available emoji's
    /// </summary>
    public ICollection<Emoji> AvailableEmoji { get; set; }

    /// <summary>
    /// This is the name of a  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Short text that explains what this  <see cref="DocReview"/> is about.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// This is the body/content of the  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public string DocReviewText { get; set; }

    /// <summary>
    /// Who has written the  <see cref="DocReview"/>.
    /// </summary>
    public User WrittenBy { get; set; }

    /// <summary>
    /// <see cref="WrittenBy"/>
    /// </summary>
    public string WrittenById { get; set; }

    /// <summary>
    /// A list of all the surveys that have been done for this  <see cref="DocReview"/>.
    /// </summary>
    public List<Survey> Surveys { get; set; }

    /// <summary>
    /// Which phase the <see cref="DocReview"/> is located in.
    /// </summary>
    public IEnumerable<TimeLinePhase> TimeLinePhases { get; set; }

    /// <summary>
    /// Settings of the  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public DocReviewSetting DocReviewSettings { get; set; }

    /// <summary>
    /// <see cref="DocReviewHistory"/>
    /// </summary>
    public ICollection<DocReviewHistory> DocReviewHistories { get; set; }

    //Constructor
    public DocReview()
    {
        DocReviewHistories = new List<DocReviewHistory>();
        Surveys = new List<Survey>();
        AvailableEmoji = new List<Emoji>();
    }

    public DocReview(int docReviewId, string docReviewText)
    {
        DocReviewId = docReviewId;
        DocReviewText = docReviewText;
    }
}