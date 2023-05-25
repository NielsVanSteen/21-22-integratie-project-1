namespace UI.MVC.Models.Shared;

/// <author> Niels Van Steen </author>
/// <summary>
/// Model to show the breadcrumbs at the top of the page.
/// </summary>
public class BreadcrumbsModel
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// List containing all the breadcrumb items.
    ///
    /// A single item contains:
    /// 1. IconClass = the css classes for the font-awesome class to display the icon.
    /// 2. Name = the hyperlink text.
    /// 3. Controller = Name of the controller of that breadcrumb item.
    /// 4. Action = name of the action of that breadcrumb item.
    /// 5. ExternalProjectName = the external projectName in the url when the user clicks the icon.
    /// </summary>
    public IEnumerable<(string IconClass, string Name, string Controller, string Action, string ExternalProjectName)>
        BreadcrumbItems { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// NOTE: Index 1 = 0'd item since index 0 = the home icon.
    /// </summary>
    public int ActiveItemIndex { get; set; }

    // Constructor.
    public BreadcrumbsModel()
    {
    }
}