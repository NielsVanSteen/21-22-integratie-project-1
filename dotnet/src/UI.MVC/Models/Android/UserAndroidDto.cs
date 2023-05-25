using Domain.User;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;


namespace UI.MVC.Models.Android;

public class UserAndroidDto
{
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The username of the <see cref="User"/>.
    /// </summary>
    public string Name { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The email of the <see cref="User"/>.
    /// </summary>
    public string Email { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The profile picture of the <see cref="User"/>.
    /// </summary>
    public string ProfilePicture { get; set; }
    
    public UserAndroidDto(){}

    public UserAndroidDto(User user)
    {
        Name = user.GetFullName();
        Email = user.Email;
        ProfilePicture = user.GetUserProfilePictureImageLink(SquareImageSize.SM);
    }
}