using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Attributes;

/// <author>Niels Van Steen</author>
/// <summary>
/// Custom validation attribute that checks the validity of a file by looking at the file size.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class MaxFileSizeAttribute : ValidationAttribute
{
    // Fields.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Maximum file size im mega bytes for normal files.
    /// </summary>
    public const int DefaultMaxFileSizeInBytes = 5 * 1000 * 1000;
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The maximum file size in <b>Mega Bytes</b>
    /// </summary>
    private readonly decimal _maxFileSize;
    
    // Constructor.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="MaxFileSizeAttribute"/>
    /// </summary>
    /// <param name="maxFileSize">The maximum file size in <b>Mega bytes</b></param>
    public MaxFileSizeAttribute(int maxFileSize)
    {
        // Convert bytes to megabytes.
        var sizeInMegaBytes = (decimal)maxFileSize * 1000 * 1000;
        
        _maxFileSize = sizeInMegaBytes;
    } // MaxFileSizeAttribute.

    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Checks if the file size does not exceed the maximum size.
    /// </summary>
    /// <param name="value">The property/field the attribute is on.</param>
    /// <param name="validationContext"><see cref="ValidationContext"/></param>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) 
            return ValidationResult.Success;
        
        
        return file.Length > _maxFileSize ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
    } // IsValid.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Error message
    /// </summary>
    /// <returns>The error message text.</returns>
    public string GetErrorMessage()
    {
        return $"Maximum allowed file size is { _maxFileSize / 1000 / 1000} MB.";
    } // GetErrorMessage.
}