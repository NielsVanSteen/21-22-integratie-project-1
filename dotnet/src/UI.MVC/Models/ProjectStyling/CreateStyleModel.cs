using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.ProjectStyling;

/// <author>Niels Van Steen</author>
/// <summary>
/// Model to create a new theme style.
/// </summary>
public class CreateStyleModel
{
    // Properties.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The name of the theme, e.g., 'Green', 'Blue', ..
    /// </summary>
    [Required]
    public string GenericName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// A nicer name for the style e.g., 'Warm', 'Ocean Breeze', 'Rustic', ..
    /// </summary>
    [Required]
    public string DisplayName { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The way the website is styled is using a 'monotone' color schemes with a grayscale accent.
    /// The 'monotone' color has 4 variants: light, medium, dark, darkest.
    /// 
    /// This colors represents the lightest color of the monotone palette.
    /// </summary>
    [Required]
    public string ColorLight { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ColorLight"/>
    ///
    /// This colors represents the medium color of the monotone palette.
    /// </summary>
    [Required]
    public string ColorMedium { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ColorLight"/>
    ///
    /// This colors represents the dark color of the monotone palette.
    /// </summary>
    [Required]
    public string ColorDark { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ColorLight"/>
    ///
    /// This colors represents the darkest color of the monotone palette.
    /// </summary>
    [Required]
    public string ColorDarkest { get; set; }
    
    // Constructor.
    public CreateStyleModel()
    {
    }
}