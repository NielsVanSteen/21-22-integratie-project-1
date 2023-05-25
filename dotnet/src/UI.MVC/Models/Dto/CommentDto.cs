using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.DocReview;
using Domain.User;
using UI.MVC.Extensions;

namespace UI.MVC.Models.Dto;

/// <author>Niels Van Steen</author>
/// <summary>
/// The Data Transfer object for <see cref="ReactionGroup"/>
/// </summary>
public class CommentDto
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ReactionGroup.CommentId"/>.
    /// </summary>
    public int CommentId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Text the user commented.
    /// </summary>
    [Required]
    public string CommentText { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ReactionGroup.BeginChar"/>.
    /// </summary>
    public int? BeginChar { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ReactionGroup.EndChar"/>.
    /// </summary>
    public int? EndChar { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id of: <see cref="Reaction.PlacedOnReactionGroupOnComment"/>.
    /// </summary>
    public int? PlacedOnReactionId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id of: <see cref="ReactionGroup.User"/>.
    /// </summary>
    public string UserId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id of: <see cref="ReactionGroup.DocReview"/>.
    /// </summary>
    public int DocReviewId { get; set; }
    
    public int? newTagId { get; set; }
    
    public string Quote { get; set; }
    
    // Constructors.
    public CommentDto() {}

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Copy Constructor. Takes an object of <see cref="ReactionGroup"/> and maps it to it's DTO variant.
    /// </summary>
    /// <param name="reactionGroup"></param>
    public CommentDto(ReactionGroup reactionGroup)
    {
        if (reactionGroup == null)
            return;
        
        CommentId = reactionGroup.CommentId;
        CommentText = reactionGroup.CommentText;
        BeginChar = reactionGroup.BeginChar;
        EndChar = reactionGroup.EndChar;
        PlacedOnReactionId = reactionGroup.PlacedOnReactionGroup?.CommentId ?? 0;
        UserId = reactionGroup.User?.Id;
        DocReviewId = reactionGroup.DocReview?.DocReviewId ?? 0;
        Quote = reactionGroup.GetQuote();
    } // CommentDto.
    
    /// <summary>
    /// Equals and HashCode (Used in the TracerBullet to check if a comment equals another comment).
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool Equals(CommentDto other)
    {
        return CommentText == other.CommentText;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CommentDto) obj);
    }

    public override int GetHashCode()
    {
        return (CommentText != null ? CommentText.GetHashCode() : 0);
    }
}