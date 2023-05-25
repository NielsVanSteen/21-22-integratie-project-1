using System.ComponentModel.DataAnnotations;
using Domain.DocReview;

namespace Domain.DocReview;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class contains a possible option for a <see cref="Survey"/>
/// </summary>
public class SurveyOption
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int SurveyOptionId { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The option text for the <see cref="Survey"/>
    /// </summary>
    [Required]
    public string Option { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Optional description explaining the option.
    /// </summary>
    public string Description { get; set; }
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The <see cref="Survey"/> the <see cref="SurveyOption"/> belongs to.
    /// </summary>
    public Survey Survey { get; set; }

    /// <summary>
    /// <see cref="Survey"/>
    /// </summary>
    public int SurveyId { get; set; }
    
    // Constructor.
    public SurveyOption() {}
}