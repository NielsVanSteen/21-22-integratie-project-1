using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.Util;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.DocReview;

public class UserCommentsFilterModel
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
    public int PageNumber { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The number of comments per page.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, PaginationNavigationModel.MaxSize);
    }
    private int _pageSize;
    
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

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// when filter criteria is changed, the page number should be reset to 1.
    /// </summary>
    public bool HasFilterChanged { get; set; }

    // Constructor.
    public UserCommentsFilterModel()
    {
    }

    // Methods.
    
    
    
    /// <author> Sander Verheyen</author>
    /// <summary>
    /// Converts the filter model to a filter object.
    /// </summary>
    /// <returns></returns>
    public UserCommentsFilter ToCommentsFilterModel()
    {
        return new UserCommentsFilter
        {
            DocReviewId = DocReviewId,
            OwnComments = OwnComments,
            CloseComments = CloseComments,
            UserId = UserId,
            PageNumber = PageNumber,
            PageSize = PageSize,
            SortOrder = SortOrder,
            SortOn = SortOn
        };
    } // ToCommentsFilterModel.
}