using Domain.User;

namespace UI.MVC.Models.ProjectModeration;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model used to show the moderators and marked emails.
/// </summary>
public class ViewModeratorsModel
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// List with all the users who are either an <see cref="UserRole.Admin"/> or a <see cref="UserRole.ProjectManager"/>.
    /// </summary>
    public IEnumerable<User> Users { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// All the marked emails.
    /// </summary>
    public IEnumerable<MarkedEmail> MarkedEmails { get; set; }
    
    // Constructor.
    public ViewModeratorsModel() {}
}