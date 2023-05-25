using Domain.Project;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Project;

/// <summary>
/// <see cref="ITimeLineRepository"/>
/// </summary>
public class TimeLineRepository : Repository, ITimeLineRepository
{
    // Constructor.
    public TimeLineRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.ReadTimeLine"/>
    /// </summary>
    public TimeLine ReadTimeLine(int id, bool includeProject = false)
    {
        var timeline = Context.TimeLines.AsQueryable();

        if (includeProject)
            timeline = timeline.Include(t => t.Project);

        return timeline.SingleOrDefault(t => t.TimeLineId == id);
    } // ReadTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.ReadTimeLineByProject"/>
    /// </summary>
    public TimeLine ReadTimeLineByProject(Domain.Project.Project project, bool includeTimeLinePhases = false)
    {
        var timelines =  Context.TimeLines.AsQueryable();
        
        if (includeTimeLinePhases)
            timelines = timelines.Include(t => t.TimeLinePhases);
        
        return timelines.SingleOrDefault(t => t.Project == project);
    } // ReadTimeLineByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.CreateTimeLine"/>
    /// </summary>
    public TimeLine CreateTimeLine(TimeLine timeLine)
    {
        Context.Entry(timeLine.Project).State = EntityState.Unchanged;
        Context.TimeLines.Add(timeLine);
        Context.SaveChanges();
        return timeLine;
    } // CreateTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.UpdateTimeLine"/>
    /// </summary>
    public TimeLine UpdateTimeLine(TimeLine timeLine)
    {
        var tmp = Context.TimeLines.SingleOrDefault(d => d.TimeLineId == timeLine.TimeLineId);
        if (tmp == null)
            return null;

        Context.Entry(tmp).CurrentValues.SetValues(timeLine);
        Context.SaveChanges();

        return timeLine;
    } // UpdateTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.DeleteTimeLine"/>
    /// </summary>
    public bool DeleteTimeLine(int id)
    {
        var tmp = Context.TimeLines.SingleOrDefault(t => t.TimeLineId == id);
        if (tmp == null)
            return false;

        Context.TimeLines.Remove(tmp);
        Context.SaveChanges();
        return true;
    } // DeleteTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.ReadTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase ReadTimeLinePhase(int id, bool includeTimeLine = false, bool includeDocReview = false)
    {
        var phases = Context.TimeLinePhases.AsQueryable();

        if (includeTimeLine)
            phases = phases.Include(p => p.TimeLine);

        if (includeDocReview)
            phases = phases.Include(p => p.DocReview);

        return phases.SingleOrDefault(p => p.TimeLinePhaseId == id);
    } // ReadTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.ReadTimeLinePhasesByTimeLine"/>
    /// </summary>
    public IEnumerable<TimeLinePhase> ReadTimeLinePhasesByTimeLine(TimeLine timeLine)
    {
        return Context.TimeLinePhases.Where(p => p.TimeLine == timeLine);
    } // ReadTimeLinePhasesByTimeLine.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.CreateTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase CreateTimeLinePhase(TimeLinePhase timeLinePhase)
    {
        Context.TimeLinePhases.Add(timeLinePhase);
        Context.SaveChanges();
        return timeLinePhase;
    } // CreateTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.UpdateTimeLinePhase"/>
    /// </summary>
    public TimeLinePhase UpdateTimeLinePhase(TimeLinePhase timeLinePhase)
    {
        Context.TimeLinePhases.Add(timeLinePhase);
        Context.Entry(timeLinePhase).State = EntityState.Modified;
        Context.SaveChanges();
        
        return timeLinePhase;
    } // UpdateTimeLinePhase.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="ITimeLineRepository.DeleteTimeLinePhase"/>
    /// </summary>
    public bool DeleteTimeLinePhase(int id)
    {
        var tmp = Context.TimeLinePhases.SingleOrDefault(t => t.TimeLinePhaseId == id);
        if (tmp == null)
            return false;

        Context.TimeLinePhases.Remove(tmp);
        Context.SaveChanges();
        return true;
    } // DeleteTimeLinePhase.
}