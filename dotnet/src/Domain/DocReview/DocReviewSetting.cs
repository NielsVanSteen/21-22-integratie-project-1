using System.ComponentModel.DataAnnotations;

namespace Domain.DocReview;

public class DocReviewSetting
{
    //Properties
    
    /// <summary>
    /// A setting that enables or disables the ability to comment on a docreview
    /// </summary>
    [Required]
    public bool IsCommentingAllowed { get; set; }
    
    /// <summary>
    /// A setting that enables or disables the ability to comment on a comment
    /// </summary>
    [Required]
    public bool IsSubCommentingAllowed { get; set; }
    
    /// <summary>
    /// A setting that enables or disables the ability to comment an emoji
    /// </summary>
    [Required]
    public bool AreEmojisAllowed { get; set; }
    
    /// <summary>
    /// See if commenting is still allowed
    /// </summary>
    [Required]
    public bool IsClosedForComments { get; set; }
    
    /// <summary>
    /// A setting to see if a user is required to log in to view the contents of a docreview
    /// </summary>
    [Required]
    public bool IsLogInRequired { get; set; }    
    
    /// <summary>
    /// A setting to determine if a comment placed by the user is required to be reviewed by a projectmanager or admin before being displayed
    /// When set to false, the comment will be hidden until reviewed by a projectmanager or admin
    /// </summary>
    [Required]
    public bool IsPostModerated { get; set; }

    //Constructor
    public DocReviewSetting()
    {
        
    }
}