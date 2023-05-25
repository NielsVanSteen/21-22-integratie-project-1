using System.Text.RegularExpressions;
using System.Web;
using Domain.DocReview;
using Microsoft.AspNetCore.Html;
using UI.MVC.CloudStorage;
using UI.MVC.Identity;

namespace UI.MVC.Extensions;

/// <author> Michiel Verschueren </author>
/// <summary>
/// Class that holds extension methods for type <see cref="DocReview"/>
/// </summary>
public static class DocReviewExtensions
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Prefix that is added to each banner image
    /// </summary>
    private const string DocReviewBannerImagePrefix = "docreview-banners/";

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method that is called to get a filename to be used for
    /// the upload of the banner image of a <see cref="DocReview"/>
    /// </summary>
    /// <param name="docReview">The <see cref="DocReview"/> id is used in the filename</param>
    /// <returns>The full image filename to be uploaded to google cloud</returns>
    public static string GetBannerImageFileName(this DocReview docReview)
    {
        return DocReviewBannerImagePrefix + docReview.DocReviewId;
    } // GetBannerImageFileName.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Return the entire link to a doc-review banner image.
    /// </summary>
    /// <param name="docReview"></param>
    /// <returns></returns>
    public static string GetBannerImageLink(this DocReview docReview, LandscapeImageSize size)
    {
        return ApplicationConstants.CloudStorageBasicUrl + docReview.GetBannerImageFileName() + "_" + (int)size;
    } // GetBannerImageLink.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the latest doc-review history.
    /// </summary>
    /// <param name="docReview"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static DocReviewHistory GetLatestStatus(this DocReview docReview)
    {
        if (!docReview.DocReviewHistories.Any())
            throw new NullReferenceException();

        return docReview.DocReviewHistories.OrderBy(d => d.EditedOn).ToList()[^1];
    } // GetLatestStatus.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Returns the latest <see cref="DocReviewHistory"/> of a <see cref="DocReview"/>
    /// </summary>
    /// <param name="docReview">The DocReview ot get the DocReviewHistory for</param>
    /// <returns></returns>
    public static DocReviewHistory GetLatestProjectHistory(this DocReview docReview)
    {
        return docReview.DocReviewHistories.MaxBy(p => p.EditedOn);
    }

    /// <author>Niels Van Steen </author>
    /// <summary>
    /// returns if the doc-review is visible for normal users.
    /// </summary>
    /// <returns></returns>
    public static bool IsDocReviewVisibleForNormalUsers(this DocReview docReview)
    {
        return new List<DocReviewStatus>()
        {
            DocReviewStatus.Published,
            DocReviewStatus.Closed
        }.Contains(docReview.GetLatestStatus().DocReviewStatus);
    } // IsDocReviewVisibleForNormalUsers.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Returns the docreview text as HtmlString in order to render it on the page
    /// </summary>
    /// <param name="docReview">The DocReview to get the Text for</param>
    /// <returns></returns>
    public static HtmlString GetContentAsHtmlString(this DocReview docReview)
    {
        return new HtmlString(HttpUtility.HtmlDecode(docReview.DocReviewText));
    }

    public static string GetTextWithoutHtml(this DocReview docReview)
    {
        var text = Regex.Replace(docReview.DocReviewText, "</.*?>", " ");
        return Regex.Replace(text, "<.*?>", String.Empty);
    }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// struct that is used to pass the begin and end char of a found string in the docReview content
    /// </summary>
    public struct Indexes
    {
        public int BeginChar;
        public int EndChar;
    }
    
    /// <author> Sander Verheyen and Michiel Verschueren </author>
    /// <summary>
    /// Method to get the beginchar en the endchar of the quote from the docreviewContent
    /// </summary>
    /// <param name="docReview">the current <see cref="DocReview"/></param>
    /// <param name="searchTerm">the string that is searched for in the DocReview Content</param>
    /// <returns>a <see cref="Indexes"/> with the values for the begin and end char</returns>
    /// <exception cref="ArgumentException">When the <see cref="searchTerm"/> is not found in the docReview content</exception>
    public static Indexes GetIndexes(this DocReview docReview, string searchTerm)
    {
        var quote = searchTerm.Trim();
        quote = HttpUtility.HtmlDecode(quote);
        // When a quote has been selected this makes sure there are
        // no starting or ending html tags that prevent the string from being found.
        var regexStart = new Regex("(^(<[a-z].*?>){1,})(.*)");
        var regexEnd = new Regex("((>.*?/<)+)(.*)");
        
        if (regexStart.IsMatch(quote))
        {
            quote = regexStart.Match(quote).Groups[3].ToString();
            quote = string.Join("", quote.Reverse());
        }
        if (regexEnd.IsMatch(quote))
        {
            quote = regexEnd.Match(quote).Groups[3].ToString();
            quote = string.Join("", quote.Reverse());
        }
        var regexGoogleDocs = new Regex("=\"\"");
        var regexFont = new Regex("family:\" ");
        if (regexGoogleDocs.IsMatch(quote))
        {
            quote = regexGoogleDocs.Replace(quote, "");
        }

        if (regexFont.IsMatch(quote))
        {
            quote = regexFont.Replace(quote, "family:\"");
        }

        var text = HttpUtility.HtmlDecode(docReview.DocReviewText).ToLower();
        var beginchar = text.IndexOf(quote.ToLower(), StringComparison.Ordinal);
        var endchar = beginchar + quote.Length;
        
        var indexes = new Indexes()
        {
            BeginChar = beginchar,
            EndChar = endchar
        };
        // The Html decode makes sure there are no Html entities in the doc-review text such as &rdquo;
        if (indexes.BeginChar == -1)
        {
            throw new ArgumentException("De quote werd niet gevonden.");
        }

        return indexes;
    }
    
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Return the doc-review text given the begin and end char.
    /// </summary>
    public static string GetSelectedTextOfDocReview(this DocReview docReview, int beginChar, int endChar, bool normal)
    {
         var length = endChar - beginChar;
        
        // The Html decode makes sure there are no Html entities in the doc-review text such as &rdquo;
        var text = HttpUtility.HtmlDecode(docReview.DocReviewText);
        text = text.Substring(beginChar, length);
        if (!normal) return text;
        text = Regex.Replace(text, "</.*?>", " ");
        text = Regex.Replace(text, "<.*?>", string.Empty);
        return text;
    } // GetSelectedTextOfDocReview.
}