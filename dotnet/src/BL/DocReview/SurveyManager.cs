using System.ComponentModel.DataAnnotations;
using BL.Project;
using DAL.Repositories.DocReview;
using Domain.DocReview;

namespace BL.DocReview;

/// <summary>
/// <see cref="ISurveyManager"/>
/// </summary>
public class SurveyManager : ISurveyManager
{
    // Fields.
    private ISurveyRepository _repository;

    // Constructor.
    public SurveyManager(ISurveyRepository repository)
    {
        _repository = repository;
    } // UserService.

    // Methods.

    /// <summary>
    /// <see cref="ISurveyManager.GetSurvey"/>
    /// </summary>
    public Survey GetSurvey(int surveyId, bool includeSurveyOptions = false)
    {
        return _repository.ReadSurvey(surveyId, includeSurveyOptions);
    } // GetSurvey.

    /// <summary>
    /// <see cref="ISurveyManager.AddUserResponse"/>
    /// </summary>
    public void AddUserResponse(IEnumerable<UserSurveyAnswer> userSurveyAnswers)
    {
        Validator.ValidateObject(userSurveyAnswers, new ValidationContext(userSurveyAnswers), validateAllProperties: true);
        _repository.CreateUserResponse(userSurveyAnswers);
    } // AddUserResponse.

    /// <summary>
    /// <see cref="ISurveyManager.GetUserRespondedToSurvey"/>
    /// </summary>
    public bool GetUserRespondedToSurvey(Survey survey, Domain.User.User user)
    {
        return _repository.ReadUserRespondedToSurvey(survey, user);
    } // GetUserRespondedToSurvey.
}