namespace UI.MVC.Models.Project;

/// <author> Michiel Verschueren </author>
/// <summary>
/// viewModel for project home page
/// </summary>
public class ProjectModel
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The project for which the view is
    /// </summary>
    public Domain.Project.Project Project { get; set; }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// All the docReviews of the project
    /// </summary>
    public IEnumerable<Domain.DocReview.DocReview> DocReviews { get; set; }
}