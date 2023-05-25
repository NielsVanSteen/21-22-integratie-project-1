using Domain.User;

namespace BL.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// The manager class for <see cref="Domain.User.MarkedEmail"/>
/// </summary>
public interface IMarkedEmailManager
{
     /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns a single markedEmail given the id.
    /// </summary>
    /// <param name="id">The id of the marked email.</param>
    /// <param name="includeProjects">Whether or not to include the navigation property <see cref="Domain.User.MarkedEmail.Projects"/></param>
    /// <returns></returns>
    public MarkedEmail GetMarkedEmail(int id, bool includeProjects = false);
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Searches if an email exists in the MarkedEmail table.
    /// </summary>
    /// <param name="email">The email to search for.</param>
    /// <param name="includeProjects">Whether or not to include the navigation property <see cref="Domain.User.MarkedEmail.Projects"/></param>
    /// <returns> <see cref="Domain.User.MarkedEmail"/> object. </returns>
    public MarkedEmail GetMarkedEmailByEmail(string email, bool includeProjects = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the marked-emails.
    /// </summary>
    /// <param name="includeProjects">Whether or not to include the navigation property <see cref="Domain.User.MarkedEmail.Projects"/></param>
    /// <returns></returns>
    public IEnumerable<MarkedEmail> GetMarkedEmails(bool includeProjects = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the marked-emails.
    /// </summary>
    /// <param name="project">The project to return all the marked-emails for.</param>
    /// <param name="includeProjects">Whether or not to include the navigation property <see cref="Domain.User.MarkedEmail.Projects"/></param>
    /// <returns></returns>
    public IEnumerable<MarkedEmail> GetMarkedEmailsByProject(Domain.Project.Project project, bool includeProjects = false);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Creates a marked email.
    /// </summary>
    /// <param name="markedEmail">The object to create.</param>
    /// <returns></returns>
    public MarkedEmail AddMarkedEmail(MarkedEmail markedEmail);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Updates a marked email.
    /// </summary>
    /// <param name="markedEmail">The object to create.</param>
    /// <returns></returns>
    public MarkedEmail ChangeMarkedEmail(MarkedEmail markedEmail);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Deletes a markedEmail.
    /// </summary>
    /// <param name="id">The id of the MarkedEmail to delete.</param>
    /// <returns></returns>
    public bool RemoveMarkedEmail(int id);
}