using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Project;

/// <author>Niels Van Steen</author>
/// <summary>
/// This class is used to store and display all the possible project styles.
/// </summary>
public class ThemeStyles
{
    // Properties.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The primary key.
    /// </summary>
    [Key]
    public int ThemeStylesId { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// What project is the style linked to? can be null -> global styles.
    /// </summary>
    public Project Project { get; set; }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// What project is the style linked to? can be null -> global styles.
    /// </summary>
    public int? ProjectId { get; set; }

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
    public ThemeStyles()
    {
    }

    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// All the different available color themes.
    /// This is placed here so it can be used in the seeding.
    /// </summary>
    [JsonIgnore]
    public static readonly IEnumerable<(string colorLight, string colorMedium, string colorDark, string colorDarkest, string genericName, string displayName)> ProjectColorStyles =
    new List<(string colorLight, string colorMedium, string colorDark, string colorDarkest, string genericName, string displayName)>()
    {
        new Tuple<string, string, string, string, string, string>("000000", "3F3F3F", "3F3F3F", "000000", "Black & White", "KdG").ToValueTuple(), // Black & White (KdG).
        new Tuple<string, string, string, string, string, string>("90A955", "7A8E49", "6A783B", "283618", "Green", "Forest Fresh").ToValueTuple(), // Green
        new Tuple<string, string, string, string, string, string>("A4DDED", "73C2FB", "54a0ff", "2e86de", "Light Blue", "Deep Sky Blue").ToValueTuple(), // Light Blue
        new Tuple<string, string, string, string, string, string>("00a8e8", "0096C7", "0077B6", "003459", "Blue", "Ocean Breeze").ToValueTuple(), // Blue
        new Tuple<string, string, string, string, string, string>("F8C537", "FF8500", "FF6000", "FF4800", "Yellow", "Summer Hot").ToValueTuple(), // Yellow
        new Tuple<string, string, string, string, string, string>("Ef3907", "D00000", "9D0208", "6A040F", "Red", "Fiery Red").ToValueTuple(), // Red
        new Tuple<string, string, string, string, string, string>("A68A64", "936639", "6F1D1B", "582F0E", "Brown", "Autumn Leaves").ToValueTuple(), // Brown
        new Tuple<string, string, string, string, string, string>("EAAC8B", "E56B6F", "B56576", "6D597A", "Pink", "Pink").ToValueTuple(), // Purple (dull)
        new Tuple<string, string, string, string, string, string>("DEFF0A", "0AFF99", "0AEFFF", "147DF5", "Neon", "Eye Burn").ToValueTuple(), // Eye burn
        new Tuple<string, string, string, string, string, string>("C05299", "973AA8", "6411AD", "47126B", "Purple", "Purple").ToValueTuple(), // Purple (brighter)
        new Tuple<string, string, string, string, string, string>("C9ADA1", "A0A083", "798478", "4D6A6D", "Saturated Grey", "Boredom Grey").ToValueTuple() // Saturated grey
    };
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The index of <see cref="ProjectColorStyles"/> that will be the default style when a project is created.
    /// </summary>
    [JsonIgnore]
    public const int DefaultProjectTheme = 2;
}