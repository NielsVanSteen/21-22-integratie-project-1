using Domain.Comment;
using UI.MVC.Extensions;

namespace UI.MVC.Models.AnalyseComments.ExportComments;

/// <author> Niels Van Steen </author>
/// <summary>
/// The comment export model, contains all the data the export comments file will contain.
/// </summary>
public class CommentExportModel
{
    // Properties.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The comment id.
    /// </summary>
    public int Id { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The comment text.
    /// </summary>
    public string CommentText { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The text the comment was made on.
    /// </summary>
    public string SelectedText { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The user who made the comment.
    /// </summary>
    public CommentExportUserModel WrittenBy { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The tags the comment has.
    ///
    /// IMPORTANT: Xml won't serialize IEnumerable, so we this property is a list.
    /// </summary>
    public List<CommentExportTags> Tags { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The comment histories.
    /// </summary>
    public List<CommentExportHistoryModel> Histories { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// List of sub-comments.
    ///
    /// IMPORTANT: Xml won't serialize IEnumerable, so we this property is a list.
    /// </summary>
    public List<CommentExportModel> SubComments { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// List with the amount of times each emoji was used.
    ///
    /// IMPORTANT: Xml won't serialize IEnumerable, so we this property is a list.
    /// </summary>
    public List<CommentExportEmojiTotals> Emojis { get; set; }
    
    // Constructors.
    public CommentExportModel()
    {
    }

    public CommentExportModel(ReactionGroup reactionGroup)
    {
        Id = reactionGroup.CommentId;
        CommentText = reactionGroup.CommentText;
        SelectedText = reactionGroup.GetQuote();
        WrittenBy = new CommentExportUserModel(reactionGroup.User);
        Tags = reactionGroup.CommentTags.Select(tag => new CommentExportTags(tag)).ToList();
        Histories = reactionGroup.CommentHistories.Select(history => new CommentExportHistoryModel(history)).ToList();
    }

}