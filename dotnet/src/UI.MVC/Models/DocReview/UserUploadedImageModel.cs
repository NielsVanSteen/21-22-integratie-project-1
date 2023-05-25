
using UI.MVC.Attributes;

namespace UI.MVC.Models.DocReview;

/// <author> Michiel Verschueren </author>
/// <summary>
/// model to upload a image to google cloud
/// </summary>
public class UserUploadedImageModel
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The image to be uploaded
    /// </summary>
    [MaxFileSize(5)]
    [AllowedExtensions(".png", ".jpg", ".jpeg", ".svg", ".gif")]
    public IFormFile UserUploadedImage { get; set; }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// The path to the image which is provided to the user to be used
    /// </summary>
    public string ImagePath { get; set; }
}