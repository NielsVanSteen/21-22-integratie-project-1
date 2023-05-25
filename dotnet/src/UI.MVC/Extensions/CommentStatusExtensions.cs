using Domain.Comment;

namespace UI.MVC.Extensions;

/// <author>Niels Van Steen</author>
/// <summary>
/// Class with extension method for <see cref="CommentStatus"/>
/// </summary>
public static class CommentStatusExtensions
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get textual values for <see cref="CommentStatus"/>
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static string ToText(this CommentStatus status)
    {
        switch (status)
        {
            case CommentStatus.Created:
            case CommentStatus.Published:
            case CommentStatus.Edited:
            case CommentStatus.Removed:
            case CommentStatus.Marked:
                return status.ToString();
            case CommentStatus.Inappropriate:
                return $"Set {CommentStatus.Inappropriate}";
            default:
                return null;
        }
    } // ToText.
}