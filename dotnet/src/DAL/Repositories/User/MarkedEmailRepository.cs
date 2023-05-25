using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.User;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IMarkedEmailRepository"/>
/// </summary>
public class MarkedEmailRepository : Repository, IMarkedEmailRepository
{
    // Constructor.
    public MarkedEmailRepository(DocReviewDbContext context) : base(context){}

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.ReadMarkedEmail"/>
    /// </summary>
    public MarkedEmail ReadMarkedEmail(int id, bool includeProjects = false)
    {
        IQueryable<MarkedEmail> markedEmails = Context.MarkedEmails;

        if (includeProjects)
            markedEmails = markedEmails.Include(e => e.Projects);

        return markedEmails.SingleOrDefault(e => e.MarkedEmailId == id);
    } // ReadMarkedEmail

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.ReadMarkedEmailByEmail"/>
    /// </summary>
    public MarkedEmail ReadMarkedEmailByEmail(string email, bool includeProjects = false)
    {
        IQueryable<MarkedEmail> markedEmails = Context.MarkedEmails;

        if (includeProjects)
            markedEmails = markedEmails.Include(m => m.Projects);
        
        return markedEmails.SingleOrDefault(e => e.Email.ToLower() == email.ToLower());
    } // ReadMarkedEmailByEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.ReadMarkedEmails"/>
    /// </summary>
    public IEnumerable<MarkedEmail> ReadMarkedEmails(bool includeProjects = false)
    {
        IQueryable<MarkedEmail> markedEmails = Context.MarkedEmails;

        if (includeProjects)
            markedEmails = markedEmails.Include(m => m.Projects);

        return markedEmails;
    } // ReadMarkedEmailByEmail.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.ReadMarkedEmailsByProject"/>
    /// </summary>
    public IEnumerable<MarkedEmail> ReadMarkedEmailsByProject(Domain.Project.Project project, bool includeProjects = false)
    {
        IQueryable<MarkedEmail> markedEmails = Context.MarkedEmails;

        if (includeProjects)
            markedEmails = markedEmails.Include(m => m.Projects);

        return markedEmails.Where(e => e.Projects.Contains(project));
    } // ReadMarkedEmailsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.CreateMarkedEmail"/>
    /// </summary>
    public MarkedEmail CreateMarkedEmail(MarkedEmail markedEmail)
    {
        Context.MarkedEmails.Add(markedEmail);
        //Context.Entry(markedEmail.Projects).State = EntityState.Unchanged;
        Context.SaveChanges();
        return markedEmail;
    } // CreateMarkedEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.UpdateMarkedEmail"/>
    /// </summary>
    public MarkedEmail UpdateMarkedEmail(MarkedEmail markedEmail)
    {
        // This is the list of projects the marked email should have after updating.
        var newProjects = new List<Domain.Project.Project>(markedEmail.Projects);
        
        // Get the marked-email from the database.
        var newMarkedEmail = Context.MarkedEmails.Include(m => m.Projects)
            .SingleOrDefault(u => u.MarkedEmailId == markedEmail.MarkedEmailId);

        if (newMarkedEmail == null || !newMarkedEmail.Projects.Any())
            return null;
       
        // Remove all the projects that the marked-email had, but shouldn't have anymore. 
        foreach (var oldProject in newMarkedEmail?.Projects?.ToList() ?? Enumerable.Empty<Domain.Project.Project>())
        {
            if (!newProjects.Contains(oldProject))
                newMarkedEmail.Projects.Remove(oldProject);
        }
        
        // Add the roles the marked-emails currently does not have yet.
        foreach (var newProject in newProjects)
        {
            if (newMarkedEmail.Projects.All(r => r.ProjectId != newProject.ProjectId))
            {
                var newRole = new Domain.Project.Project { ProjectId = newProject.ProjectId };
                Context.Projects.Attach(newRole);
                newMarkedEmail.Projects.Add(newRole);
            }
        }
        
        Context.SaveChanges();

        return markedEmail;
    } // UpdateMarkedEmail.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IMarkedEmailRepository.DeleteMarkedEmail"/>
    /// </summary>
    public bool DeleteMarkedEmail(int id)
    {
        var markedEmail = Context.MarkedEmails.SingleOrDefault(e => e.MarkedEmailId == id);
        if (markedEmail == null)
            return false;
        
        Context.MarkedEmails.Remove(markedEmail);
        Context.SaveChanges();
        return true;
    } // DeleteMarkedEmail.
}