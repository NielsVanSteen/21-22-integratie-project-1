using System.Text;
using System.Text.Json;
using Domain.Comment;
using Domain.DocReview;
using Domain.User;
using Newtonsoft.Json;
using UI.MVC.Controllers.Api;
using UI.MVC.Models.Dto;

namespace SpecFlowProject1.Steps;

public class HttpService
{
    private const string BaseUri = "https://localhost:5001/api";

    private HttpClient NewHttpClient()
    {
        var httpHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback =
                (sender, cert, chain, sslPolicyErrors) => true
        };

        return new HttpClient(httpHandler);
    } // NewHttpClient.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="UsersController.Post"/> to add a <see cref="User"/>.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to add.</param>
    /// <returns></returns>
    public async Task<User> AddUser(User user)
    {
        using var http = NewHttpClient();
        var json = JsonConvert.SerializeObject(user);
        var requestContext = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await http.PostAsync($"{BaseUri}/users", requestContext);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<User>(resString);
    } // AddUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="DocReviewsController.Post"/> to add a <see cref="DocReview"/>.
    /// </summary>
    /// <param name="docReview">The <see cref="DocReview"/> to add.</param>
    /// <returns></returns>
    public async Task<DocReview> AddDocReview(DocReview docReview)
    {
        using var http = NewHttpClient();
        var json = JsonConvert.SerializeObject(docReview);
        var requestContext = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await http.PostAsync($"{BaseUri}/docreviews", requestContext);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<DocReview>(resString);
    } // AddUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.Post"/> to add a <see cref="ReactionGroup"/>.
    /// </summary>
    /// <param name="comment">The <see cref="DocReview"/> to add.</param>
    /// <returns></returns>
    public async Task<CommentDto> AddCommentDto(CommentDto comment)
    {

        using var http = NewHttpClient();
        var json = JsonConvert.SerializeObject(comment);
        var requestContext = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await http.PostAsync($"{BaseUri}/comments/AddComment", requestContext);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CommentDto>(resString);
    } // AddUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="DocReviewsController.Get(int)"/> to retrieve a <see cref="DocReview"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="DocReview"/> we want to retrieve.</param>
    /// <returns></returns>
    public async Task<DocReview> GetDocReview(int id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/docreviews/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<DocReview>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.GetByDocReview(int)"/> to retrieve all the <see cref="ReactionGroup"/> for a <see cref="DocReview"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="DocReview"/> we want all the comments for.</param>
    /// <returns></returns>
    public async Task<IEnumerable<CommentDto>> GetCommentsByDocReview(int id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/comments/GetByDocReview/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<CommentDto>>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.GetByUser(string)"/> to retrieve all the <see cref="ReactionGroup"/> of a <see cref="User"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="User"/> we want all the comments for.</param>
    /// <returns></returns>
    public async Task<IEnumerable<CommentDto>> GetCommentsByUser(string id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/comments/GetByUser/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<CommentDto>>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.GetByUserAndDocReview(string, int)"/> to retrieve all the <see cref="ReactionGroup"/> of a <see cref="User"/> by <see cref="DocReview"/>
    /// </summary>
    /// <param name="userId">The Id <see cref="User"/> we want all the comments for.</param>
    /// <param name="docReviewId">The Id <see cref="DocReview"/> we want all the comments for.</param>
    /// <returns></returns>
    public async Task<IEnumerable<CommentDto>> GetCommentsByUserAndDocReview(string userId, int docReviewId)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/comments/GetByUserAndDocReview/{userId}/{docReviewId}");
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<CommentDto>>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.GetReactionsOfCommentByComment(int)"/> to retrieve all the subComments of a <see cref="ReactionGroup"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="ReactionGroup"/> we want all the subComments for.</param>
    /// <returns></returns>
    public async Task<IEnumerable<CommentDto>> GetReactionsOfCommentByComment(int id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/comments/GetReactionsOfCommentByComment/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<CommentDto>>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="CommentsController.Get(int)"/> to retrieve a <see cref="User"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="ReactionGroup"/> we want to retrieve.</param>
    /// <returns></returns>
    public async Task<CommentDto> GetComment(int id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/comments/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CommentDto>(resString);
    } // ReadUser.
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Sends an HttpRequest to <see cref="UsersController.Get(string)"/> to retrieve a <see cref="User"/>.
    /// </summary>
    /// <param name="id">The Id <see cref="User"/> we want to retrieve.</param>
    /// <returns></returns>
    public async Task<User> GetUser(string id)
    {
        using var http = NewHttpClient();
        var res = await http.GetAsync($"{BaseUri}/users/"+id);
        var resString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<User>(resString);
    } // ReadUser.
}