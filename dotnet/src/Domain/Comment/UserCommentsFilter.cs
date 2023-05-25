using Domain.Util;

namespace Domain.Comment;

public class UserCommentsFilter
{
    // Properties.
    
    /// <author> Sander Verheyen </author>
    /// <summary>
    /// For which doc-review is this filter.
    /// </summary>
    public int DocReviewId { get; set; }
     
    /// <author> Sander Verheyen </author>
    /// <summary>
    /// If only the user's comments should be shown. 
    /// </summary>
    public bool OwnComments { get; set; }
     
    /// <author> Sander Verheyen </author>
    /// <summary>
    /// See if the comments where loaded
    /// </summary>
    public bool CloseComments { get; set; }
     
    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Which user applied the filters.
    /// </summary>
    public string UserId { get; set; }
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The current page of the pagination.
    /// </summary>
    public int? PageNumber { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The number of comments per page.
    /// </summary>
    public int? PageSize {get; set;}

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// How should the comments be sorted.
    /// </summary>
    public SortOrder SortOrder { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// On what property should the comments be sorted.
    /// </summary>
    public SortOn SortOn { get; set; }
    
    // Constructors.
    public UserCommentsFilter()
    {
    }
}