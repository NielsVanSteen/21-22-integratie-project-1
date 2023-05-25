using Domain.Project;

namespace DAL.Repositories.Project;

/// <summary>
/// The repository interface for the footer logo's.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.Project.Project"/>
/// <see cref="Domain.Project.ProjectStyling"/>
/// </example>
public interface IProjectFooterLogoRepository
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Get all the project footer logo's by project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public IEnumerable<FooterLogo> ReadFooterLogosByProject(Domain.Project.Project project);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Get a single footer logo given the id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeProject">Whether or not to include the navigation property <see cref="Domain.Project.FooterLogo.Project"/></param>
    /// <returns></returns>
    public FooterLogo ReadFooterLogo(int id, bool includeProject = false);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Creates a new footer logo.
    /// </summary>
    /// <param name="footerLogo"></param>
    /// <returns></returns>
    public FooterLogo CreateFooterLogo(FooterLogo footerLogo);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Delete a footer logo.
    /// </summary>
    /// <param name="footerLogo"></param>
    /// <returns></returns>
    public bool DeleteFooterLogo(int id);
}