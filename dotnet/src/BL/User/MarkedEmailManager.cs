using System.ComponentModel.DataAnnotations;
using DAL.Repositories.User;
using Domain.User;

namespace BL.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IMarkedEmailManager"/>
/// </summary>
public class MarkedEmailManager : IMarkedEmailManager
{
   
    // Fields.
    private IMarkedEmailRepository _repository;

    // Constructor.
    public MarkedEmailManager(IMarkedEmailRepository repository)
    {
        _repository = repository;
    } // UserService.

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmail"/>
    /// </summary>
    public MarkedEmail GetMarkedEmail(int id, bool includeProjects = false)
    {
        return _repository.ReadMarkedEmail(id, includeProjects);
    } // GetMarkedEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmailByEmail"/>
    /// </summary>
    public MarkedEmail GetMarkedEmailByEmail(string email, bool includeProjects = false)
    {
        return _repository.ReadMarkedEmailByEmail(email, includeProjects);
    } // GetMarkedEmailByEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmails"/>
    /// </summary>
    public IEnumerable<MarkedEmail> GetMarkedEmails(bool includeProjects = false)
    {
        return _repository.ReadMarkedEmails(includeProjects);
    } // GetMarkedEmails.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmail"/>
    /// </summary>
    public IEnumerable<MarkedEmail> GetMarkedEmailsByProject(Domain.Project.Project project, bool includeProjects = false)
    {
        return _repository.ReadMarkedEmailsByProject(project, includeProjects);
    } // GetMarkedEmailsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmail"/>
    /// </summary>
    public MarkedEmail AddMarkedEmail(MarkedEmail markedEmail)
    {
        Validator.ValidateObject(markedEmail, new ValidationContext(markedEmail), validateAllProperties: true);
        return _repository.CreateMarkedEmail(markedEmail);
    } // AddMarkedEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmail"/>
    /// </summary>
    public MarkedEmail ChangeMarkedEmail(MarkedEmail markedEmail)
    {
        Validator.ValidateObject(markedEmail, new ValidationContext(markedEmail), validateAllProperties: true);
        return _repository.UpdateMarkedEmail(markedEmail);
    } // ChangeMarkedEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailManager.GetMarkedEmail"/>
    /// </summary>
    public bool RemoveMarkedEmail(int id)
    {
        return _repository.DeleteMarkedEmail(id);
    } // RemoveMarkedEmail.
}