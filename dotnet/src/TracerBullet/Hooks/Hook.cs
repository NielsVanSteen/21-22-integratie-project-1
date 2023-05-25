using System;
using BL;
using BL.Comment;
using BL.DocReview;
using BL.User;
using BoDi;
using DAL;
using DAL.Repositories;
using DAL.Repositories.Comment;
using DAL.Repositories.DocReview;
using DAL.Repositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Hooks
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Class responsible for the context injection for the specflow.
    /// </summary>
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer container;

        public Hooks(IObjectContainer container)
        {
            this.container = container;
        }

        [BeforeScenario]
        public void Create()
        {
            // Create DbContext. -> use another db just for this specflow.
            var context = new DocReviewDbContext(new DbContextOptionsBuilder<DocReviewDbContext>().UseSqlite(@"Data Source=../../../specflow.db").Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            // Create Repositories.
            var commentRepository = new CommentRepository(context);
            var docReviewRepository = new DocReviewRepository(context);
            var userRepository = new UserRepository(context);
            
            // Create the managers & register them for context injection.
            container.RegisterInstanceAs<ICommentManager>(new CommentManager(commentRepository));
            container.RegisterInstanceAs<IUserManager>(new UserManager(userRepository));
            container.RegisterInstanceAs<IDocReviewManager>(new DocReviewManager(docReviewRepository));
        }

        [AfterScenario]
        public void Destroy()
        {
            container.Resolve<ICommentManager>();
            container.Resolve<IUserManager>();
            container.Resolve<IDocReviewManager>();
        }
    }
}