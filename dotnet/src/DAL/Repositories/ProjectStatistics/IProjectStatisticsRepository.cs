using Domain.Comment;
using Domain.DocReview;
using Domain.Project;
using Domain.ProjectStatistics;

namespace DAL.Repositories.ProjectStatistics;

/// <author>Niels Van Steen</author>
/// <summary>
/// Repository interface used to get all the statistical values shown on the dashboard.
/// </summary>
public interface IProjectStatisticsRepository
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Create a new <see cref="Domain.ProjectStatistics.ProjectStatistics"/>record.
    /// </summary>
    /// <param name="projectStatistics"></param>
    /// <returns></returns>
    public Domain.ProjectStatistics.ProjectStatistics CreateProjectStatistics(
        Domain.ProjectStatistics.ProjectStatistics projectStatistics);

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Read the last<see cref="Domain.ProjectStatistics.ProjectStatistics"/> given the <see cref="Project"/>
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public Domain.ProjectStatistics.ProjectStatistics ReadLastProjectStatisticByProject(Domain.Project.Project project);


    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read all the <see cref="Domain.ProjectStatistics.ProjectStatistics"/> given the <see cref="Project"/>
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProject(
        Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read the <see cref="Domain.ProjectStatistics.ProjectStatistics"/> given the <see cref="Domain.ProjectStatistics.ProjectStatistics.ProjectStatisticsId"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Domain.ProjectStatistics.ProjectStatistics ReadProjectStatistics(int id);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Read project statistics given the project and DAY.
    /// </summary>
    /// <returns></returns>
    public Domain.ProjectStatistics.ProjectStatistics ReadProjectStatisticsByProjectAndDay(
        Domain.Project.Project project, DateTime dateTime);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get all the <see cref="Domain.ProjectStatistics.ProjectStatistics"/> given the <see cref="Project"/>.
    /// </summary>
    /// <param name="project">The project to get all the statistics for.</param>
    /// <param name="detailAmount">
    /// making a chart with e.g., +1000 items is ridiculous (and slow + resource intensive), thus the <see cref="detailAmount"/>
    /// specifies the amount of records that will be returned -> these records will be evenly spread across all the rows.
    /// </param>
    /// <returns></returns>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProject(
        Domain.Project.Project project, int detailAmount);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get all the <see cref="Domain.ProjectStatistics.ProjectStatistics"/> given the <see cref="Project"/>. between two dates.
    /// </summary>
    /// <param name="project">The project to get all the statistics for.</param>
    /// <param name="detailAmount">
    /// making a chart with e.g., +1000 items is ridiculous (and slow + resource intensive), thus the <see cref="detailAmount"/>
    /// specifies the amount of records that will be returned -> these records will be evenly spread across all the rows.
    /// </param>
    /// <param name="beginDate">The begin date which all the statistics me come after.</param>
    /// <param name="endDate">The end date which all the statistics must be before.</param>
    /// <returns></returns>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> ReadProjectStatisticsByProjectAndTimeFrame(
        Domain.Project.Project project, int detailAmount, DateTime beginDate, DateTime endDate);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStatistics.ReactionGroupAmount"/>.
    /// </summary>
    /// <param name="project">The project to get the <see cref="ReactionGroup"/> total for.</param>
    /// <returns></returns>
    public int ReadReactionGroupAmountByProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStatistics.EmojiAmount"/>.
    /// </summary>
    /// <param name="project">The project to get the <see cref="EmojiReaction"/> total for.</param>
    /// <returns></returns>
    public int ReadEmojiAmountByProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectStatistics.EmojiTypeAmount"/>.
    /// </summary>
    /// <param name="project">The project to return the total amounts of emoji types for..</param>
    /// <returns>
    /// Named tuple containing <see cref="Emoji.EmojiId"/> <see cref="Emoji.Code"/> and a number displaying the amount of times that emoji has been placed on any <see cref="DocReview"/>
    /// in the entire <see cref="Project"/>.
    /// </returns>
    public IEnumerable<EmojiTypeTotal> ReadEmojiTypesAmountByProject(
        Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the amount of users with the role <see cref="Domain.User.UserRole.RegularUser"/>
    /// <see cref="ProjectStatistics.UsersAmount"/>.
    /// </summary>
    /// <param name="project">The project to get the <see cref="User"/> total for.</param>
    /// <param name="identifier">
    /// <see cref="Domain.User.UserRole.Admin"/> and <see cref="Domain.User.UserRole.ProjectManager"/> don't have the <see cref="Project.ExternalName"/>
    /// in their username, thus this value is used to check quickly if a user is a <see cref="Domain.User.UserRole.RegularUser"/> or not.
    /// </param>
    /// <returns></returns>
    public int ReadUsersAmountByProject(Domain.Project.Project project, string identifier);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the amount of users with the role <see cref="Domain.User.UserRole.ProjectManager"/>
    /// <see cref="ProjectStatistics.ManagersAmount"/>.
    /// </summary>
    /// <param name="project">The project to get the <see cref="User"/> total for.</param>
    /// <param name="identifier">
    /// <see cref="Domain.User.UserRole.Admin"/> and <see cref="Domain.User.UserRole.ProjectManager"/> don't have the <see cref="Project.ExternalName"/>
    /// in their username, thus this value is used to check quickly if a user is a <see cref="Domain.User.UserRole.RegularUser"/> or not.
    /// </param>
    /// <returns></returns>
    public int ReadProjectManagersAmountByProject(Domain.Project.Project project, string identifier);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Instead of making a method to retrieve the amount of <see cref="ReactionGroup"/> for each <see cref="CommentStatus"/>.
    /// This method returns a tuple containing the <see cref="Reaction.CommentId"/>, the <see cref="CommentStatus"/> and the amount
    /// it occurs in the entire project.
    /// </summary>
    /// <param name="project">The project to return the total amounts of comment with all the <see cref="CommentStatus"/> for.</param>
    /// <returns></returns>
    public IEnumerable<CommentStatusTotal> ReadCommentsStatusAmountByProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get the amount each doc-review status occurs per project.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DocReviewStatusTotal> ReadDocReviewsStatusAmountByProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Return the amount of doc-reviews on the entire project.
    /// </summary>
    /// <returns></returns>
    public int ReadDocReviewsAmountByProject(Domain.Project.Project project);

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Return all the survey statistics by project.
    /// Returns a list with each item being a survey, and containing the survey + a list with each option and the amount of times it has been chosen on the survey.
    /// -> returns only the surveys per project.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SurveyStatistic> ReadSurveyStatisticsByProject(Domain.Project.Project project);
}