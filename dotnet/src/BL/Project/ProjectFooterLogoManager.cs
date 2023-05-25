using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Project;

namespace BL.Project;

public class ProjectFooterLogoManager : IProjectFooterLogoManager
{
    // Fields.
    private readonly IProjectFooterLogoRepository _repository;

    // Constructor.
    public ProjectFooterLogoManager(IProjectFooterLogoRepository repository)
    {
        _repository = repository;
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoManager.GetFooterLogosByProject"/>
    /// </summary>
    public IEnumerable<FooterLogo> GetFooterLogosByProject(Domain.Project.Project project)
    {
        return _repository.ReadFooterLogosByProject(project);
    } // GetFooterLogosByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoManager.GetFooterLogo"/>
    /// </summary>
    public FooterLogo GetFooterLogo(int id, bool includeProject = false)
    {
        return _repository.ReadFooterLogo(id, includeProject);
    } // GetFooterLogo.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoManager.AddFooterLogo"/>
    /// </summary>
    public FooterLogo AddFooterLogo(FooterLogo footerLogo)
    {
        Validator.ValidateObject(footerLogo, new ValidationContext(footerLogo), validateAllProperties: true);
        return _repository.CreateFooterLogo(footerLogo);
    } // AddFooterLogo.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectFooterLogoManager.RemoveFooterLogo"/>
    /// </summary>
    public bool RemoveFooterLogo(int id)
    {
        return _repository.DeleteFooterLogo(id);
    } // RemoveFooterLogo
}