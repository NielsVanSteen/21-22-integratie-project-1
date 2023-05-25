using Domain.Project;

namespace BL.Project;

/// <author> Niels Van Steen </author>
/// <summary>
/// Repository interface for <see cref="ThemeStyles"/>
/// </summary>
public interface IThemeStylesManager
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Read a single <see cref="ThemeStyles"/> given the id.
    /// </summary>
    /// <returns></returns>
    public ThemeStyles GetThemeStyles(int id);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Read all the <see cref="ThemeStyles"/>.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ThemeStyles> GetAllThemeStyles();
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  Return all the theme styles of a project.
    /// </summary>
    public IEnumerable<ThemeStyles> GetThemeStylesByProject(Domain.Project.Project project);
    
     /// <author> Niels Van Steen </author>
    /// <summary>
    /// Reads a single project styling given the id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeThemeStyle">Whether or not to include the navigation property for <see cref="ProjectStyling.ThemeStyle"/></param>
    /// <returns></returns>
    public ProjectStyling GetProjectStylingById(int id, bool includeThemeStyle = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Updates a project styling.
    /// </summary>
    /// <param name="projectStyling"></param>
    /// <returns></returns>
    public ProjectStyling ChangeProjectStyling(ProjectStyling projectStyling);
    
     /// <author> Niels Van Steen </author>
    /// <summary>
    /// Creates a new theme styles.
    /// </summary>
    /// <param name="themeStyles"></param>
    /// <returns></returns>
    public ThemeStyles AddThemeStyles(ThemeStyles themeStyles);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Deletes a theme style.
    /// </summary>
    /// <param name="id"></param>
    public bool RemoveThemeStyles(int id);
}