using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.DocReview;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;

namespace UI.MVC.Models.Android;

public class CommentAndroidDto
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
    /// The selected Text the user commented on.
    /// </summary>
    public string SelectedText { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of: <see cref ="Reaction.PlacedOnReactionGroupId"/>.
    /// </summary>
    public int? PlacedOnCommentId { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id of: <see cref="ReactionGroup.User"/>.
    /// </summary>
    public string UserName { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The photo of: <see cref="ReactionGroup.User"/>.
    /// </summary>
    public string UserPhoto { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The dictionary of the reacted <see cref="Emoji"/>.
    /// </summary>
    public Dictionary<string, int> EmojiReactions{ get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The status of the <see cref="ReactionGroup"/>.
    /// </summary>
    public string Status{ get; set; }

    public CommentAndroidDto(){}
    
    public CommentAndroidDto(ReactionGroup reactionGroup, Dictionary<string, int> emojis = null)
    {
        //Check if the main comment has emojis
        if (emojis != null)
        {
          EmojiReactions = new Dictionary<string, int>(emojis);  
        }
        
        CommentId = reactionGroup.CommentId;
        CommentText = reactionGroup.CommentText;
        SelectedText = reactionGroup.GetQuote();
        PlacedOnCommentId = reactionGroup.PlacedOnReactionGroupId;
        UserName = reactionGroup.User.GetFullName();
        UserPhoto = reactionGroup.User.GetUserProfilePictureImageLink(SquareImageSize.SM);
        Status = reactionGroup.CommentHistories.OrderBy(ch => ch.EditedOn).Last().CommentStatus.ToString();

    }
}