using System.ComponentModel.DataAnnotations;

namespace Domain.DocReview;

public class Emoji
{
    // Properties.
    
    /// <summary>
    /// The unique id.
    /// </summary>
    [Key]
    public int EmojiId { get; set; }

    /// <summary>
    /// Emoji HTML code.
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// <see cref="DocReview"/> the <see cref="Emoji"/> belongs to.
    /// </summary>
    public DocReview DocReview { get; set; }
    
    /// <summary>
    /// <see cref="DocReview"/>.
    /// </summary>
    public int? DocReviewId { get; set; }

    // Constructor.
    
    public Emoji() {}

}