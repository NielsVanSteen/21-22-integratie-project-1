using Domain.Project;

namespace UI.MVC.Models.Shared.PopUp;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class holds all the non-generic information a popup windows shows. this way the html for the popup windows can be in a partial view
/// and can be reused everywhere in the application.
/// </summary>
public class PopUpModel
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This is the name of the article-element and is unique per type of record, See remarks for more info.
    /// </summary>
    /// <remarks>
    /// E.g., on the <see cref="ProjectTag"/> page a list of tags is displayed, when the user presses the delete button a popup will be shown.
    /// This property will have a class like: 'article-popup-delete-project-tag' it is a unique class only for popups that are meant to delete a <see cref="ProjectTag"/>
    /// </remarks>
    public string PopUpContainerClass { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The title of the popup. E.g., 'Delete Project Tag?'.
    /// </summary>
    public string PopUpTitle { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The body of the popup. For deleting records this most likely displays some warning message.
    /// </summary>
    public string PopUpBody { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The class the primary  button has. This is a unique class and the event listener that will make the http-call is attached to this class.
    /// This button is E.g., (Delete, Save, Submit, ...).
    /// </summary>
    public string ButtonPrimaryClass { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Text the primary button has E.g., Delete, Submit, Create, ..
    /// </summary>
    public string ButtonPrimaryText { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The bootstrap class of the button. E.g, btn-danger (for delete), btn-success (for create), ...
    /// </summary>
    public string ButtonPrimaryBootstrapType { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Whether the popup is a confirm popup, or just a message popup.
    /// A Confirm popup has a button to confirm, while a message popup only has an ok button.
    /// </summary>
    public bool IsConfirmPopup { get; set; } = true;

    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Does the popup window have a close button on top.
    /// </summary>
    public bool HasClosedButton { get; set; } = true;
    
    // Constructor.
    public PopUpModel() {}
}