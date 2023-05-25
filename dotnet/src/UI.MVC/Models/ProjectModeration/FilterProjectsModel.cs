using Domain.Project;
using Domain.Util;

namespace UI.MVC.Models.ProjectModeration;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model to sort projects
/// </summary>
public class FilterProjectsModel
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="Project"/> name to filter on.
    /// This searches for the <see cref="Project.InternalName"/> as well as the <see cref="Project.ExternalName"/>.
    /// </summary>
    public string Name { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The order to sort on.
    /// <seealso cref="SortOrder"/> for more information.
    /// </summary>
    public SortOrder SortOrder { get; set; }
    
    // Constructor.
    public FilterProjectsModel() {}
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Maps the sort view model <see cref="SortOrder"/> to the domain model <see cref="Domain.Util.SortOrder"/>.
    /// </summary>
    public Domain.Util.SortOrder MapSortOrder()
    {
        return SortOrder == SortOrder.Ascending ? Domain.Util.SortOrder.Ascending : Domain.Util.SortOrder.Descending;
    } // MapSortOrder.
}