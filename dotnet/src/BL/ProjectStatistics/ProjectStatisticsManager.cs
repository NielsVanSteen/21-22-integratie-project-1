using System.ComponentModel.DataAnnotations;
using DAL.Repositories.ProjectStatistics;
using Domain.DocReview;
using Domain.ProjectStatistics;

namespace BL.ProjectStatistics;

/// <author> Niels Van Steen </author>
/// <summary>
/// <see cref="IProjectStatisticsManager"/>
/// </summary>
public class ProjectStatisticsManager : IProjectStatisticsManager
{
    // Fields.
    private readonly IProjectStatisticsRepository _repository;

    // Constructor.
    public ProjectStatisticsManager(IProjectStatisticsRepository repository)
    {
        _repository = repository;
    } // ProjectStatisticsManager.


    // Methods.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetReactionGroupAmountByProject"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics AddProjectStatistics(
        Domain.ProjectStatistics.ProjectStatistics projectStatistics)
    {
        Validator.ValidateObject(projectStatistics, new ValidationContext(projectStatistics), validateAllProperties: true);
        return _repository.CreateProjectStatistics(projectStatistics);
    } // AddProjectStatistics.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetProjectStatisticsByProject(Domain.Project.Project)"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> GetProjectStatisticsByProject(
        Domain.Project.Project project)
    {
        return _repository.ReadProjectStatisticsByProject(project);
    } // GetProjectStatisticsByProject.

    /// <author> Bjorn Straetemans </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetLastProjectStatisticByProject(Domain.Project.Project)"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics GetLastProjectStatisticByProject(Domain.Project.Project project)
    {
        return _repository.ReadLastProjectStatisticByProject(project);
    }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetProjectStatistics(int)"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics GetProjectStatistics(int id)
    {
        return _repository.ReadProjectStatistics(id);
    } // GetProjectStatistics.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetProjectStatisticsByProjectAndDay(int)"/>
    /// </summary>
    public Domain.ProjectStatistics.ProjectStatistics GetProjectStatisticsByProjectAndDay(
        Domain.Project.Project project, DateTime dateTime)
    {
        return _repository.ReadProjectStatisticsByProjectAndDay(project, dateTime);
    } // GetProjectStatisticsByProjectAndDay.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetProjectStatisticsByProject(Domain.Project.Project, int)"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> GetProjectStatisticsByProject(
        Domain.Project.Project project, int detailAmount)
    {
        return _repository.ReadProjectStatisticsByProject(project, detailAmount);
    } // GetProjectStatisticsByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetProjectStatisticsByProjectAndTimeFrame(Domain.Project.Project, int, DateTime, DateTime)"/>
    /// </summary>
    public IEnumerable<Domain.ProjectStatistics.ProjectStatistics> GetProjectStatisticsByProjectAndTimeFrame(
        Domain.Project.Project project, int detailAmount, DateTime beginDate,
        DateTime endDate)
    {
        return _repository.ReadProjectStatisticsByProjectAndTimeFrame(project, detailAmount, beginDate, endDate);
    } // GetProjectStatisticsByProjectAndTimeFrame.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetReactionGroupAmountByProject"/>
    /// </summary>
    public int GetReactionGroupAmountByProject(Domain.Project.Project project)
    {
        return _repository.ReadReactionGroupAmountByProject(project);
    } // GetReactionGroupAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetEmojiAmountByProject"/>
    /// </summary>
    public int GetEmojiAmountByProject(Domain.Project.Project project)
    {
        return _repository.ReadEmojiAmountByProject(project);
    } // GetEmojiAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetEmojiTypesAmountByProject"/>
    /// </summary>
    public IEnumerable<EmojiTypeTotal> GetEmojiTypesAmountByProject(
        Domain.Project.Project project)
    {
        return _repository.ReadEmojiTypesAmountByProject(project);
    } // GetEmojiTypesAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetReactionGroupAmountByProject"/>
    /// </summary>
    public int GetUsersAmountByProject(Domain.Project.Project project, string identifier)
    {
        return _repository.ReadUsersAmountByProject(project, identifier);
    } // GetUsersAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetReactionGroupAmountByProject"/>
    /// </summary>
    public int GetProjectManagersAmountByProject(Domain.Project.Project project, string identifier)
    {
        return _repository.ReadProjectManagersAmountByProject(project, identifier);
    } // GetReactionGroupAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetReactionGroupAmountByProject"/>
    /// </summary>
    public IEnumerable<CommentStatusTotal> GetCommentsStatusAmountByProject(Domain.Project.Project project)
    {
        return _repository.ReadCommentsStatusAmountByProject(project);
    } // GetCommentsStatusAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetDocReviewsStatusAmountByProject"/>
    /// </summary>
    public IEnumerable<DocReviewStatusTotal> GetDocReviewsStatusAmountByProject(Domain.Project.Project project)
    {
        return _repository.ReadDocReviewsStatusAmountByProject(project);
    } // GetDocReviewsStatusAmountByProject.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetDocReviewsAmountByProject"/>
    /// </summary>
    public int GetDocReviewsAmountByProject(Domain.Project.Project project)
    {
        return _repository.ReadDocReviewsAmountByProject(project);
    } // GetDocReviewsAmountByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="IProjectStatisticsManager.GetSurveyStatisticsByProject"/>
    /// </summary>
    public IEnumerable<SurveyStatistic> GetSurveyStatisticsByProject(Domain.Project.Project project)
    {
        return _repository.ReadSurveyStatisticsByProject(project);
    } // GetSurveyStatisticsByProject.
}