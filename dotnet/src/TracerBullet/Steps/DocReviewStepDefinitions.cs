using BL.Comment;
using BL.DocReview;
using BL.User;
using Domain.Comment;
using Domain.DocReview;
using Domain.User;
using FluentAssertions;
using UI.MVC.Models.Dto;

namespace SpecFlowProject1.Steps;

[Binding]
public class DocReviewStepDefinitions
{
    // Fields.

    private List<CommentDto> _comments;
    private DocReview _docReview;
    private IList<DocReviewSetting> _docReviewSettings;
    private CommentDto _commentDto;

    private readonly HttpService _httpService;

    // Constructor.
    public DocReviewStepDefinitions()
    {
        _docReviewSettings = new List<DocReviewSetting>();

        _httpService = new HttpService();
    } // Constructor.

    // Step definiations.

    // Seed DocReviewSettings.
    [Given(@"DocReviewSettings:")]
    public void GivenDocReviewSettings(Table table)
    {
        foreach (var row in table.Rows)
        {
            var docReviewSetting = new DocReviewSetting
            {
                IsCommentingAllowed = Boolean.Parse(row[0]),
                IsSubCommentingAllowed = Boolean.Parse(row[1]),
                AreEmojisAllowed = Boolean.Parse(row[2]),
                IsClosedForComments = Boolean.Parse(row[3]),
                IsLogInRequired = Boolean.Parse(row[4])
            };

            // Only 1 row is in the testData -> settings = owned -> it'll be added to the DB along with the docReview.
            _docReviewSettings.Add(docReviewSetting);
        } // Foreach.
    } // GivenDocReviewSettings.

    // Seed the users.
    [Given(@"Users:")]
    public async Task GivenUsers(Table table)
    {
        foreach (var row in table.Rows)
        {
            var user = new User
            {
                Id = row[0],
                Firstname = row[1],
                Lastname = row[2],
                Email = row[3],
                PasswordHash = row[4]
            };

            if (await _httpService.GetUser(row[0]) == null)
                await _httpService.AddUser(user);
        } // Foreach.
    } // GivenUsers.

    // Seed DocReviews.
    [Given(@"DocReviews:")]
    public async Task GivenDocReviews(Table table)
    {
        int teller = 0;
        foreach (var row in table.Rows)
        {
            var newDocReview = new DocReview
            {
                DocReviewId = Int32.Parse(row[0]),
                Name = row[1],
                Description = row[3],
                DocReviewText = row[5],
                WrittenBy = await _httpService.GetUser(row[6]),
                DocReviewSettings = _docReviewSettings[teller++]
            };

            if (await _httpService.GetDocReview(Int32.Parse(row[0])) == null)
                await _httpService.AddDocReview(newDocReview);
        } // Foreach.
    } // GivenDocReviews.

    // Seed the comments.
    [Given(@"Comments:")]
    public async Task GivenComments(Table table)
    {
        foreach (var row in table.Rows)
        {
            var newComment = new ReactionGroup
            {
                CommentId = Int32.Parse(row[0]),
                User = await _httpService.GetUser(row[2]),
                DocReview = await _httpService.GetDocReview(Int32.Parse(row[3])),
                CommentText = row[1]
            };
            
            if ( (await _httpService.GetComment(Int32.Parse(row[0]))).CommentId == 0)
                await _httpService.AddCommentDto(new CommentDto(newComment));
        } // Foreach.
    } // GivenComments.

    [Given(@"There is a docReview (.*)")]
    public async Task GivenErIsEenDocReview(int id)
    {
        _docReview = await _httpService.GetDocReview(id);
        _docReview.Should().NotBeNull();
    } // GivenErIsEenDocreview.

    [When(@"DocReview (.*) loads")]
    public async Task WhenDocReviewLoads(int docReviewId)
    {
        _comments = new List<CommentDto>();

        foreach (var c in await _httpService.GetCommentsByDocReview(docReviewId))
        {
            _comments.Add(c);
        }
    } // WhenDocReviewLoads.

    [Then(@"the comments should contain the following comments:")]
    public async Task ThenTheCommentsShouldContainTheFollowingComments(Table table)
    {
        int teller = 0;
        foreach (var row in table.Rows)
        {
            var comment = new ReactionGroup
            {
                CommentId = Int32.Parse(row[0]),
                User = await _httpService.GetUser(row[2]),
                DocReview = await _httpService.GetDocReview(Int32.Parse(row[3])),
                CommentText = row[2],
            };
            
            // Test equality.
            _comments[teller++].CommentId.Should().Be(comment.CommentId);
        }
    } // ThenTheCommentsShouldContainTheFollowingComments.


    [When(@"User (.*) writes a comment (.*) ""(.*)"" on characters (.*) - (.*)")]
    public async Task WhenUserWritesACommentOnCharacters(string userId, int commentId, string commentText,
        int beginChar, int endChar)
    {
        _commentDto = new CommentDto(new ReactionGroup
        {
            CommentId = commentId,
            User = await _httpService.GetUser(userId),
            DocReview = await _httpService.GetDocReview(2),
            CommentText = commentText
        });
    }

    [When(@"Clicks the publish button")]
    public async Task WhenClicksThePublishButton()
    {
        await _httpService.AddCommentDto(_commentDto);
    }

    [Then(@"DocReview (.*) now has (.*) comments")]
    public async Task ThenDocReviewNowHasComments(int docReviewId, int numberOfComments)
    {
        var commentsDocReview = await _httpService.GetCommentsByDocReview(docReviewId);
        int count = 0;

        foreach (var comment in commentsDocReview)
        {
            count++;
            var reactions = await _httpService.GetReactionsOfCommentByComment(comment.CommentId);

            foreach (var unused in reactions)
                count++;
        }

        count.Should().Be(numberOfComments);
    } // ThenDocReviewNowHasComments.

    [Then(@"User (.*) has comment (.*)")]
    public async Task ThenUserHasComment(int userId, int commentId)
    {
        var commentsUser = await _httpService.GetCommentsByUser(userId.ToString());
        _commentDto = await _httpService.GetComment(commentId);
        commentsUser.Should().Contain(_commentDto); 
    } // ThenUserHasComment.

    [Then(@"DocReview (.*) page now shows comment (.*)")]
    public async Task ThenDocReviewPageNowShowsComment(int docReviewId, int commentId)
    {
        var commentsDocReview = await _httpService.GetCommentsByDocReview(docReviewId);
        _commentDto = await _httpService.GetComment(commentId);
        commentsDocReview.Should().Contain(_commentDto);
    }


    [Given(@"User (.*) is logged in")]
    public void GivenUserIsLoggedIn(int p0)
    {
       // Out of scope.
    } // GivenUserIsLoggedIn.

    [Given(@"User (.*) has a comment (.*) on DocReview (.*)")]
    public async Task GivenUserHasACommentOnDocReview(int userId, int commentId, int docReviewId)
    {
        _docReview = await _httpService.GetDocReview(docReviewId);
        var comments = await _httpService.GetCommentsByUserAndDocReview(userId.ToString(), docReviewId);
        _commentDto = await _httpService.GetComment(commentId);
        comments.Should().Contain(_commentDto);
    } // GivenUserHasACommentOnDocReview.

    [When(@"User (.*) writes a comment (.*) ""(.*)"" on comment (.*)")]
    public async Task WhenUserWritesACommentOnCommentOfUser(int userId, int commentId, string commentText, int mainCommentId)
    {
        var mainComment = await _httpService.GetComment(mainCommentId);
        
        _commentDto = new CommentDto() {CommentText = commentText, UserId = userId.ToString(), DocReviewId = mainComment.DocReviewId, PlacedOnReactionId = mainCommentId};
    }

    [When(@"User (.*) clicks the publish button")]
    public async Task WhenUserClicksThePublishButton(int p0)
    {
        await _httpService.AddCommentDto(_commentDto);
    } // WhenUserClicksThePublishButton.


    [Then(@"comment (.*) should have a subcomment: comment (.*)")]
    public async Task ThenCommentShouldHaveASubcommentComment(int p0, int p1)
    {
        var mainComment = await _httpService.GetComment(p0);
        //var subComment = (await _httpService.GetReactionsOfCommentByComment(p0) as IList<CommentDto>)?[0];
        var subComment = await _httpService.GetComment(p1);
        var superCommentOfSubComment = await _httpService.GetComment(subComment.PlacedOnReactionId ?? 0);
        
        superCommentOfSubComment.CommentId.Should().Be(mainComment.CommentId);
    }
}