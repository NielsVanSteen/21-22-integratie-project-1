using System.ComponentModel.DataAnnotations;
using BL.DocReview;
using Domain.DocReview;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.MVC.Attributes;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.DocReview;

/// <author> Michiel Verschueren </author>
/// <summary>
/// Model to create a new <see cref="DocReview"/>
/// </summary>
public class WriteDocReviewModel
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The name to be used for the new DocReview
    /// <seealso cref="DocReview.Name"/>
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The Description to be used for the new DocReview
    /// this will be a html string provide by ckeditor
    /// <seealso cref="DocReview.Description"/>
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The Content to be used for the new DocReview
    /// this will be a html string provide by ckeditor
    /// <seealso cref="DocReview.DocReviewText"/>
    /// </summary>
    [Required]
    [Display(Name = "Content")]
    public string DocReviewText { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Setting to (dis)allow commenting on the new DocReview
    /// <seealso cref="DocReviewSetting.IsCommentingAllowed"/>
    /// </summary>
    [Display(Name = "Allow comments")]
    public bool IsCommentingAllowed { get; set; }
   
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Setting to (dis)allow Subcommenting on the new DocReview
    /// <seealso cref="DocReviewSetting.IsSubCommentingAllowed"/>
    /// </summary>
    [Display(Name = "Allow sub-comments")]
    public bool IsSubCommentingAllowed { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Setting to (dis)allow Emoji's on comments on the new DocReview
    /// <seealso cref="DocReviewSetting.AreEmojisAllowed"/>
    /// </summary>
    [Display(Name = "Allow Emoji's")]
    public bool AreEmojisAllowed { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Setting to force users to login before being able to view the docreview
    /// <seealso cref="DocReviewSetting.IsLogInRequired"/>
    /// </summary>
    [Display(Name = "Require login")]
    public bool IsLogInRequired { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Setting to enable post moderation
    /// pre-moderation: comments have to be approved by a project manager before visible to other users
    /// post-moderation: comments are immediately displayed to other users without the need for approval by a project manager
    /// <seealso cref="DocReviewSetting.IsPostModerated"/>
    /// </summary>
    [Display(Name = "Enable post moderation")]
    public bool IsPostModerated { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// All available emoji's
    /// </summary>
    public IEnumerable<Emoji> AvailableEmojis { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Emoji's selected by the user creating the DocReview to be able to be used by users on this DocReview
    /// <seealso cref="DocReview.AvailableEmoji"/>
    /// </summary>
    
    public ICollection<string> SelectedEmojiIds { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Uplaoded image to be used as the bannerimage for the <see cref="DocReview"/>
    /// </summary>
    [MaxFileSize(5)]
    [AllowedExtensions(".png", ".jpg", ".jpeg", ".svg", ".gif")]
    [Required]
    public IFormFile BannerImage { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// instance of <see cref="SelectedEmojiParser"/>
    /// </summary>
    public SelectedEmojiParser SelectedEmojiParser { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method used to parse the user selected emoji's into their HTML code
    /// </summary>
    /// <param name="emojiManager">emojiManager used for requesting the codes from de database</param>
    /// <returns>HTML Emoji codes for the emojis the user has selected</returns>
    public IEnumerable<string> ParseSelectedEmojiIds(IEmojiManager emojiManager)
    {
        SelectedEmojiParser = new SelectedEmojiParser(emojiManager);
        return SelectedEmojiParser.Parse(SelectedEmojiIds);
    }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Creates instance of <see cref="DocReview"/> created using the properties of this class
    /// </summary>
    /// <returns>newly created instance of <see cref="DocReview"/></returns>
    public Domain.DocReview.DocReview GetDocReview()
    {
        return new Domain.DocReview.DocReview()
        {
            Name = this.Name,
            Description = this.Description,
            DocReviewText = this.DocReviewText.Replace("\r\n",""),
            DocReviewSettings = new DocReviewSetting
            {
                IsCommentingAllowed = this.IsCommentingAllowed,
                IsSubCommentingAllowed = this.IsSubCommentingAllowed,
                IsLogInRequired = this.IsLogInRequired,
                IsPostModerated = this.IsPostModerated,
                AreEmojisAllowed = this.AreEmojisAllowed,
                IsClosedForComments = false
            }
        };
    }
    
}