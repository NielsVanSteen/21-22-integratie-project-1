using Domain.DocReview;

namespace DAL.Repositories.DocReview;

/// <summary>
/// The repository interface for all the classes concerning a <see cref="Domain.DocReview.Survey"/>.
/// </summary>
/// <example>
/// The classes include:
/// <see cref="Domain.DocReview.Survey"/>
/// <see cref="Domain.DocReview.SurveyOption"/>
/// <see cref="Domain.DocReview.UserSurveyAnswer"/>
/// </example>
public interface ISurveyRepository
{
    /// <summary>
    /// Method to get a <see cref="Domain.DocReview.Survey"/> by its id.
    /// </summary>
    /// <param name="surveyId">The id of the requested <see cref="Survey"/></param>
    /// <param name="includeSurveyOptions">Wheter or not to include the navigationProperty <see cref="Survey.SurveyOptions"/></param>
    /// <returns>The found Survey or null</returns>
    Survey ReadSurvey(int surveyId, bool includeSurveyOptions = false);
    
    /// <summary>
    /// Method to add a list of <see cref="Domain.DocReview.UserSurveyAnswer"/> to the database.
    /// </summary>
    /// <param name="userSurveyAnswers"><see cref="IEnumerable{T}"/> of <see cref="UserSurveyAnswer"/> which represents the options that are selected by the user</param>
    void CreateUserResponse(IEnumerable<UserSurveyAnswer> userSurveyAnswers);

    /// <summary>
    /// Method to check if a user has already answered a survey.
    /// </summary>
    /// <param name="survey">The <see cref="Survey"/> that will be searched for</param>
    /// <param name="user">The <see cref="User"/> that will be searched for</param>
    /// <returns>True if the user has already responded to the survey, otherwise False</returns>
    public bool ReadUserRespondedToSurvey(Survey survey, Domain.User.User user);
}