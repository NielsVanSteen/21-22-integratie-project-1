using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;

namespace UI.MVC.Models.Dto;

/// <author>Bjorn Straetemans</author>
/// <summary>
/// The Data Transfer object for <see cref="ReactionGroup"/>
/// </summary>
public class ViewCommentDto
{
    // Properties.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// <see cref="ReactionGroup.CommentId"/>.
    /// </summary>
    public int CommentId { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Text the user commented.
    /// </summary>
    [Required]
    public string CommentText { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of: <see cref="Reaction.PlacedOnReactionGroupOnComment"/>.
    /// </summary>
    public int? PlacedOnReactionId { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of: <see cref="ReactionGroup.User"/>.
    /// </summary>
    public string UserId { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of: <see cref="ReactionGroup.DocReview"/>.
    /// </summary>
    public int DocReviewId { get; set; }

    public int? NewTagId { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The quote of: <see cref="ReactionGroup"/>.
    /// </summary>
    public string Quote { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The profile picture of: <see cref="User"/>.
    /// </summary>
    public string ProfilePicture { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The username of: <see cref="User"/>.
    /// </summary>
    public string UserName { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The date of: <see cref="ReactionGroup"/>.
    /// </summary>
    public string date { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The tuple of: <see cref="Emoji"/>.
    /// </summary>
    public Dictionary<string, int> Emojis { get; set; }
    
    
    // Constructors.
    public ViewCommentDto()
    {
    }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Copy Constructor. Takes an object of <see cref="ReactionGroup"/> and maps it to it's DTO variant.
    /// </summary>
    /// <param name="reactionGroup"></param>
    /// <param name="emojis"></param>
    public ViewCommentDto(ReactionGroup reactionGroup, Dictionary<string, int> emojis)
    {
        if (reactionGroup == null)
            return;

        CommentId = reactionGroup.CommentId;
        CommentText = reactionGroup.CommentText;
        PlacedOnReactionId = reactionGroup.PlacedOnReactionGroup?.CommentId ?? 0;
        UserId = reactionGroup.User?.Id;
        DocReviewId = reactionGroup.DocReview?.DocReviewId ?? 0;
        Quote = reactionGroup.GetQuote();
        ProfilePicture = reactionGroup.User.GetUserProfilePictureImageLink(SquareImageSize.SM);
        UserName = reactionGroup.User.GetFullName();
        date = reactionGroup.GetFirstHistory().EditedOn.GetPostedOn(FormatExtensions.Language.English);
        if (emojis != null)
        {
            Emojis = new Dictionary<string, int>(emojis);  
        }
    } // CommentDto.
}