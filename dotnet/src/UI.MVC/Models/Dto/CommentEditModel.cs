using System.ComponentModel.DataAnnotations;

namespace UI.MVC.Models.Dto;

public class CommentEditModel
{
    [Required]
    public int CommentId { get; set; }
    [Required]
    public string EditedText { get; set; }
}