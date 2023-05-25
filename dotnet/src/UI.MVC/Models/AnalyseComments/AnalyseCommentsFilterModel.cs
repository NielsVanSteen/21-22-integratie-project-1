using System.ComponentModel.DataAnnotations;
using Domain.Comment;
using Domain.Util;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.AnalyseComments;

/// <author> Niels Van Steen</author>
/// <summary>
/// Filter model for the analyse comments view.
/// </summary>
public class AnalyseCommentsFilterModel
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
    public IEnumerable<string> CommentStatus { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// List of all the selected doc-reviews the comment belong to at least one of are stored in this list.
    /// </summary>
    public IEnumerable<string> DocReviews { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// All the selected project tags the comment should have at least one of are stored in this list.
    /// </summary>
    public IEnumerable<string> ProjectTags { get; set; }

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
    public AnalyseCommentsFilterModel()
    {
    }

    // Methods.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Parses the comment status list to a list of comment status objects.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CommentStatus> ParseCommentStatus()
    {
        if (CommentStatus == null || !CommentStatus.Any())
            return new List<CommentStatus>();

        if (CommentStatus.Any(c => c.ToLower() == "all"))
            return null;

        List<CommentStatus> commentStatuses = new List<CommentStatus>();
        foreach (string commentStatus in CommentStatus)
        {
            commentStatuses.Add((CommentStatus) Enum.Parse(typeof(CommentStatus), commentStatus));
        }

        return commentStatuses;
    } // ParseCommentStatus.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Parse the doc-review list to a list of doc-review ids.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> ParseDocReviews()
    {
        if (DocReviews == null || !DocReviews.Any())
            return new List<int>();

        if (DocReviews.Any(c => c.ToLower() == "all"))
            return null;

        var docReviews = new List<int>();
        foreach (var docReview in DocReviews)
        {
            docReviews.Add(int.Parse(docReview));
        }

        return docReviews;
    } // ParseDocReviews.

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Parse the project tag list to a list of project tag ids.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<int> ParseProjectTags()
    {
        if (ProjectTags == null || !ProjectTags.Any())
            return new List<int>();

        if (ProjectTags.Any(c => c.ToLower() == "all"))
            return null;

        var projectTags = new List<int>();
        foreach (var projectTag in ProjectTags)
        {
            projectTags.Add(int.Parse(projectTag));
        }

        return projectTags;
    } // ParseProjectTags.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Converts the filter model to a filter object.
    /// </summary>
    /// <returns></returns>
    public CommentsFilterModel ToCommentsFilterModel()
    {
        return new CommentsFilterModel
        {
            SearchText = SearchText,
            CommentStatus = ParseCommentStatus(),
            DocReviews = ParseDocReviews(),
            ProjectTags = ParseProjectTags(),
            PageNumber = PageNumber,
            PageSize = PageSize,
            SortOrder = SortOrder,
            SortOn = SortOn
        };
    } // ToCommentsFilterModel.
}