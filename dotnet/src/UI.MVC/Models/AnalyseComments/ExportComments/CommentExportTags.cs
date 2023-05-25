using Domain.Comment;

namespace UI.MVC.Models.AnalyseComments.ExportComments;

/// <author> Niels Van Steen </author>
/// <summary>
/// The class that contains the tags for the export comments.
/// </summary>
public class CommentExportTags
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The tag name.
    /// </summary>
    public string Tag { get; set; }

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Is it a private or public tag?
    /// </summary>
    public string PrivatePublic { get; set; }
    
    // Constructor.
    public CommentExportTags()
    {
    }

    public CommentExportTags(CommentTag commentTag)
    {
        if (commentTag.ProjectTag == null)
            return;
        
        Tag = commentTag.ProjectTag.Name;
        PrivatePublic = commentTag.ProjectTag.IsPublic ? "Public" : "Private";
    }
}