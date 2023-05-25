using Domain.DocReview;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.DocReview;

/// <summary>
/// <see cref="ISurveyRepository"/>
/// </summary>
public class SurveyRepository : Repository, ISurveyRepository
{
    // Constructor.
    public SurveyRepository(DocReviewDbContext context) : base(context)
    {
    }

    // Methods.

    /// <summary>
    /// <see cref="ISurveyRepository.ReadUserRespondedToSurvey"/>
    /// </summary>
    public Survey ReadSurvey(int surveyId, bool includeSurveyOptions = false)
    {
        IQueryable<Survey> surveys = Context.Surveys;

        if (includeSurveyOptions)
            surveys = surveys.Include(s => s.SurveyOptions);

        return surveys.SingleOrDefault(x => x.SurveyId == surveyId);
    } // ReadSurvey.

    /// <summary>
    /// <see cref="ISurveyRepository.ReadUserRespondedToSurvey"/>
    /// </summary>
    public void CreateUserResponse(IEnumerable<UserSurveyAnswer> userSurveyAnswers)
    {
        Context.SurveyAnswers.AddRange(userSurveyAnswers);
        Context.SaveChanges();
    } // CreateUserResponse.

    /// <summary>
    /// <see cref="ISurveyRepository.ReadUserRespondedToSurvey"/>
    /// </summary>
    public bool ReadUserRespondedToSurvey(Survey survey, Domain.User.User user)
    {
        return Context.SurveyAnswers.Any(x => (x.SurveyId == survey.SurveyId && x.UserId == user.Id));
    } // ReadUserRespondedToSurvey.
}