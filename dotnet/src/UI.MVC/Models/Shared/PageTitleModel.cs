namespace UI.MVC.Models.Shared;

/// <author> Niels Van Steen</author>
/// <summary>
/// A model to insert generic page titles that are the same on all views.
/// </summary>
public class PageTitleModel
{
    // Properties.
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// The actual page title.
    /// </summary>
    public string Title { get; set; }
    
    /// <author> Niels Van Steen</author>
    /// <summary>
    /// A description, shown on hovering a question mark next to the title giving more information.
    /// </summary>
    public string Description { get; set; }

    /// <author> Niels Van Steen</author>
    /// <summary>
    /// Some page titles have an 'action link' this element is optional and contains that element.
    /// </summary>
    public string ActionElement { get; set; }
    
    // Constructor.
    public PageTitleModel()
    {
    }
}