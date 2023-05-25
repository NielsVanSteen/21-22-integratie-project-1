using System.Text.RegularExpressions;
using System.Web;
using Domain.Comment;

namespace UI.MVC.Extensions;

/// <author>Niels Van Steen & Sander Verheyen</author>
/// <summary>
/// Extensions for the <see cref="ReactionGroup"/>
/// </summary>
public static class CommentHistoryExtensions
{
    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// Takes all the comment histories, sort them on date en return the last one.
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static CommentStatus GetCurrentCommentStatus(this ReactionGroup reaction)
    {
        // Get all the history of the current comment.
        var commentHistory = reaction.GetLastHistory();
        if (commentHistory == null)
            return CommentStatus.Created;

        return commentHistory.CommentStatus;
    } // GetCurrentCommentStatus

    /// <author>Niels Van Steen & Sander Verheyen</author>
    /// <summary>
    /// Returns the quote in string format (without html). 
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static string GetQuote(this ReactionGroup reaction)
    {
        if (reaction.PlacedOnReactionGroupId != null)
        {
            return "@" + reaction.PlacedOnReactionGroup.User.Firstname + " " + reaction.PlacedOnReactionGroup.User.Lastname;

        }
        
        var beginChar = reaction.BeginChar ?? 0;
        var length = (reaction.EndChar ?? 0) - beginChar;
        // The Html decode makes sure there are no Html entities in the doc-review text such as &rdquo;
        var text = HttpUtility.HtmlDecode(reaction.DocReview.DocReviewText);
        text = text.Substring(beginChar, length);
        text = Regex.Replace(text, "</.*?>", " ");
        text = Regex.Replace(text, "<.*?>", string.Empty);
        return text;
    } // GetQuote

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns only the first part of the quote.
    /// </summary>
    /// <param name="reaction"></param>
    /// <param name="characters">The amount of characters starting from 0 to get the quote.</param>
    /// <param name="beginIndex">The begin index to get the characters from, default = 0</param>
    /// <returns></returns>
    public static string GetQuote(this ReactionGroup reaction, int characters, int beginIndex = 0)
    {
        var quote = reaction.GetQuote();
        var length = quote.Length;
        
        // For sub-comments the quote is the name of the commenter of the main comment -> don't take a substring of e.g., '@Niels Van Steen' but return the whole string.
        if (reaction.PlacedOnReactionGroupId != null)
            return quote;
        
        // If the length if greater than the characters, return the first characters and add an ellipsis.
        if (length > characters)
        {
            quote = quote.Substring(beginIndex, characters);
            quote += "...";
        }
        
        // Otherwise return the whole quote.
        return quote;
    } // GetQuote.

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// returns the quote with html tags.
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static string GetQuoteHtml(this ReactionGroup reaction)
    {
        if (reaction.PlacedOnReactionGroupId != null)
            return null;

        var beginChar = reaction.BeginChar ?? 0;
        /*var length = (reaction.EndChar ?? 0) - beginChar;
        var text = HttpUtility.HtmlDecode(reaction.DocReview.DocReviewText);
        var quote = text.Substring(beginChar, length);
        return quote;*/
        return reaction.DocReview.GetSelectedTextOfDocReview(beginChar, reaction.EndChar ?? 0, false);
    } // GetCurrentCommentStatus

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Checks if the last history was Edited and was done by a moderator;
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool EditedByModerator(this ReactionGroup reaction)
    {
        var lastHistory = reaction.GetLastHistory();
        return lastHistory != null &&
               (lastHistory.EditedBy.IsModerator() && lastHistory.CommentStatus == CommentStatus.Edited);
    }
    
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Returns the last history of a comment
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static CommentHistory GetLastHistory(this ReactionGroup reaction)
    {
        return reaction.GetCommentHistoriesOrderedOnDate().LastOrDefault();
    } // GetLastHistory.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get the oldest comment history.
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static CommentHistory GetFirstHistory(this ReactionGroup reaction)
    {
        return reaction.GetCommentHistoriesOrderedOnDate().FirstOrDefault();
    } // GetFirstHistory.
    
    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Get the comment history ordered on date.
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    private static IEnumerable<CommentHistory> GetCommentHistoriesOrderedOnDate(this ReactionGroup reaction)
    {
        var commentHistories = reaction.CommentHistories;
        if (commentHistories == null)
            return new List<CommentHistory>();
        
        var sortedCommentHistories = commentHistories.Where(history => history.ReactionGroupId == reaction.CommentId).ToList();
        
        // Order the list by EditedOn
        sortedCommentHistories = sortedCommentHistories.OrderBy(history => history.EditedOn).ToList();
        if (sortedCommentHistories.Count == 0)
            return new List<CommentHistory>();
        
        return sortedCommentHistories;
    } // GetCommentHistoriesOrderedOnDate.

    /// <author> Sander Verheyen </author>!
    /// <summary>
    /// Check if the reaction has any history of being published.
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool IsPublished(this ReactionGroup reaction)
    {
        // All the comment histories
        var commentHistories = reaction.CommentHistories.Where(history => history.ReactionGroupId == reaction.CommentId)
            .ToList();
        foreach (var history in commentHistories)
        {
            // Check if the history has the status published.
            if (history.CommentStatus == CommentStatus.Published)
            {
                return true;
            }
        }
        // If there was no published found.
        return false;
    }

    public static bool HasPublicTag(this ReactionGroup reaction)
    {
        return reaction.CommentTags.Any(tag => tag.ProjectTag.IsPublic);
    }
}
