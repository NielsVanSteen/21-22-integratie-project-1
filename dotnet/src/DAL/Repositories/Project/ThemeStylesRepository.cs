using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <author> Niels Van Steen </author>
/// <summary>
///  <see cref="IThemeStylesRepository"/>
/// </summary>
public class ThemeStylesRepository : Repository, IThemeStylesRepository
{
    // Constructor.
    public ThemeStylesRepository(DocReviewDbContext context) : base(context)
    {
    }

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.ReadThemeStyles"/>
    /// </summary>
    public ThemeStyles ReadThemeStyles(int id)
    {
        return Context.ThemeStyles.SingleOrDefault(t => t.ThemeStylesId == id);
    } // ReadThemeStyles.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.ReadAllThemeStyles"/>
    /// </summary>
    public IEnumerable<ThemeStyles> ReadAllThemeStyles()
    {
        return Context.ThemeStyles;
    } // ReadAllThemeStyles.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Return all the theme styles of a project.
    /// </summary>
    public IEnumerable<ThemeStyles> ReadThemeStylesByProject(Domain.Project.Project project)
    {
        return Context.ThemeStyles
            .Include(t => t.Project)
            .Where(t => t.Project == project);
    } // ReadThemeStylesByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.ReadProjectStylingById"/>
    /// </summary>
    public ProjectStyling ReadProjectStylingById(int id, bool includeThemeStyle = false)
    {
        var stylings = Context.ProjectStylings.AsQueryable();

        if (includeThemeStyle)
            stylings = stylings.Include(s => s.ThemeStyle);

        return stylings.SingleOrDefault(s => s.ProjectStylingId == id);
    } // ReadProjectStylingById.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.UpdateProjectStyling"/>
    /// </summary>
    public ProjectStyling UpdateProjectStyling(ProjectStyling projectStyling)
    {
        var tmp = Context.ProjectStylings.SingleOrDefault(d => d.ProjectStylingId == projectStyling.ProjectStylingId);
        if (tmp == null)
            return null;

        Context.Entry(tmp).CurrentValues.SetValues(projectStyling);
        Context.SaveChanges();
        return projectStyling;
    } // UpdateProjectStyling.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.CreateThemeStyles"/>
    /// </summary>
    public ThemeStyles CreateThemeStyles(ThemeStyles themeStyles)
    {
        Context.ThemeStyles.Add(themeStyles);
        Context.SaveChanges();
        return themeStyles;
    } // CreateThemeStyles.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesRepository.DeleteThemeStyles"/>
    /// </summary>
    public bool DeleteThemeStyles(int id)
    {
        var tmp = Context.ThemeStyles.SingleOrDefault(t => t.ThemeStylesId == id);
        if (tmp == null)
            return false;
            
        Context.ThemeStyles.Remove(tmp);
        return true;
    } // DeleteThemeStyles.
}