using System.ComponentModel.DataAnnotations;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// Each project has a timeline consisting of one or multiple <see cref="TimeLinePhases"/>.
/// </summary>
public class TimeLine
{
    // Properties.
    [Key]
    public int TimeLineId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="Project"/> the <see cref="TimeLine"/> belongs to.
    /// </summary>
    public Project Project { get; set; }

    /// <author>Brian Nys</author>
    /// <summary>
    /// The foreign key the <see cref="Project"/> the <see cref="TimeLine"/> belongs to.
    /// </summary>
    public int ProjectId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// All the phases belonging to the timeline.
    /// </summary>
    public ICollection<TimeLinePhase> TimeLinePhases { get; set; }

    // Constructor.
    public TimeLine() { }
}