using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Comment;

/// <author>Michiel Verschueren</author>
/// <summary>
/// Class containing the <see cref="CommentStatus"/> changed of a <see cref="ReactionGroup"/>.
/// </summary>
public class CommentHistory : IValidatableObject
{
    // Properties.

    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int CommentHistoryId { get; set; }

    /// <summary>
    /// The <see cref="User"/> who edited the <see cref="CommentHistory"/>.
    /// </summary>
    public User.User EditedBy { get; set; }

    public string EditedById { get; set; }

    /// <summary>
    /// The <see cref="CommentStatus"/> of the <see cref="ReactionGroup"/>.
    /// </summary>
    [Required]
    public CommentStatus CommentStatus { get; set; }

    /// <summary>
    /// The date the <see cref="ReactionGroup"/> was edited.
    /// </summary>
    [Required]
    public DateTime EditedOn { get; set; }

    /// <summary>
    /// The <see cref="ReactionGroup"/> which was edited.
    /// </summary>
    public ReactionGroup ReactionGroup { get; set; }

    public int ReactionGroupId { get; set; }

    //Constructors
    public CommentHistory()
    {
    }


    // Tertiary validation of the Enum CommentStatus
    /// <author>Michiel Verschueren</author>
    /// <summary>
    /// This method validates the value of <see cref="CommentStatus">CommentStatus</see> by making sure the enum is defined for the value in the property
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns>The result of the validation</returns>
    IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();
        if (!Enum.IsDefined(typeof(CommentStatus), this.CommentStatus))
        {
            StringBuilder errorMessage = new StringBuilder("The CommentStatus kind must be ");
            foreach (var item in Enum.GetValues(typeof(CommentStatus)))
            {
                errorMessage.Append(item).Append(' ');
            }

            validationResults.Add(new ValidationResult(errorMessage.ToString(), new List<string> { "CommentStatus" }));
        }

        return validationResults;
    }
}