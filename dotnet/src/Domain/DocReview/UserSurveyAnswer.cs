using System.ComponentModel.DataAnnotations;

namespace Domain.DocReview;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class contains a <see cref="Survey"/> answer. Answered by a <see cref="User"/>
/// </summary>
public class UserSurveyAnswer
{
    // Properties. 

    [Key]
    public int UserSurveyAnswerId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="User"/> who answered the survey.
    /// </summary>
    public User.User User { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="User"/>
    /// </summary>
    public string UserId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="Survey"/> the answer belongs to.
    /// </summary>
    [Required]
    public Survey Survey { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="Survey"/>
    /// </summary>
    public int SurveyId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The option the user chose.
    /// </summary>
    [Required]
    public SurveyOption ChosenOption { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ChosenOptionId"/>
    /// </summary>
    public int ChosenOptionId { get; set; }


    // Constructor.
    public UserSurveyAnswer() { }
}