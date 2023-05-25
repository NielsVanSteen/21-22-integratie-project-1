using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Project;

namespace BL.Project;

/// <author> Niels Van Steen </author>
/// <summary>
///  <see cref="IThemeStylesManager"/>
/// </summary>
public class ThemeStylesManager : IThemeStylesManager
{
    // Fields.
    private readonly IThemeStylesRepository _repository;

    // Constructor.
    public ThemeStylesManager(IThemeStylesRepository repository)
    {
        _repository = repository;
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.GetThemeStyles"/>
    /// </summary>
    public ThemeStyles GetThemeStyles(int id)
    {
        return _repository.ReadThemeStyles(id);
    } // GetThemeStyles.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.GetAllThemeStyles"/>
    /// </summary>
    public IEnumerable<ThemeStyles> GetAllThemeStyles()
    {
        return _repository.ReadAllThemeStyles();
    } // GetAllThemeStyles.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.GetThemeStylesByProject"/>
    /// </summary>
    public IEnumerable<ThemeStyles> GetThemeStylesByProject(Domain.Project.Project project)
    {
        return _repository.ReadThemeStylesByProject(project);
    }

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.GetProjectStylingById"/>
    /// </summary>
    public ProjectStyling GetProjectStylingById(int id, bool includeThemeStyle = false)
    {
        return _repository.ReadProjectStylingById(id, includeThemeStyle);
    } // GetProjectStylingById.

    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.ChangeProjectStyling"/>
    /// </summary>
    public ProjectStyling ChangeProjectStyling(ProjectStyling projectStyling)
    {
        Validator.ValidateObject(projectStyling, new ValidationContext(projectStyling), validateAllProperties: true);
        return _repository.UpdateProjectStyling(projectStyling);
    } // ChangeProjectStyling.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.AddThemeStyles"/>
    /// </summary>
    public ThemeStyles AddThemeStyles(ThemeStyles themeStyles)
    {
        Validator.ValidateObject(themeStyles, new ValidationContext(themeStyles), validateAllProperties: true);
        return _repository.CreateThemeStyles(themeStyles);
    } // AddThemeStyles.

   /// <author> Niels Van Steen </author>
    /// <summary>
    ///  <see cref="IThemeStylesManager.RemoveThemeStyles"/>
    /// </summary>
    public bool RemoveThemeStyles(int id)
    {
        return _repository.DeleteThemeStyles(id);
    } // RemoveThemeStyles.
}