using System.Linq;
using System.Text.RegularExpressions;
using Domain.Comment;
using Domain.DocReview;
using Domain.ProjectStatistics;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// <see cref="IProjectStatisticsRepository"/>
/// </summary>
public class ProjectStatisticsRepository : Repository, IProjectStatisticsRepository
{
    // Constructor.
    public ProjectStatisticsRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.CreateProjectStatistics"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics CreateProjectStatistics(
        Domain.ProjectStatistics.ProjectStatistics projectStatistics)
    {
        Context.ProjectStatistics.Add(projectStatistics);
        Context.SaveChanges();
        return projectStatistics;
    } // CreateProjectStatistics.

    /// <author> Bjorn Straetemans </author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadLastProjectStatisticByProject(Domain.Project.Project)"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics ReadLastProjectStatisticByProject(Domain.Project.Project project)
    {
        var statistic = Context.ProjectStatistics
            .Include(s => s.Project)
            .Include(s => s.CommentStatusTypeAmount)
            .Include(s => s.DocReviewStatusTypeAmount)
            .Include(s => s.EmojiTypeAmount)
            .ThenInclude(s => s.Emoji)
            .Where(s => s.Project == project);

        if (!statistic.Any())
        {
            return null;
        }

        return statistic.OrderBy(p => p.LastUpdated).Last();
    }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadProjectStatisticsByProject(Domain.Project.Project)"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProject(
        Domain.Project.Project project)
    {
        return Context.ProjectStatistics
            .Include(s => s.Project)
            .Include(s => s.CommentStatusTypeAmount)
            .Include(s => s.DocReviewStatusTypeAmount)
            .Include(s => s.EmojiTypeAmount)
            .ThenInclude(s => s.Emoji)
            .Where(s => s.Project == project);
    } // ReadProjectStatisticsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadProjectStatistics"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics ReadProjectStatistics(int id)
    {
        return Context.ProjectStatistics
            .Include(s => s.Project)
            .Include(s => s.CommentStatusTypeAmount)
            .Include(s => s.DocReviewStatusTypeAmount)
            .Include(s => s.EmojiTypeAmount)
            .ThenInclude(s => s.Emoji)
            .SingleOrDefault(s => s.ProjectStatisticsId == id);
    } // ReadProjectStatistics.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read project statistics given the project and DAY.
    /// </summary>
    /// <returns></returns>
    public Domain.ProjectStatistics.ProjectStatistics ReadProjectStatisticsByProjectAndDay(
        Domain.Project.Project project, DateTime dateTime)
    {
        return Context.ProjectStatistics.SingleOrDefault(s =>
            s.Project == project && s.LastUpdated.Date == dateTime.Date);
    } // ReadProjectStatisticsByProjectAndDay.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadProjectStatisticsByProject(Domain.Project.Project, int)"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProject(
        Domain.Project.Project project, int detailAmount)
    {
        var size = Context.ProjectStatistics.Count();

        if (detailAmount > size)
            detailAmount = size;

        var result = Context.ProjectStatistics
            .Where(s => s.Project == project)
            .Include(s => s.Project)
            .Include(s => s.CommentStatusTypeAmount)
            .Include(s => s.DocReviewStatusTypeAmount)
            .Include(s => s.EmojiTypeAmount)
            .ThenInclude(s => s.Emoji)
            .AsEnumerable()

            // must use .AsEnumerable() -> Entity framework doesn't support .chunk below: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/supported-and-unsupported-linq-methods-linq-to-entities?redirectedfrom=MSDN
            .Chunk(size / detailAmount)
            .Select(ch => ch.FirstOrDefault());

        return result;
    } // ReadProjectStatisticsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.CreateProjectStatistics"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProjectAndTimeFrame(
        Domain.Project.Project project, int detailAmount, DateTime beginDate, DateTime endDate)
    {
        var size = Context.ProjectStatistics.Count(p => p.Project == project);

        if (detailAmount > size)
            detailAmount = size;

        var result = Context.ProjectStatistics
            .Where(s => s.Project == project)
            .Where(s => s.LastUpdated >= beginDate && s.LastUpdated <= endDate)
            .Include(s => s.Project)
            .Include(s => s.CommentStatusTypeAmount)
            .Include(s => s.DocReviewStatusTypeAmount)
            .Include(s => s.EmojiTypeAmount)
            .ThenInclude(e => e.Emoji)
            .AsEnumerable()

            // must use .AsEnumerable() -> Entity framework doesn't support .chunk below: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/supported-and-unsupported-linq-methods-linq-to-entities?redirectedfrom=MSDN
            .Chunk(size / detailAmount)
            .Select(ch => ch.FirstOrDefault());

        return result;
    } // ReadProjectStatisticsByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadReactionGroupAmountByProject"/>
    /// </summary>
    public int ReadReactionGroupAmountByProject(Domain.Project.Project project)
    {
        return Context.Comments.Count(c => c.DocReview.Project == project);
    } // ReadReactionGroupAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadReactionGroupAmountByProject"/>
    /// </summary>
    public int ReadEmojiAmountByProject(Domain.Project.Project project)
    {
        return Context.EmojiReactions
            .Count(e => e.DocReview.Project == project);
    } // ReadEmojiAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadReactionGroupAmountByProject"/>
    /// </summary>
    public IEnumerable<EmojiTypeTotal> ReadEmojiTypesAmountByProject(Domain.Project.Project project)
    {
        // SQL query.

        //  SELECT COUNT(*), c.EmojiId, e.Code
        //  FROM CommentComposites c
        //  INNER JOIN Emojis e 
        //  ON c.EmojiId = e.EmojiId
        //  INNER JOIN DocReviews d
        //  ON c.DocReviewId = d.DocReviewId
        //  WHERE c.EmojiId IS NOT NULL AND d.ProjectId = 1
        //  GROUP BY c.EmojiId

        // LINQ method syntax.
        var result = Context.EmojiReactions // Take all emoji reactions.
            .Where(e => e.DocReview.Project == project) // Filter on project.
            .Include(e => e.Emoji) // Include Emoji's
            .GroupBy(info => info.Emoji.EmojiId) // Group By emoji id
            .Select(group => // Create a tuple that holds the Emoji Id, EmojiCode & EmojiId
                new EmojiTypeTotal
                {
                    Emoji = group.FirstOrDefault().Emoji,
                    Total = group.Count()
                });

        return result.AsEnumerable();
    } // ReadEmojiTypesAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadReactionGroupAmountByProject"/>
    /// </summary>
    public int ReadUsersAmountByProject(Domain.Project.Project project, string identifier)
    {
        return Context.Users.Count(u =>
            u.RegisteredForProjects.Contains(project) && !u.UserName.ToLower().EndsWith(identifier.ToLower()));
    } // ReadUsersAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadProjectManagersAmountByProject"/>
    /// </summary>
    public int ReadProjectManagersAmountByProject(Domain.Project.Project project, string identifier)
    {
        return Context.Users.Count(u =>
            u.RegisteredForProjects.Contains(project) && u.UserName.ToLower().EndsWith(identifier.ToLower()));
    } // ReadProjectManagersAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadCommentsStatusAmountByProject"/>
    /// </summary>
    public IEnumerable<CommentStatusTotal> ReadCommentsStatusAmountByProject(Domain.Project.Project project)
    {
        // Sql.

        //  SELECT COUNT(h.CommentStatus), h.CommentStatus
        //  FROM CommentComposites c
        //  INNER JOIN CommentHistories h
        //  ON h.ReactionGroupId = c.CommentId
        //  INNER JOIN DocReviews d
        //  ON d.DocReviewId = c.DocReviewId
        //  WHERE h.EditedOn = (
        //      SELECT MAX(editedOn)
        //      FROM CommentHistories h2
        //      WHERE h2.ReactionGroupId = c.CommentId
        //  ) AND d.ProjectId = 1
        //  GROUP BY h.CommentStatus

        // LINQ.
        var result = Context
            .CommentHistories
            .Where(h => h.ReactionGroup.DocReview.Project == project)
            .Where(h =>
                h.EditedOn == Context
                    .CommentHistories
                    .Where(h2 => h2.ReactionGroup == h.ReactionGroup).Max(d => d.EditedOn)
            )
            .GroupBy(h => h.CommentStatus)
            .Select(group => new
                CommentStatusTotal()
                {
                    CommentStatus = group.FirstOrDefault().CommentStatus,
                    Total = group.Count()
                });

        return result.AsEnumerable();
    } // ReadCommentsStatusAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadDocReviewsStatusAmountByProject"/>
    /// </summary>
    public IEnumerable<DocReviewStatusTotal> ReadDocReviewsStatusAmountByProject(Domain.Project.Project project)
    {
        // Sql.

        //  SELECT COUNT(h.DocReviewStatus), h.DocReviewStatus
        //  FROM DocReviews d
        //  INNER JOIN DocReviewHistories h
        //  ON h.DocReviewId = d.DocReviewId
        //  WHERE h.EditedOn = (
        //      SELECT MAX(h2.editedOn)
        //      FROM DocReviewHistories h2
        //      WHERE h2.DocReviewId = h.DocReviewId
        //  )
        //  GROUP BY h.DocReviewStatus;

        // LINQ.
        var result = Context
            .DocReviewHistories
            .Where(h => h.DocReview.Project == project)
            .Where(h =>
                h.EditedOn == Context
                    .DocReviewHistories
                    .Where(h2 => h2.DocReviewId == h.DocReviewId).Max(d => d.EditedOn)
            )
            .GroupBy(h => h.DocReviewStatus)
            .Select(group => new
                DocReviewStatusTotal()
                {
                    DocReviewStatus = group.FirstOrDefault().DocReviewStatus,
                    Total = group.Count()
                });

        return result;
    } // ReadDocReviewsStatusAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadDocReviewsAmountByProject"/>
    /// </summary>
    public int ReadDocReviewsAmountByProject(Domain.Project.Project project)
    {
        return Context.DocReviews.Count(d => d.Project == project);
    } // ReadDocReviewsAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsRepository.ReadSurveyStatisticsByProject"/>
    /// </summary>
    public IEnumerable<SurveyStatistic> ReadSurveyStatisticsByProject(Domain.Project.Project project)
    {
        // Don't ask my how I did it, or how it works, but it does. ~ Niels
        // Only took a bit over 3 hours to get it working, I'm fine ;\
        
        // The first part of the query groups per survey & option -> returns per row: the survey, the option & the amount of of times that option has been chosen on the survey. (see first .select for more info).
        var result = Context.SurveyAnswers
            .Where(s => s.Survey.DocReview.Project == project)
            .Include(t => t.ChosenOption)
            .Include(t => t.ChosenOption.Survey).ThenInclude(t => t.DocReview)
            .Include(t => t.Survey).ThenInclude(t => t.DocReview)
            .GroupBy(s => new
            {
                s.Survey.SurveyId,
                s.ChosenOption.SurveyOptionId
            })
            
            // This is the result of a single row: SurveyId, optionId & the amount that option has been chosen on the survey.
            .Select(group => new
            {
                SurveyId = group.Key.SurveyId,
                OptionId = group.Key.SurveyOptionId,
                Amount = group.Count()
            })
            
            // All the heavy weight filtering has been done in the db -> The only problem is that the result = one row per option & count(option) for a survey. We want 1 survey that has a list of option & count(option).
            // this problem is solved in the LINQ below. doesn't matter it's in memory because a survey should have more than half a dozen options. AND I think it's impossible in SQL, and I know it's definitely impossible with EF-LINQ.
            .AsEnumerable()
            .GroupBy(e => e.SurveyId)
            .Select(s => new SurveyStatistic()
            {
                // This part creates a list, where each item in the list is the option and the amount of time that option has been chosen on the survey.
                OptionsAmount = s
                    .GroupBy(e => e.OptionId)
                    .Select(t => new OptionAmount()
                    {
                        SurveyOption = Context
                            .SurveyOptions
                            .Find(t.Key),
                        Amount = t.Sum(e => e.Amount)
                    }),
                
                // This part selects the current survey.
                Survey = Context
                    .Surveys
                    //.Find(s.Key)
                    .Include(s => s.DocReview)
                    .SingleOrDefault(i => i.SurveyId == s.Key)
            });

        return result;
    } // ReadSurveyStatisticsByProject.
}