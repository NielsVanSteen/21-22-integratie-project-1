using System.ComponentModel.DataAnnotations;
using DAL.Repositories.Project;
using Domain.Project;

namespace BL.Project;

/// <summary>
/// <see cref="ITimeLineManager"/>
/// </summary>
public class TimeLineManager : ITimeLineManager
{
    // Fields.
    private ITimeLineRepository _repository;

    // Constructor.
    public TimeLineManager(ITimeLineRepository repository)
    {
        _repository = repository;
    } // TimeLineManager.

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.GetTimeLine"/>
    /// </summary>
    public TimeLine GetTimeLine(int id, bool includeProject = false)
    {
        return _repository.ReadTimeLine(id, includeProject);
    } // GetTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.GetTimeLineByProject"/>
    /// </summary>
    public TimeLine GetTimeLineByProject(Domain.Project.Project project, bool includeTimeLinePhases = false)
    {
        return _repository.ReadTimeLineByProject(project, includeTimeLinePhases);
    } // GetTimeLineByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.AddTimeLine"/>
    /// </summary>
    public TimeLine AddTimeLine(TimeLine timeLine)
    {
        Validator.ValidateObject(timeLine, new ValidationContext(timeLine), validateAllProperties: true);
        return _repository.CreateTimeLine(timeLine);
    } // AddTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.ChangeTimeLine"/>
    /// </summary>
    public TimeLine ChangeTimeLine(TimeLine timeLine)
    {
        Validator.ValidateObject(timeLine, new ValidationContext(timeLine), validateAllProperties: true);
        return _repository.UpdateTimeLine(timeLine);
    } // ChangeTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.RemoveTimeLine"/>
    /// </summary>
    public bool RemoveTimeLine(int id)
    {
        return _repository.DeleteTimeLine(id);
    } // RemoveTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.GetTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase GetTimeLinePhase(int id, bool includeTimeLine = false, bool includeDocReview = false)
    {
        return _repository.ReadTimeLinePhase(id, includeTimeLine, includeDocReview);
    } // GetTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.GetTimeLinePhasesByTimeLine"/>
    /// </summary>
    public IEnumerable<TimeLinePhase> GetTimeLinePhasesByTimeLine(TimeLine timeLine)
    {
        return _repository.ReadTimeLinePhasesByTimeLine(timeLine);
    } // GetTimeLinePhasesByTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.AddTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase AddTimeLinePhase(TimeLinePhase timeLinePhase)
    {
        Validator.ValidateObject(timeLinePhase, new ValidationContext(timeLinePhase), validateAllProperties: true);
        return _repository.CreateTimeLinePhase(timeLinePhase);
    } // AddTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.ChangeTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase ChangeTimeLinePhase(TimeLinePhase timeLinePhase)
    {
        Validator.ValidateObject(timeLinePhase, new ValidationContext(timeLinePhase), validateAllProperties: true);
        return _repository.UpdateTimeLinePhase(timeLinePhase);
    } // ChangeTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineManager.RemoveTimeLinePhase"/>
    /// </summary>
    public bool RemoveTimeLinePhase(int id)
    {
        return _repository.DeleteTimeLinePhase(id);
    } // RemoveTimeLinePhase.
}