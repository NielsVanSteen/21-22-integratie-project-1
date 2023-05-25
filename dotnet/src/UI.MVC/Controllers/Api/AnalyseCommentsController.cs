using System.Text.Json;
using System.Xml;
using BL.Comment;
using BL.DocReview;
using BL.Project;
using Domain.Comment;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using ServiceStack.Text;
using UI.MVC.Identity;
using UI.MVC.Models.AnalyseComments;
using UI.MVC.Models.AnalyseComments.ExportComments;
using UI.MVC.Models.Dto;
using JsonSerializer = System.Text.Json.JsonSerializer;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace UI.MVC.Controllers.Api;

/// <author> Sander Verheyen</author>
/// <summary>
/// Counterpart of the <see cref="AnalyseCommentController"/> for web API.
/// </summary>
[ApiController]
[Route("/api/{project}/[controller]")]
[Authorize(Policy = ApplicationConstants.IsModerator)]
public class AnalyseCommentsController : Controller
{
    // Fields.
    private readonly ICommentManager _commentManager;
    private readonly IProjectManager _projectManager;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _hostingEnvironment;

    // Constructor.
    public AnalyseCommentsController(UserManager<User> userManager, ICommentManager commentManager,
        IProjectManager projectService, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _commentManager = commentManager;
        _projectManager = projectService;
        _hostingEnvironment = environment;
    }

    // Methods.
    private async Task<IEnumerable<CommentExportModel>> GenerateExportCommentDtos(AnalyseCommentsFilterModel commentsFilterModel)
    {
         // Get Comments.
        var project = _projectManager.GetProjectByExternalName(ApplicationConstants.GetProjectName(RouteData));
        var user = await _userManager.GetUserAsync(User);
        
        // the lists came in a string separated by a comma. -> split them.
        commentsFilterModel.CommentStatus = commentsFilterModel.CommentStatus.FirstOrDefault()?.Split(',').ToList();
        commentsFilterModel.DocReviews = commentsFilterModel.DocReviews.FirstOrDefault()?.Split(',').ToList();
        commentsFilterModel.ProjectTags = commentsFilterModel.ProjectTags.FirstOrDefault()?.Split(',').ToList();
        
        // Create the domain layer filter model. and get all the comments.
        var filterModel = commentsFilterModel.ToCommentsFilterModel();
        var comments = _commentManager.GetCommentsOfProject(project.ProjectId, filterModel, true, true, true, true, true, true).ToList();

        // Convert comments into dto objects.
        var exportComments = new List<CommentExportModel>();

        foreach (var comment in comments.Where(c => c.PlacedOnReactionGroupId == null))
        {
            // Comment.
            var exportComment = new CommentExportModel(comment);
            exportComments.Add(exportComment);
            exportComment.Emojis = _commentManager.GetEmojisOfComment(comment.CommentId, user).Select(t => new CommentExportEmojiTotals(t.EmojiCode, t.EmojiCode, t.Count)).ToList();
            
            // Sub-comment.
            var subComments = _commentManager.GetSubCommentsByComment(comment.CommentId, true, true, true, true, true).ToList();
            exportComment.SubComments = subComments.Select(c => new CommentExportModel(c)).ToList();
            exportComment.SubComments.ToList().ForEach(c => c.Emojis = _commentManager.GetEmojisOfComment(c.Id, user).Select(t => new CommentExportEmojiTotals(t.EmojiCode, t.EmojiCode, t.Count)).ToList());
        } // end foreach.
        
        return exportComments;
    } // GenerateExportCommentDtos.


    [HttpGet("ExportCommentsJson")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ExportCommentsJson([FromQuery]AnalyseCommentsFilterModel commentsFilterModel)
    {
        // Generate the comment dtos.
        var exportComments = (await GenerateExportCommentDtos(commentsFilterModel)).ToList();
        
        // Return the comments as a json file.
        string json = JsonSerializer.Serialize(exportComments, new JsonSerializerOptions { WriteIndented = true });
        var fs = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(json));
        
        // Return as a file
        return File(fs, "application/octet-stream", "comments.json");
    } // ExportCommentsJson.
    
    [HttpGet("ExportCommentsXml")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ExportCommentsXml([FromQuery]AnalyseCommentsFilterModel commentsFilterModel)
    {
        // Generate the comment dtos.
        var exportComments = (await GenerateExportCommentDtos(commentsFilterModel)).ToList();
        
        // Create the serializer and the writer objects.
        using var stringWriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(List<CommentExportModel>));
        using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
       
        // Serialize the comments.
        serializer.Serialize(xmlWriter, exportComments);
        var xml = stringWriter.ToString();
        
        // Create a memory stream.
        var fs = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(xml));
        
        // Return as a file.
        return File(fs, "application/octet-stream", "comments.xml");
    } // ExportCommentsXml.
    
    [HttpGet("ExportCommentsCsv")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ExportCommentsCsv([FromQuery]AnalyseCommentsFilterModel commentsFilterModel)
    {
        // Generate the comment dtos.
        var exportComments = (await GenerateExportCommentDtos(commentsFilterModel)).ToList();
        
        // Serialize to csv
        var csv = CsvSerializer.SerializeToCsv(exportComments); // Nuget package: servicestack.text

        // Create a memory stream.
        var fs = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(csv));
        
        // Return as a file.
        return File(fs, "application/octet-stream", "comments.csv");
    } // ExportCommentsCsv.
    
}