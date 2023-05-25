using System.Text.RegularExpressions;
using System.Web;
using Domain.DocReview;

namespace UI.MVC.Extensions;

public static class SurveyExtension
{
    /// <author>Niels Van Steen & Sander Verheyen & Michiel Verschueren</author>
    /// <summary>
    /// Returns the quote in string format (without html). 
    /// </summary>
    /// <param name="survey"></param>
    /// <returns></returns>
    public static string GetQuote(this Survey survey)
    {
        var beginChar = survey.BeginChar;
        var length = survey.EndChar - beginChar;
        var text = HttpUtility.HtmlDecode(survey.DocReview.DocReviewText);
        text = text.Substring(beginChar, length);
        text = Regex.Replace(text, "</.*?>", " ");
        text = Regex.Replace(text, "<.*?>", string.Empty);
        return text;
    } // GetQuote
    
     /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns only the first part of the quote.
    /// </summary>
    /// <param name="survey"></param>
    /// <param name="characters">The amount of characters starting from 0 to get the quote.</param>
    /// <param name="beginIndex">The begin index to get the characters from, default = 0</param>
    /// <returns></returns>
    public static string GetQuote(this Survey survey, int characters, int beginIndex = 0)
    {
        var quote = survey.GetQuote();
        var length = quote.Length;

        // If the length if greater than the characters, return the first characters and add an ellipsis.
        if (length > characters)
        {
            quote = quote.Substring(beginIndex, characters);
            quote += "...";
        }
        
        // Otherwise return the whole quote.
        return quote;
    } // GetQuote.
    
    /// <author>Sander Verheyen & Michiel Verschueren</author>
    /// <summary>
    /// returns the quote with html tags.
    /// </summary>
    /// <param name="survey"></param>
    /// <returns></returns>
    public static string GetQuoteHTML(this Survey survey)
    { 
        var beginChar = survey.BeginChar;
        var length = survey.EndChar - beginChar;
        var text = HttpUtility.HtmlDecode(survey.DocReview.DocReviewText);
        var quote = text.Substring(beginChar, length);
        return quote;
    } // GetCurrentCommentStatus

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// returns the type of a option input button depending on <see cref="Survey.AreMultipleOptionsAllowed"/>
    /// </summary>
    /// <param name="survey"></param>
    /// <returns>"Checkbox" or "radio"</returns>
    public static string getInputType(this Survey survey)
    {
        return survey.AreMultipleOptionsAllowed ? "checkbox" : "radio";
    }
    
}