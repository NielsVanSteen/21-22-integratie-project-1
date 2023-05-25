using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.SignalR;
using UI.MVC.CloudStorage;
using UI.MVC.Identity;
using UI.MVC.Models.Hub;

namespace UI.MVC.Extensions;

/// <summary>
/// Extension method for <see cref="User"/>
/// </summary>
public static class UserExtensions
{
    private const string UserProfilePicturePrefix = "users/";

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the user's full name.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetFullName(this User user)
    {
        if (user == null)
            return "Anonymous";
        
        return $"{user.Firstname} {user.Lastname}";
    } // GetFullName.

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Check if the user is a moderator.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static bool IsModerator(this User user)
    {
        if (user == null)
            return false;
        
        return user.UserName.ToLower().Contains(ApplicationConstants.BackEndUrlName.ToLower());
    } // IsModerator.

    public static string GenerateUsrProfilePictureFileName(this User user)
    {
        return UserProfilePicturePrefix + user.Id;
    }

    /// <summary>
    /// Get the user profile picture image.
    /// </summary>
    /// <param name="user">The user to get the image for.</param>
    /// <param name="size">The image size.</param>
    /// <returns></returns>
    public static string GetUserProfilePictureImageLink(this User user, SquareImageSize size)
    {
        if (user == null || !user.HasProfilePicture)
            return ApplicationConstants.CloudStorageBasicUrl + UserProfilePicturePrefix + "default" + "_" + (int)size;
        
        return ApplicationConstants.CloudStorageBasicUrl + UserProfilePicturePrefix + user.Id + "_" + (int)size;
    } // GetUserImageLink.


    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method to change the users assigned projects while sending notifications to the user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="newProjects"></param>
    /// <param name="hubContext"></param>
    public static void ChangeRegisteredProjects(this User user, ICollection<Project> newProjects, IHubContext<DocreviewHub> hubContext)
    {
        var tmpList = user.RegisteredForProjects.ToList();
        var stringListAdded = new List<string>();
        var stringListRemoved = new List<string>();
        
        foreach (var project in newProjects)
        {
            if (!tmpList.Contains(project))
            {
                tmpList.Add(project);
                stringListAdded.Add(project.ExternalName);
            }
        }

        if (stringListAdded.Any())
        { 
            var display = string.Join(", ", stringListAdded);
            hubContext.Clients.Group(user.Email).SendCoreAsync("AddedProject",new[] {display}); 
        }
        
        foreach (var project in tmpList.ToList())
        {
            if (!newProjects.Contains(project))
            {
                tmpList.Remove(project);
                stringListRemoved.Add(project.ExternalName);
            }
        }
        
        if (stringListRemoved.Any())
        { 
            var display = string.Join(", ", stringListRemoved);
            hubContext.Clients.Group(user.Email).SendCoreAsync("RemovedProject",new[] {display}); 
        }
        
        user.RegisteredForProjects = tmpList;

    }
}