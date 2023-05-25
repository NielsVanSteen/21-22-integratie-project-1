using Domain.User;

namespace UI.MVC.Models.AnalyseComments.ExportComments;

/// <author> Niels Van Steen </author>
/// <summary>
/// Model to display the user in the comment export model.
/// <see cref="CommentExportModel"/>
/// </summary>
public class CommentExportUserModel
{
    // Properties.
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The user's id.
    /// </summary>
    public string Id { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The user's first name.
    /// </summary>
    public string Firstname { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The user's last name.
    /// </summary>
    public string Lastname { get; set; }
    
    /// <author> Niels Van Steen </author>
    /// <summary>
    /// The user's email.
    /// </summary>
    public string Email { get; set; }
    
    // Constructors.
    public CommentExportUserModel()
    {
    }

    public CommentExportUserModel(User user)
    {
        if (user == null)
            return;
        
        Id = user.Id;
        Firstname = user.Firstname;
        Lastname = user.Lastname;
        Email = user.Email;
    }
}