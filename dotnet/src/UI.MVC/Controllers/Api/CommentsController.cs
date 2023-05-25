using System.Text.RegularExpressions;
using System.Web;
using BL.Comment;
using BL.DocReview;
using BL.Project;
using BL.User;
using Domain.Comment;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Android;
using UI.MVC.Models.Dto;
using UI.MVC.Models.Hub;
using UserCommentsFilterModel = UI.MVC.Models.DocReview.UserCommentsFilterModel;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/{project}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ICommentManager _commentManager;
    private readonly IUserManager _userService;
    private readonly IDocReviewManager _docReviewManager;
    private readonly ICommentHistoryManager _commentHistoryManager;
    private readonly ICommentTagManager _commentTagManager;
    private readonly IProjectManager _projectManager;
    private readonly IEmojiManager _emojiManager;
    private readonly IHubContext<DocreviewHub> _hubContext;

    public CommentsController(UserManager<User> userManager, ICommentManager commentManager,
        IDocReviewManager docReviewManager, IUserManager userService, ICommentHistoryManager commentHistoryManager,
        ICommentTagManager commentTagManager, IProjectManager projectManager, IHubContext<DocreviewHub> hubContext, IEmojiManager emojiManager)
    {
        _userManager = userManager;
        _commentManager = commentManager;
        _docReviewManager = docReviewManager;
        _userService = userService;
        _commentHistoryManager = commentHistoryManager;
        _commentTagManager = commentTagManager;
        _projectManager = projectManager;
        _hubContext = hubContext;
        _emojiManager = emojiManager;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var comment = _commentManager.GetCommentById(id, true, true, false, false, true);
        return Ok(new CommentDto(comment));
    } // Get.

    [HttpGet("GetDetailComment/{id}")]
    public async Task<IActionResult> GetDetailComment(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var comment = _commentManager.GetCommentById(id, true, true, true, true, true);
        var emojis = _commentManager.GetEmojisOfComment(id, user).ToList();
        var emojiMap = emojis
            .Select(e => new KeyValuePair<string, int>(e.EmojiCode, e.Count))
            .ToDictionary(d => d.Key, d => d.Value);
        return Ok(new ViewCommentDto(comment, emojiMap));
    } // Get.

    [HttpGet("GetByDocReview/{id}")]
    public IActionResult GetByDocReview(int id)
    {
        var comments = _commentManager.GetCommentsByDocReview(id, false, false, false, true).AsEnumerable();
        var commentDtos = new List<CommentDto>();

        foreach (var c in comments)
            commentDtos.Add(new CommentDto(c));

        return Ok(commentDtos);
    } // GetByDocReview.


    /// <author> Bjorn Straetemans</author>
    /// <summary>
    /// Method to get all comments (no emojis) with its subcomments (no emojis)
    /// </summary>
    /// <returns>an list with commentDto's</returns>
    [HttpGet("GetCommentByDocReviewAndroid/{id:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> GetCommentByDocReviewAndroid(int id)
    {
        var comments = _commentManager.GetCommentsByDocReview(id, true, false, true, true, true, true).ToList();
        var user = await _userManager.GetUserAsync(User);

        var commentDtos = new List<CommentAndroidDto>();
        foreach (var c in comments)
        {
            //Check if the comment is a main comment
            if (c.PlacedOnReactionGroupId == null)
            {
                //Check if the main comment has reactions
                if (c.Reactions != null)
                {
                    // Add ALL emoji reactions of the main comment to the Dto.
                    var emojiReactions = _commentManager.GetEmojisOfComment(c.CommentId, user).ToList();
                    //var emojiReactions = _commentManager.GetEmojiReactionsByComment(c.CommentId).ToList();
                    if (emojiReactions.Any())
                    {
                        var emojiMap = emojiReactions
                            .Select(e => new KeyValuePair<string, int>(e.EmojiCode, e.Count))
                            .ToDictionary(d => d.Key, d => d.Value);
                        //Add the reaction to the comment dto list with the emoji codes
                        commentDtos.Add(new CommentAndroidDto(c, emojiMap));
                    }

                    // Comment has no reactions -> just create a comment dto.
                    else
                    {
                        //Add the reaction to the comment dto list
                        commentDtos.Add(new CommentAndroidDto(c));
                    }

                    //Check if the main comment has normal reactions
                    var subComments = _commentManager.GetSubCommentsByComment(c, null , null, includeUser: true, includeHistory: true, includePlacedOnComment: true, includeReactions: true, includeDocReview:true).ToList();
                    if (subComments.Any())
                    {
                        //Add the reaction to the comment dto list
                        foreach (var sc in subComments)
                        {
                            commentDtos.Add(new CommentAndroidDto(sc));
                        }
                    }
                }
            }
        }

        return Ok(commentDtos);
    } // GetCommentByDocReviewAndroid.
    
    [HttpGet("GetByUser/{id:int}")]
    public IActionResult GetByUser(string id)
    {
        var comments = _commentManager.GetCommentsByUser(id).AsEnumerable();

        var commentDtos = new List<CommentDto>();
        foreach (var c in comments)
            commentDtos.Add(new CommentDto(c));

        return Ok(commentDtos);
    } // GetByUser.

    /// <summary>
    /// Returns the reactions of a specific user on a specific doc-review
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="docReviewId"></param>
    /// <returns></returns>
    [HttpGet("GetByUserAndDocReview/{userId}/{docReviewId:int}")]
    public IActionResult GetByUserAndDocReview(string userId, int docReviewId)
    {
        var comments = _commentManager.GetCommentsByUserAndDocReview(userId, docReviewId, true).AsEnumerable();

        var commentDtos = new List<CommentDto>();
        foreach (var c in comments)
            commentDtos.Add(new CommentDto(c));

        return Ok(commentDtos);
    } // GetByUserAndDocReview.

    /// <summary>
    /// Returns all sub-comments given the comment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetReactionsOfCommentByComment/{id}")]
    public IActionResult GetReactionsOfCommentByComment(int id)
    {
        var comments = _commentManager.GetSubCommentsByComment(id).AsEnumerable();
        var commentDtos = comments.Select(c => new CommentDto(c));

        return Ok(commentDtos);
    } // GetReactionsOfCommentByComment.

    /// <author> Bjorn Straetemans</author>
    /// <summary>
    /// Method to set comment status on deleted
    /// </summary>
    /// <returns></returns>
    [HttpPost("CreateCommentDeletedHistory")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> CreateCommentDeletedHistory([FromBody] int id)
    {
        var reaction = _commentManager.GetCommentById(id);
        if (reaction.CommentHistories != null && reaction.GetCurrentCommentStatus() == CommentStatus.Removed)
        {
            return BadRequest("Deze comment is verwijderd");
        }

        var comment = _commentManager.GetCommentById(id);
        var user = await _userManager.GetUserAsync(User);
        if (comment == null)
        {
            return NotFound("De comment werd niet gevonden.");
        }

        var history = new CommentHistory()
        {
            EditedById = user.Id,
            CommentStatus = CommentStatus.Removed,
            EditedOn = DateTime.Now,
            ReactionGroupId = id
        };
        _commentHistoryManager.AddCommentHistory(history);
        return Ok("Removed");

    } // SetStatusDeleted.

    /// <author> Bjorn Straetemans</author>
    /// <summary>
    /// Method to edit comment and set status on edit
    /// </summary>
    /// <returns></returns>
    [HttpPut("UpdateCommentEditHistory")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> UpdateCommentEditHistory([FromBody] CommentEditModel commentEditModel)
    {

        if (string.IsNullOrEmpty(commentEditModel.EditedText))
        {
            return BadRequest("De comment moet een reactie hebben.");
        }
        if (_commentManager.GetCommentById(commentEditModel.CommentId).GetCurrentCommentStatus() ==
            CommentStatus.Removed)
        {
            return BadRequest("Deze comment is verwijderd");
        }

        var comment = _commentManager.GetCommentById(commentEditModel.CommentId);
        var user = await _userManager.GetUserAsync(User);
        if (comment == null)
        {
            return NotFound("De comment werd niet gevonden.");
        }

        if (commentEditModel.EditedText.Equals(comment.CommentText))
        {
            return BadRequest("De text was niet veranderd.");
        }

        var history = new CommentHistory()
        {
            EditedById = user.Id,
            CommentStatus = CommentStatus.Edited,
            EditedOn = DateTime.Now,
            ReactionGroupId = commentEditModel.CommentId
        };
        comment.CommentText = commentEditModel.EditedText;
        _commentHistoryManager.AddCommentHistory(history);
        _commentManager.ChangeComment(comment);
        return Ok("Edited");

    } // SetStatusDeleted.

    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Method to set a comment history to published
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("CreateCommentPublishHistory")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> CreateCommentPublishHistory([FromBody] int id)
    {
        var comment = _commentManager.GetCommentById(id);
        if (comment.GetCurrentCommentStatus() == CommentStatus.Removed)
        {
            return BadRequest("Deze comment is verwijderd");
        }

        var user = await _userManager.GetUserAsync(User);
        if (comment == null)
        {
            return NotFound();
        }

        var history = new CommentHistory()
        {
            EditedById = user.Id,
            CommentStatus = CommentStatus.Published,
            EditedOn = DateTime.Now,
            ReactionGroupId = id
        };
        _commentHistoryManager.AddCommentHistory(history);
        return Ok("Published");

    } // SetStatusPublished

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Method to add a tag to a Comment
    /// </summary>
    /// <param name="commentTagDto"></param>
    /// <returns></returns>
    [HttpPost("UpdateTag")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> UpdateTag([FromBody] CommentTagDto commentTagDto)
    {
        if (_commentManager.GetCommentById(commentTagDto.ReactionGroupId).GetCurrentCommentStatus() ==
            CommentStatus.Removed)
        {
            return BadRequest("Deze comment is verwijderd");
        }

        var user = await _userManager.GetUserAsync(User);
        var newTag = new CommentTag()
        {
            PlacedByUser = user,
            ReactionGroupId = commentTagDto.ReactionGroupId,
            ProjectTagId = commentTagDto.ProjectTagId
        };
        var commentTag = _commentTagManager.AddCommentTag(newTag);
        if (commentTag == null)
        {
            return NotFound("De comment tag werd niet gevonden");
        }

        return NoContent();

    } // UpdateTag.

    /// <author>Sander Verheyen</author>
    /// <summary>
    /// Method to remove a tag from a Comment
    /// </summary>
    /// <param name="commentTagDto"></param>
    /// <returns></returns>
    [HttpDelete("DeleteTag")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> DeleteTag([FromBody] CommentTagDto commentTagDto)
    {
        if (_commentManager.GetCommentById(commentTagDto.ReactionGroupId).GetCurrentCommentStatus() ==
            CommentStatus.Removed)
        {
            return BadRequest("Deze comment is verwijderd");
        }

        var user = await _userManager.GetUserAsync(User);
        var removeTag = new CommentTag()
        {
            PlacedByUser = user,
            ReactionGroupId = commentTagDto.ReactionGroupId,
            ProjectTagId = commentTagDto.ProjectTagId
        };
        var commentTag = _commentTagManager.RemoveCommentTag(removeTag);
        if (commentTag == null)
        {
            return NotFound("De comment tag werd niet gevonden");
        }

        return NoContent();
    }

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="commentDto"></param>
    /// <returns></returns>
    [HttpPost("CreateComment")]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        var user = await _userManager.GetUserAsync(User);
        int? beginChar;
        int? endChar;
        if (user == null)
        {
            return BadRequest("Je moet ingelogd zijn om een reactie te kunnen plaatsen");
        }

        if (string.IsNullOrEmpty(commentDto.CommentText))
        {
            return BadRequest("Gelieve een reactie in te geven.");
        }

        if (string.IsNullOrEmpty(commentDto.Quote) && commentDto.PlacedOnReactionId == null)
        {
            return BadRequest("Gelieve een quote te selecteren");
        }

        var docReview = _docReviewManager.GetDocReview(commentDto.DocReviewId);
        try
        {
            var indexes = docReview.GetIndexes(commentDto.Quote);
            beginChar = indexes.BeginChar;
            endChar = indexes.EndChar;
        }
        catch (Exception)
        {
            return BadRequest("Gelieve een geldige quote te selecteren");
        }

        // Create a new reactiongroup.
        var comment = new ReactionGroup()
        {
            DocReviewId = commentDto.DocReviewId,
            UserId = user.Id,
            BeginChar = beginChar,
            EndChar = endChar,
            CommentText = commentDto.CommentText,
            PlacedOnReactionGroupId = commentDto.PlacedOnReactionId
        };
        // Add the comment to the database.
        comment = _commentManager.AddComment(comment);

        var createdHistory = new CommentHistory()
        {
            CommentStatus = CommentStatus.Created,
            ReactionGroupId = comment.CommentId,
            EditedOn = DateTime.Now,
            EditedById = user.Id
        };
        _commentHistoryManager.AddCommentHistory(createdHistory);

        // If the docreview is postmoderated instantly publish the comment.
        if (docReview.DocReviewSettings.IsPostModerated)
        {
            var history = new CommentHistory()
            {
                CommentStatus = CommentStatus.Published,
                ReactionGroupId = comment.CommentId,
                EditedOn = DateTime.Now,
                EditedById = user.Id
            };
            _commentHistoryManager.AddCommentHistory(history);
        }

        // Return the fullname, commentId and ReactionGroupId.
        var fullname = user.Firstname + " " + user.Lastname;
        return Ok(new
        {
            Fullname = fullname,
            Id = comment.CommentId,
            PlacedOn = comment.PlacedOnReactionGroupId,
            SubCommenting = comment.DocReview.DocReviewSettings.IsSubCommentingAllowed,
            Emojis = comment.DocReview.DocReviewSettings.AreEmojisAllowed,
        });
    }

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Create a new commentHistory with status marked.
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpPost("ReportComment")]
    [Authorize]
    public async Task<IActionResult> ReportComment([FromBody] int commentId)
    {
        // Get user
        var user = await _userManager.GetUserAsync(User);
        var name = ApplicationConstants.GetProjectName(RouteData);
        if (user == null)
        {
            return NotFound("User wasn't found");
        }

        var comment = _commentManager.GetCommentById(commentId, false, false, false, true);
        if (comment == null)
        {
            return NotFound("No comment was selected");
        }

        var commentHistory = new CommentHistory()
        {
            EditedById = user.Id,
            ReactionGroupId = commentId,
            CommentStatus = CommentStatus.Marked,
            EditedOn = DateTime.Now
        };
        _commentHistoryManager.AddCommentHistory(commentHistory);
        var docReview = _docReviewManager.GetDocReview(comment.DocReviewId, includeProject: true);
        var dictionary = _commentHistoryManager.GetCommentStatisticsByDocReviewAndStatus(docReview, CommentStatus.Marked);
        if (dictionary.FirstOrDefault().Value % 5 == 0)
        {
            await _hubContext.Clients.Group(docReview.ProjectId.ToString()).SendCoreAsync("MarkedComments", new[] { docReview.Name + " from project: " + docReview.Project.ExternalName });
        }

        return NoContent();
    }

    /// <author> Sander Verheyen </author>
    /// <summary>
    /// Makes a new emoji reaction on a comment.
    /// </summary>
    /// <param name="emojiCommentDto"></param>
    /// <returns></returns>
    [HttpPost("CreateEmojiReaction")]
    [Authorize]
    public async Task<IActionResult> CreateEmojiReaction([FromBody] EmojiCommentDto emojiCommentDto)
    {
        // Get the user
        var user = await _userManager.GetUserAsync(User);
        var removeEmojiCode = "";
        var removeEmojiId = 0;
        if (user == null)
        {
            return NotFound("User wasn't found");
        }
        // Check if this EmojiReaction already exists
        var emoji = _commentManager.GetEmojiReactionByCommentAndEmoji(emojiCommentDto.PlacedOnReactionId,
            emojiCommentDto.EmojiId, user.Id);
        if (emoji != null)
        {
            removeEmojiCode = emoji.Emoji.Code;
            removeEmojiId = emoji.EmojiId;
            _commentManager.RemoveReaction(emoji);
            return Ok(new { AddCode = "", RemoveCode = removeEmojiCode, EmojiId = 0, RemoveEmojiId = removeEmojiId });
        }
        // Check if the user already placed a reaction
        var reaction = _commentManager.GetEmojiReactionsByCommentAndUser(emojiCommentDto.PlacedOnReactionId, user.Id);

        if (reaction != null)
        {
            removeEmojiCode = reaction.Emoji.Code;
            removeEmojiId = reaction.EmojiId;
            _commentManager.RemoveReaction(reaction);
        }
        // Create a new EmojiReaction
        var newReaction = new EmojiReaction()
        {
            UserId = user.Id,
            PlacedOnReactionGroupId = emojiCommentDto.PlacedOnReactionId,
            EmojiId = emojiCommentDto.EmojiId,
            DocReviewId = emojiCommentDto.DocReviewId
        };
        newReaction = _commentManager.AddReaction(newReaction);
        var addCode = _emojiManager.GetEmoji(newReaction.EmojiId);
        return Ok(new { AddCode = addCode.Code, RemoveCode = removeEmojiCode, EmojiId = newReaction.EmojiId, RemoveEmojiId = removeEmojiId });
    }
}

