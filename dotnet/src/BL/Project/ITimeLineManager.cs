using Domain.Project;

namespace BL.Project;

/// <summary>
/// The interface of the ProjectManager, used for all the classes concerning a <see cref="Domain.Project.TimeLine"/>
/// </summary>
public interface ITimeLineManager
{
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns a timeline given the id.
    /// </summary>
    /// <param name="id"><see cref="TimeLine.TimeLineId"/></param>
    /// <param name="includeProject">Whether or not to include the navigation property for <see cref="TimeLine.Project"/></param>
    /// <returns></returns>
    public TimeLine GetTimeLine(int id, bool includeProject = false);

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Gets the timeline of a project.
    /// </summary>
    /// <param name="project">The project to get the timeline for.</param>
    /// <param name="includeTimeLinePhases">Whether or not to include the navigation property <see cref="TimeLine.TimeLinePhases"/></param>
    /// <returns></returns>
    public TimeLine GetTimeLineByProject(Domain.Project.Project project, bool includeTimeLinePhases = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Adds a new timeline in the database.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public TimeLine AddTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Changes a timeline in the database.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public TimeLine ChangeTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Remove a timeline.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool RemoveTimeLine(int id);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Get a timeline phase given the id.
    /// </summary>
    /// <param name="id"><see cref="TimeLinePhase.TimeLinePhaseId"/></param>
    /// <param name="includeTimeLine">Whether or not to include the navigation property for <see cref="TimeLinePhase.TimeLine"/></param>
    /// <param name="includeDocReview">Whether or not to include the navigation property for <see cref="TimeLinePhase.DocReview"/></param>
    /// <returns></returns>
    public TimeLinePhase GetTimeLinePhase(int id, bool includeTimeLine = false, bool includeDocReview = false);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns all the timeline phases of a timeline.
    /// </summary>
    /// <param name="timeLine"></param>
    /// <returns></returns>
    public IEnumerable<TimeLinePhase> GetTimeLinePhasesByTimeLine(TimeLine timeLine);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Adds a timeline phase.
    /// </summary>
    /// <param name="timeLinePhase"></param>
    /// <returns></returns>
    public TimeLinePhase AddTimeLinePhase(TimeLinePhase timeLinePhase);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Changes a timeline phase.
    /// </summary>
    /// <param name="timeLinePhase"></param>
    /// <returns></returns>
    public TimeLinePhase ChangeTimeLinePhase(TimeLinePhase timeLinePhase);
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Removes a timeline phase.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool RemoveTimeLinePhase(int id);
}