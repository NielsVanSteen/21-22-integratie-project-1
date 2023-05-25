using Domain.DocReview;

namespace BL.DocReview;

/// <summary>
/// The interface of the ProjectManager, used for all the classes concerning a <see cref="Domain.DocReview.Survey"/>
/// </summary>
public interface ISurveyManager
{
    /// <summary>
    /// Get a survey by its id
    /// </summary>
    /// <param name="surveyId">The id</param>
    /// <param name="includeSurveyOptions">Whether or not to include the navigation property for <see cref="Domain.DocReview.Survey.SurveyOptions"/></param>
    /// <returns></returns>
    Survey GetSurvey(int surveyId, bool includeSurveyOptions = false);
    
    /// <summary>
    /// Adds a user's response to a survey.
    /// </summary>
    /// <param name="userSurveyAnswers"></param>
    void AddUserResponse(IEnumerable<UserSurveyAnswer> userSurveyAnswers);
    
    /// <summary>
    /// Get the user's response to a survey.
    /// </summary>
    /// <param name="survey"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool GetUserRespondedToSurvey(Survey survey, Domain.User.User user);

}