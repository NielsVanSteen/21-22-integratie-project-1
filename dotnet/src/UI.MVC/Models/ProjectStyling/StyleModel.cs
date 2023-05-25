using Domain.Project;
using UI.MVC.Models.Shared;

namespace UI.MVC.Models.ProjectStyling;

/// <author> Niels Van Steen</author>
/// <summary>
/// The model to display a the theme styles.
/// </summary>
public class StyleModel
{
    // Properties.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// All the styles.
    /// </summary>
    public IEnumerable<ThemeStyles> Styles { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Are the styles global styles or project specific styles.
    /// </summary>
    public bool IsGlobalStyle { get; set; }
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The id of the active style.
    /// </summary>
    public int ActiveStyleId { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The page title.
    /// </summary>
    public PageTitleModel PageTitleModel { get; set; }
    
    // Constructor.
    public StyleModel()
    {
    }
}