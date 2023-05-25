using System.ComponentModel.DataAnnotations;
using BL.DocReview;
using BL.Project;
using BL.User;
using Domain.DocReview;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Account;
using UI.MVC.Models.DocReview;
using UI.MVC.Models.Shared;

namespace UI.MVC.Controllers;

public class DocReviewController : Controller
{
    private readonly IProjectManager _projectService;
    private readonly IDocReviewManager _docReviewManager;
    private readonly IUserManager _userService;
    private readonly UserManager<User> _userManager;
    private readonly IEmojiManager _emojiManager;
    private readonly ICloudStorage _cloudStorage;


    // Constructor.
    public DocReviewController(IUserManager userService, IProjectManager projectService, UserManager<User> userManager,
        IDocReviewManager docReviewManager, IEmojiManager emojiManager, ICloudStorage cloudStorage)
    {
        _projectService = projectService;
        _userManager = userManager;
        _docReviewManager = docReviewManager;
        _emojiManager = emojiManager;
        _cloudStorage = cloudStorage;
        _userService = userService;
    }

    // Methods.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Called to display the view to write a new <see cref="DocReview"/>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult Write()
    {
        var model = new WriteDocReviewModel
        {
            AvailableEmojis = _emojiManager.GetAvailableEmojis()
        };

        var projectName = ApplicationConstants.GetProjectName(RouteData);
        ViewBag.TimeLine = _projectService.GetProjectByExternalName(projectName).TimeLine;
        ViewBag.UserUploadedImageModel = new UserUploadedImageModel();
        return View(model);
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method that gets called when a new DocReview is to be created
    /// </summary>
    /// <param name="model"> <see cref="WriteDocReviewModel"/> that contains all the data that is provided by the user</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Write(WriteDocReviewModel model)
    {
        // Get current project
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);

        model.AvailableEmojis = _emojiManager.GetAvailableEmojis();

        if (!ModelState.IsValid)
        {
            ViewBag.TimeLine = _projectService.GetProjectByExternalName(projectName).TimeLine;
            ViewBag.UserUploadedImageModel = new UserUploadedImageModel();
            return View(model);
        }

        var emojis = new List<Emoji>();
        var allEmojis = _emojiManager.GetAvailableEmojis();

        var docReview = model.GetDocReview();
        var user = await _userManager.GetUserAsync(User);

        if (docReview.DocReviewSettings.AreEmojisAllowed)
        {
            // Parse Emoji's and add them
            foreach (var code in model.ParseSelectedEmojiIds(_emojiManager))
            {
                emojis.Add(new Emoji()
                {
                    Code = code,
                    DocReviewId = docReview.DocReviewId
                });
            }
        }

        // Check if the emoji list includes emoji's that are not in the allowed emoji list.
        bool hasNonAllowedEmoji = emojis.Any(e => allEmojis.Select(x => x.EmojiId).Contains(e.EmojiId));

        if (hasNonAllowedEmoji)
        {
            ModelState.AddModelError("AreEmojiOnCommentsAllowed", "List includes non-allowed emoji's!");
            ViewBag.TimeLine = _projectService.GetProjectByExternalName(projectName).TimeLine;
            return View(model);
        }


        // initialize different properties
        docReview.WrittenById = user.Id;
        docReview.ProjectId = project.ProjectId;
        docReview.Project = project;
        docReview.WrittenBy = user;

        // Only add the list with emoji's if emoji's are allowed.
        if (model.AreEmojisAllowed)
            docReview.AvailableEmoji = emojis;

        // add Docreview to database
        docReview.DocReviewHistories.Add(new DocReviewHistory()
        {
            DocReview = docReview,
            DocReviewId = docReview.DocReviewId,
            DocReviewStatus = DocReviewStatus.Created,
            EditedOn = DateTime.Now,
            Editor = user,
            EditorId = user.Id
        });
        docReview = _docReviewManager.AddDocReview(docReview);

        // Upload the image.
        await _cloudStorage.UploadFileAsync(model.BannerImage, docReview.GetBannerImageFileName(), new CachingTime(CachingTime.CacheDefaults.Long));

        return RedirectToAction("Index", "ProjectManage");
    } // Write.

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method that gets called to upload a new image to google cloud
    /// </summary>
    /// <param name="model"><see cref="UserUploadedImageModel"/> that contains the image to be uploaded</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UploadImage(UserUploadedImageModel model)
    {
        // Check if ModelState is valid.
        if (!ModelState.IsValid)
        {
            // Since this method redirects ModelState is lost -> we'll use a custom status message model.
            var messageModel = new ConfirmStatusMessageModel();
            messageModel.Title = "Wrong File Upload!";
            messageModel.Description =
                string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            messageModel.Type = ConfirmStatusMessageModel.NotificationType.Type.Error;
            return RedirectToAction("Write", messageModel);
        }

        // Get project
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);
        
        //Get unique filename and image path
        var filename = project.GenerateUserUploadedImageFileName();
        model.ImagePath = ApplicationConstants.CloudStorageBasicUrl + filename;
        
        if (model.UserUploadedImage != null)
        {
            // Upload the image.
            await _cloudStorage.UploadFileAsync(model.UserUploadedImage, filename,
                new CachingTime(CachingTime.CacheDefaults.Long));
        }

        ViewBag.UserUploadedImageModel = model;
        return RedirectToAction("Write");
    } // UploadImage.
}