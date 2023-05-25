using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Attributes;

/// <author>Niels Van Steen</author>
/// <summary>
/// Custom validation attribute that checks the validity of a file by looking at the extension.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    // Fields.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Static array defining the default allowed extension for images.
    /// </summary>
    public static string[] ImageDefaultAllowedExtension => new[] {".jpg", ".jpeg", ".png", ".gif", ".svg"};
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Array containing all the allowed extensions.
    /// </summary>
    private readonly string[] _extensions;
    
    // Constructor.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Constructor, the allowed extension are passed in here.
    /// </summary>
    /// <param name="extensions">
    /// Variable amount of string parameters, these are the file extensions.
    /// <remarks>
    /// It does not matter whether you include the '.' in the name. E.g., '.png' and 'png' are both valid.
    /// </remarks>
    /// </param>
    public AllowedExtensionsAttribute(params string[] extensions)
    {
        _extensions = extensions;
    } // AllowedExtensionsAttribute.
    
    // Methods.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Checks if the file extension is allowed.
    /// </summary>
    /// <param name="value">The property/field the attribute is on.</param>
    /// <param name="validationContext"><see cref="ValidationContext"/></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // If the attribute is not a file -> return success.
        if (value is not IFormFile file) 
            return ValidationResult.Success;
        
        // Check the extension.
        var extension = Path.GetExtension(file.FileName);
        if (!_extensions.Contains(extension.ToLower()))
            return new ValidationResult(GetErrorMessage());

        return ValidationResult.Success;
    } // IsValid.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Error message
    /// </summary>
    /// <returns>The error message text.</returns>
    public static string GetErrorMessage()
    {
        return $"This extension is not allowed! The allowed extension include: {string.Join(", ", AllowedExtensionsAttribute.ImageDefaultAllowedExtension)}";
    } // GetErrorMessage
}