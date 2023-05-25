using Domain.Comment;

namespace UI.MVC.Models.AnalyseComments.ExportComments;

/// <author> Niels Van Steen </author>
/// <summary>
/// The history model for the <see cref="CommentExportModel"/>
/// </summary>
public class CommentExportHistoryModel
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The date the status was edited.
    /// </summary>
    public DateTime EditedOn { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The status.
    /// </summary>
    public string Status { get; set; }
    
    // Constructor.
    public CommentExportHistoryModel()
    {
    }

    public CommentExportHistoryModel(CommentHistory commentHistory)
    {     
        EditedOn = commentHistory.EditedOn;
        Status = commentHistory.CommentStatus.ToString();
    }
}