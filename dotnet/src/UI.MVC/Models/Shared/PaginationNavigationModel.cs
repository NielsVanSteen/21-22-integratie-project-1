namespace UI.MVC.Models.Shared;

/// <author> Niels Van Steen </author>
/// <summary>
/// Model for the pagination navigation view.
/// </summary>
public class PaginationNavigationModel
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The maximum number of page items to show.
    /// </summary>
    public const int MaxSize = 25;

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The current active page in the pagination navigation.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The total number of pages in the pagination navigation.
    /// </summary>
    public int TotalPages { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The total number of items in the pagination navigation.
    /// </summary>
    public int PageSize { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The total amount of comments.
    /// </summary>
    public int TotalItems { get; set; }

    // Constructors.
    public PaginationNavigationModel()
    {
    }

    public PaginationNavigationModel(int currentPage, int totalPages, int pageSize, int totalItems)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
        TotalItems = totalItems;
    }
}