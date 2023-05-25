using System.ComponentModel.DataAnnotations;
using Domain.DocReview;
using Domain.User;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;

namespace UI.MVC.Models.Android;

public class DocReviewAndroidDto
{
    //Properties
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Id.
    /// </summary>
    [Key]
    public int DocReviewId { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// All the available emoji's
    /// </summary>
    public List<string> EmojiCodes { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// This is the name of a  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Short text that explains what this  <see cref="DocReview"/> is about.
    /// </summary>
    [Required]
    public string Description { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// This is the body/content of the  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public string DocReviewText { get; set; }

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Who has written the  <see cref="DocReview"/>.
    /// </summary>
    [Required]
    public string WrittenBy { get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The status of the <see cref="DocReview"/>.
    /// </summary>
    public string Status{ get; set; }
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// The Banner of the <see cref="DocReview"/>.
    /// </summary>
    public string Banner{ get; set; }
    
    //Constructor
    public DocReviewAndroidDto(Domain.DocReview.DocReview docReview)
    {
        DocReviewId = docReview.DocReviewId;
        EmojiCodes = docReview.AvailableEmoji.Select(emoji =>  emoji.Code).ToList();
        Name = docReview.Name;
        Description = docReview.Description;
        DocReviewText = docReview.DocReviewText;
        WrittenBy = docReview.WrittenBy.Firstname + " " + docReview.WrittenBy.Lastname;
        Status = docReview.DocReviewHistories.OrderBy(dh => dh.EditedOn).Last().DocReviewStatus.ToString();
        Banner = docReview.GetBannerImageLink(LandscapeImageSize.MD);
    } 
}