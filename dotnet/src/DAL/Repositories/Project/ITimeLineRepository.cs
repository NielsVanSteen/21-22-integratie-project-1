using Domain.Project;

namespace DAL.Repositories.Project;

/// <summary>
/// The repository interface for all the classes concerning the TimeLine.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.Project.TimeLine"/>
/// <see cref="Domain.Project.TimeLinePhase"/>
/// </example>
public interface ITimeLineRepository
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns a timeline given the id.
    /// </summary>
    /// <param name="id"><see cref="TimeLine.TimeLineId"/></param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="TimeLine.Project"/></param>
    /// <returns></returns>
    public TimeLine ReadTimeLine(int id, bool includeProject = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Reads the timeline of a project.
    /// </summary>
    /// <param name="project">The project to get the timeline for.</param>
    /// <param name="includeTimeLinePhases">Whether or not to include the navigation property <see cref="TimeLine.TimeLinePhases"/></param>
    /// <returns></returns>
    public TimeLine ReadTimeLineByProject(Domain.Project.Project project, bool includeTimeLinePhases = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Creates a new timeline in the database.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public TimeLine CreateTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Updates a timeline in the database.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public TimeLine UpdateTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Delete a timeline.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteTimeLine(int id);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Read a timeline phase given the id.
    /// </summary>
    /// <param name="id"><see cref="TimeLinePhase.TimeLinePhaseId"/></param>
    /// <param name="includeTimeLine">Whether or not to include the navigation property for <see cref="TimeLinePhase.TimeLine"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="TimeLinePhase.DocReview"/></param>
    /// <returns></returns>
    public TimeLinePhase ReadTimeLinePhase(int id, bool includeTimeLine = false, bool includeDocReview = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns all the timeline phases of a timeline.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public IEnumerable<TimeLinePhase> ReadTimeLinePhasesByTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Creates a timeline phase.
    /// </summary>
    /// <param name="timeLinePhase"></param>
    /// <returns></returns>
    public TimeLinePhase CreateTimeLinePhase(TimeLinePhase timeLinePhase);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Updates a timeline phase.
    /// </summary>
    /// <param name="timeLinePhase"></param>
    /// <returns></returns>
    public TimeLinePhase UpdateTimeLinePhase(TimeLinePhase timeLinePhase);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Deletes a timeline phase.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool DeleteTimeLinePhase(int id);
    
}