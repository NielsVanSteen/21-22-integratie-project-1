using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <summary>
/// <see cref="IProjectFooterLogoRepository"/>
/// </summary>
public class ProjectFooterLogoRepository : Repository, IProjectFooterLogoRepository
{
    // Fields.
    public ProjectFooterLogoRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoRepository.ReadFooterLogosByProject"/>
    /// </summary>
    public IEnumerable<FooterLogo> ReadFooterLogosByProject(Domain.Project.Project project)
    {
        IQueryable<FooterLogo> footerLogos = Context.FooterLogos;
        return footerLogos.Where(l => l.Project == project);
    } // ReadFooterLogosByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoRepository.ReadFooterLogo"/>
    /// </summary>
    public FooterLogo ReadFooterLogo(int id, bool includeProject = false)
    {
        IQueryable<FooterLogo> footerLogos = Context.FooterLogos;

        if (includeProject)
            footerLogos.Include(l => l.Project);

        return footerLogos.SingleOrDefault(l => l.FooterLogoId == id);
    } // ReadFooterLogo.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoRepository.CreateFooterLogo"/>
    /// </summary>
    public FooterLogo CreateFooterLogo(FooterLogo footerLogo)
    {
        Context.FooterLogos.Add(footerLogo);
        Context.Entry(footerLogo.Project).State = EntityState.Unchanged;
        Context.SaveChanges();
        return footerLogo;
    } // CreateFooterLogo.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoRepository.DeleteFooterLogo"/>
    /// </summary>
    public bool DeleteFooterLogo(int id)
    {
        var logo = Context.FooterLogos.SingleOrDefault(t => t.FooterLogoId == id);
        if (logo == null)
            return false;
        Context.FooterLogos.Remove(logo);
        Context.SaveChanges();
        return true;
    } // DeleteFooterLogo.
}