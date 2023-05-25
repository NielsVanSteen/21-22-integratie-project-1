using System.Net;
using System.Text.RegularExpressions;
using BL.DocReview;
using BL.Project;
using Domain.DocReview;
using Domain.Project;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json.Linq;
using UI.MVC.Attributes;
using UI.MVC.CloudStorage;
using UI.MVC.Extensions;
using UI.MVC.Identity;
using UI.MVC.Models.Android;
using UI.MVC.Models.DocReview;
using UI.MVC.Models.Hub;
using UI.MVC.Models.Dto;

namespace UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/{project}/[controller]")]
public class DocReviewsController : ControllerBase
{
    // Fields.
    private readonly UserManager<User> _userManager;
    private readonly IDocReviewManager _docReviewService;
    private readonly ICloudStorage _cloudStorage;
    private readonly IProjectManager _projectService;
    private readonly IHubContext<DocreviewHub> _hubContext; 
    private readonly ISurveyManager _surveyService;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _hostingEnvironment;

    
    // Constructor.
    public DocReviewsController(UserManager<User> userManager, IDocReviewManager manager, ICloudStorage cloudStorage, IProjectManager projectService, IHubContext<DocreviewHub> hubContext, ISurveyManager surveyService, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _userManager = userManager;
        _docReviewService = manager;
        _cloudStorage = cloudStorage;
        _projectService = projectService;
        _hubContext = hubContext;
        _surveyService = surveyService;
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _httpClient = new HttpClient();
    } // DocReviewsController.
    
    // Methods.

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var docReview = _docReviewService.GetDocReview(id, true);
        return Ok(docReview);
    } // Get.
    
    /// <summary>
    /// Method that gets all the docreviews from a project
    /// </summary>
    /// <param name="id">ProjectId</param>
    /// <returns></returns>
    [HttpGet("GetDocReviewsByProject/{id}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult GetDocReviewsByProject(int id )
    {
        
        IEnumerable<DocReview> docReviews = _docReviewService.GetDocReviewsByProject(id, true, true,true);
        var docReviewsDto = new List<DocReviewAndroidDto>();

        if (docReviews == null || !docReviews.Any())
        {
            return NoContent();
        }
        
        foreach (var docReview in docReviews)
        {
            docReviewsDto.Add(new DocReviewAndroidDto(docReview));
        }
        
        return Ok(docReviewsDto);
    }//GetProjectByUser
  
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Post method to archive a <see cref="DocReview"/>
    /// </summary>
    /// <returns></returns>
    [HttpPost("ArchiveDocReview/{docReviewId:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ArchiveDocReview(int docReviewId)
    {
        // Get Current docReview
        var docReview = _docReviewService.GetDocReview(docReviewId, includeHistory: true);

        // Get current User
        var user = await _userManager.GetUserAsync(User);

        // if DocReview is not found Return NotFound(404)
        if (docReview is null)
            return NotFound("Invalid DocReview Id");

        if (docReview.GetLatestProjectHistory().DocReviewStatus == DocReviewStatus.Archived)
        {
            // Add new DocReviewHistory to docReview
            docReview.DocReviewHistories.Add(new DocReviewHistory
                {
                    DocReview = docReview,
                    DocReviewStatus = DocReviewStatus.Published,
                    EditedOn = DateTime.Now
                }
            );
        }
        else
        {
            // Add new DocReviewHistory to docReview
            docReview.DocReviewHistories.Add(new DocReviewHistory
                {
                    DocReview = docReview,
                    DocReviewStatus = DocReviewStatus.Archived,
                    EditedOn = DateTime.Now,
                    Editor = user
                }
            );
        }

        _docReviewService.ChangeDocReview(docReview);

        return NoContent();
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Post method to delete a <see cref="DocReview"/> from the database
    /// </summary>
    /// <returns></returns>
    [HttpPost("DeleteDocReview/{docReviewId:int}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult DeleteDocReview(int docReviewId)
    {
        // Get Current docReview
        var docReview = _docReviewService.GetDocReview(docReviewId);

        // If DocReview is not found return NotFound(404)
        if (docReview is null)
            return NotFound("Invalid DocReview Id");

        // Remove the docreview from the database
        _docReviewService.RemoveDocReview(docReview);

        return NoContent();
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] DocReview docReview)
    {
        _docReviewService.AddDocReview(docReview);
        return NoContent();
    } // Post.
    
    
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Close a <see cref="Docreview"/> for commenting.
    /// </summary>
    /// <returns></returns>
    [HttpPut("UpdateDocReviewClosed/{id}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> UpdateDocReviewClosed(int id)
    {
        
        var docReview = _docReviewService.GetDocReview(id, includeHistory: true, includeProject:true);
        var user = await _userManager.GetUserAsync(User);
        if (docReview == null)
        {
            return NotFound("Invalid DocReview Id");
        }
        
        // If docReview is already closed for comments return NoComment(204)
        if (docReview.DocReviewSettings.IsClosedForComments)
        {
            return NoContent();
        }
        
        var history = new DocReviewHistory()
        {
            EditorId = user.Id,
            EditedOn = DateTime.Now,
            DocReviewStatus = DocReviewStatus.Closed,
            DocReviewId = docReview.DocReviewId
        };
        
        await _hubContext.Clients.Group(docReview.ProjectId.ToString()).SendCoreAsync("ClosedDocreview",new[] {docReview.Name + " from project: " + docReview.Project.ExternalName});

        docReview.DocReviewSettings.IsClosedForComments = true;
        _docReviewService.AddDocReviewHistory(history);
        _docReviewService.ChangeDocReview(docReview);
        return NoContent();
    }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Post method to publish a <see cref="DocReview"/>
    /// </summary>
    /// <returns></returns>
    [HttpPut("PublishDocReview/{docReviewId:int}")]
    //[Authorize(Policy = CustomIdentityConstants.IsModerator)]
    public async Task<IActionResult> PublishDocReview(int docReviewId)
    {
        // Get Current docReview
        var docReview = _docReviewService.GetDocReview(docReviewId, includeHistory: true, includeProject: true);

        // Get current user
        var user = await _userManager.GetUserAsync(User);

        // If docreview is null return NotFound(404)
        if (docReview is null)
            return NotFound("Invalid DocReview Id");

        // If the latest history is created return BadRequest(400) because the docreview can not be published
        if (docReview.GetLatestProjectHistory().DocReviewStatus != DocReviewStatus.Created)
        {
            return BadRequest("DocReview is already published");
        }

        // Add new DocReviewHistory to docReview
        docReview.DocReviewHistories.Add(new DocReviewHistory
            {
                DocReview = docReview,
                DocReviewStatus = DocReviewStatus.Published,
                EditedOn = DateTime.Now,
                Editor = user
            }
        );
        await _hubContext.Clients.Group(docReview.ProjectId.ToString()).SendCoreAsync("PublishedDocreview",new[] {docReview.Name + " from project: " + docReview.Project.ExternalName});
        
        _docReviewService.ChangeDocReview(docReview);

        return NoContent();
    }

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Uploads an image to cloud storage <see cref="ICloudStorage"/>, which can then be used by in a doc-review.
    /// </summary>
    /// <returns></returns>
    [HttpPost("Upload")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> Upload(IFormFile image)
    {
        // Check the file size.
        if (image.Length > (MaxFileSizeAttribute.DefaultMaxFileSizeInBytes))
            return Conflict("Max file size is 5MB");
        
        // Check the extension.
        if (!AllowedExtensionsAttribute.ImageDefaultAllowedExtension.Contains(Path.GetExtension(image.FileName).ToLower()))
            return Conflict(AllowedExtensionsAttribute.GetErrorMessage());

        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);
        var filename = project.GenerateUserUploadedImageFileName();
        var name = await _cloudStorage.UploadFileAsync(image, filename, new CachingTime(CachingTime.CacheDefaults.Long));

        return CreatedAtAction("Write", "DocReview", name, name);
    } // Upload.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the links of all images of a specific project.
    /// The project is not given as a parameter since it's part of the url.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetDocReviewImagesByProject")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult GetDocReviewImagesByProject()
    {
        var links = new List<string>();
        var projectName = ApplicationConstants.GetProjectName(RouteData);
        var project = _projectService.GetProjectByExternalName(projectName);
        var storageObjects = _cloudStorage.GetFilesByPrefix(project.GetAllDocReviewImagesPrefixByProject());

        foreach (var obj in storageObjects)
            links.Add(ApplicationConstants.CloudStorageBasicUrl + projectName);

        return Ok(links);
    } // GetDocReviewImagesByProject.
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method to create a new survey on a part of the docreviewContent
    /// </summary>
    /// <param name="surveyDto"></param>
    /// <returns></returns>
    [HttpPost("AddSurvey")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public IActionResult AddSurvey(SurveyDto surveyDto)
    {
        // Validate the parameters
        if (string.IsNullOrWhiteSpace(surveyDto.Description))
        {
            return BadRequest("Can not create survey with empty description");
        }
        if (string.IsNullOrWhiteSpace(surveyDto.Quote))
        {
            return BadRequest("Can not create survey with empty quote");
        }
        if (string.IsNullOrWhiteSpace(surveyDto.Title))
        {
            return BadRequest("Can not create survey with empty title");
        }
        
        // Get the DocReview from the database
        var docReview = _docReviewService.GetDocReview(surveyDto.DocReviewId);

        if (docReview is null)
        {
            return NotFound("Invalid docReview");
        }
        
        //Get the begin and endchar from the quote
        int beginChar;
        int endChar;
        try
        {
            var beginAndEndChar = docReview.GetIndexes(surveyDto.Quote);
            beginChar = beginAndEndChar.BeginChar;
            endChar = beginAndEndChar.EndChar;
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        
        
        //Create a list of SurveyOptions from the dictionary in the DTO
        var options = new List<SurveyOption>();
        foreach (var option in surveyDto.Options)
        {
            if (string.IsNullOrWhiteSpace(option.Key))
                continue;
            
            options.Add(new SurveyOption
            {
                Option = option.Key,
                Description = option.Value
            });
        }

        if (options.Count < 2)
            return BadRequest("Please give at least two different options with unique texts");

        //Create the survey and link it to the DocReview
        docReview.Surveys.Add(new Survey
        {
            AreMultipleOptionsAllowed = surveyDto.AreMultipleOptionsAllowed,
            Description = surveyDto.Description,
            DocReview = docReview,
            Title = surveyDto.Title,
            BeginChar = beginChar,
            EndChar = endChar,
            SurveyOptions = options
        });

        //Push the updated docreview to the database
        _docReviewService.ChangeDocReview(docReview);
        
        return NoContent();
    }

    /// <summary>
    /// Method to add response to a survey to the database
    /// </summary>
    /// <returns></returns>
    [HttpPost("surveyResponse/{surveyId:int}")]
    [Authorize]
    public async Task<IActionResult> PostSurveyResponse([FromRoute]int surveyId, SurveyResponseDto surveyResponseDto)
    {
        // Validate the parameters
        if (surveyResponseDto.OptionIds.Count == 0)
        {
            return BadRequest("Selecteer minimaal 1 optie");
        }

        // Get the survey from the database
        var survey = _surveyService.GetSurvey(surveyId, includeSurveyOptions:true);

        if (survey is null)
        {
            return NotFound("Ongeldige survey");
        }

        if (!survey.AreMultipleOptionsAllowed && surveyResponseDto.OptionIds.Count > 1)
        {
            return BadRequest("Je mag maar 1 optie selecteren");
        }

        //Get the user from the database
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return NotFound("Ongeldige gebruiker");
        }

        if (_surveyService.GetUserRespondedToSurvey(survey, user))
        {
            return BadRequest("Je hebt al een antwoord gegeven op deze survey");
        }

        //Create a list of SurveyResponses from the list of optionIds in the DTO
        var surveyResponses = new List<UserSurveyAnswer>();
        try
        {
            foreach (var optionId in surveyResponseDto.OptionIds)
            {
                var surveyResponse = new UserSurveyAnswer
        
                {
                    Survey = survey,
                    ChosenOption = survey.SurveyOptions.Single(x => x.SurveyOptionId == optionId),
                    User = user
                };
                surveyResponses.Add(surveyResponse);
            }
        }
        catch (Exception)
        {
            return BadRequest("Ongeldige optie");
        }

        _surveyService.AddUserResponse(surveyResponses);

        return NoContent();
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Method to import a google doc as html as html
    /// </summary>
    /// <param name="fileId">string with the fileId that comes from the route</param>
    /// <returns>HTML content of the document</returns>
    [HttpPost("import/{fileId}")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> ImportFromGoogleDocs(string fileId)
    {
        // Get the request url using the fileId
        var requestUrl = fileId.GetExportUrl();
        
        //get the authentication token
        var authToken =JObject.Parse(await GetAuthToken());

        // If there is an error with the token, return the error
        if (authToken["error"] != null)
        {
            return BadRequest("An error occurred: " + authToken["error"]);
        }

        //Compose the request
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(requestUrl),
            Headers =
            {
                { HttpRequestHeader.Authorization.ToString(), $"Bearer {authToken["access_token"]}" },
                { HttpRequestHeader.Accept.ToString(), "text/html" },
                { HttpRequestHeader.ContentType.ToString(), "application/json" }
            }
        };
        
        //Send the request
        var response = await _httpClient.SendAsync(requestMessage);
        
        //If the response is not ok, return the error
        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(response.ReasonPhrase);
        }
        
        //Get the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        
        //Replace the standard inline css that google provides
        responseContent = Regex.Replace(responseContent, "<body style=\".*?\">", "<body>");
        
        //Return the content
        return Ok(new HtmlString(responseContent));
    }
    
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// private method to get the auth token from google
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetAuthToken()
    {
        //Compose the request
        var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://oauth2.googleapis.com/token"),
            Headers =
            {
                { HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded" }
            },
        };
        
        //Add the body to the request
        requestMessage.Content = JsonContent.Create(
            new
            {
                client_id = _configuration["GOOGLE_AUTHENTICATION_CLIENT_ID"],
                client_secret = _configuration["GOOGLE_AUTHENTICATION_CLIENT_SECRET"],
                grant_type = "refresh_token",
                refresh_token = _configuration["GOOGLE_AUTHENTICATION_CLIENT_REFRESH"],
            });
        
        //Send the request
        var response = await _httpClient.SendAsync(requestMessage);
        
        //Get the response content and return it
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }

    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Api method to parse a pdf to html string
    /// </summary>
    /// <param name="pdf"><see cref="IFormFile"/> - the file that is uploaded</param>
    /// <param name="token">CancellationToken that is passed in to tell if the request is cancelled</param>
    /// <returns></returns>
    [HttpPost("uploadPdf")]
    [Authorize(Policy = ApplicationConstants.IsModerator)]
    public async Task<IActionResult> UploadPdf(IFormFile pdf, CancellationToken token)
    {
        try
        {
            //Parse the uploaded file to a html string
            var content = await pdf.ParseToPdf(_hostingEnvironment, token).ConfigureAwait(false);
            return Ok(content);
        }
        catch (TaskCanceledException)
        {
            //When the task is cancelled return
            return BadRequest("The task was cancelled");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}