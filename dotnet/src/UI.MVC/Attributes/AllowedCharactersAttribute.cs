using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UI.MVC.Attributes;

/// <author>Niels Van Steen</author>
/// <summary>
/// Custom validation attribute that checks the allowed characters of a field.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class AllowedCharactersAttribute : ValidationAttribute
{
    // Enum
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The type of characters that are allowed.
    /// </summary>
    public enum AllowedCharactersOptions : byte
    {
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Only numeric characters are allowed.
        /// </summary>
        Numeric = 0,
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Only Alphabetical characters are allowed.
        /// </summary>
        Alphabetical = 1,
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Only alphanumeric values are allowed.
        /// </summary>
        AlphaNumeric = 2,
    }
    
    // Fields.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The characters that are allowed. <see cref="AllowedCharactersOptions"/>
    /// </summary>
    private readonly AllowedCharactersOptions _allowedCharactersOptions;
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// In addition to the allowed characters <see cref="_allowedCharactersOptions"/> the characters given in this array are not seen as illegal characters.
    /// </summary>
    /// <remarks>
    /// E.g., Given the options <see cref="AllowedCharactersOptions.Numeric"/> and <see cref="_charactersToSkip"/> contains 'a'.
    /// Then only numeric values AND the letter 'a' are allowed.
    /// </remarks>
    private readonly char[] _charactersToSkip;
    
    // Constructor.
    public AllowedCharactersAttribute(AllowedCharactersOptions allowedCharactersOptions)
    {
        _allowedCharactersOptions = allowedCharactersOptions;
        _charactersToSkip = null;
    } // MaxFileSizeAttribute
    
    public AllowedCharactersAttribute(AllowedCharactersOptions allowedCharactersOptions, params char[] charactersToSkip)
    {
        _allowedCharactersOptions = allowedCharactersOptions;
        _charactersToSkip = charactersToSkip;
    } // MaxFileSizeAttribute
    
    // Methods.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Validate the string based on the allowed characters.
    /// </summary>
    /// <returns></returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Check the type.
        if (value is not string stringValue) 
            return ValidationResult.Success;
        
        // Remove the excluded characters.
        foreach (var character in _charactersToSkip)
            stringValue = stringValue.Replace(char.ToString(character), String.Empty);

        // Check whether the string passed the validation.
        var result = true;
        switch (_allowedCharactersOptions)
        {
            case AllowedCharactersOptions.Numeric:
                result = stringValue.All(char.IsNumber);
                break;
            case AllowedCharactersOptions.Alphabetical:
                result = stringValue.All(char.IsLetter);
                break;
            case AllowedCharactersOptions.AlphaNumeric:
                result = stringValue.All(char.IsLetterOrDigit);
                break;
        }
        
        // Return the correct result.
        return result ? ValidationResult.Success : new ValidationResult(GetErrorMessage());
    } // IsValid.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Error message
    /// </summary>
    /// <returns>The error message text.</returns>
    public string GetErrorMessage()
    {
        var optional = "";
        if (_charactersToSkip.Any())
            optional = $"and {string.Join(" , ", _charactersToSkip)}";
        
        // Take the enum value, replace each capital letter with a space and the lowercase variant.
        var name = Regex.Replace(_allowedCharactersOptions.ToString(), @"(?<!_)([A-Z])", " $1");
        
        return $"The name can only contain {name.ToLower()} characters {optional}";
    } // GetErrorMessage.
}