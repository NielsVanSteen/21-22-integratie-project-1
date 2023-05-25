using System.ComponentModel.DataAnnotations;
using Domain.Util;

namespace Domain.Comment;

/// <author> Niels Van Steen</author>
/// <summary>
/// Used to supply all the filter criteria for the comments.
/// </summary>
public class CommentsFilterModel
{
    // Properties.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Optional search parameter for the comments, searches the user's first name, last name and the comment text.
    /// </summary>
    [Display(Name = "Search Text")]
    public string SearchText { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// All the selected comment statuses the comment should have at least one of are stored in this list.
    /// </summary>
    public IEnumerable<CommentStatus> CommentStatus { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// List of all the selected doc-reviews the comment belong to at least one of are stored in this list.
    /// </summary>
    public IEnumerable<int> DocReviews { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// All the selected project tags the comment should have at least one of are stored in this list.
    /// </summary>
    public IEnumerable<int> ProjectTags { get; set; }

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
    public CommentsFilterModel()
    {
    }
}