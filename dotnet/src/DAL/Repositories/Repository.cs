namespace DAL.Repositories;

///<author>Niels Van Steen</author>
/// <summary>
/// Abstract repository class which holds the Context field centrally.
/// </summary>
public abstract class Repository
{
    // Fields.
    protected DocReviewDbContext Context { get; }

    // Constructor.
    protected Repository(DocReviewDbContext context)
    {
        Context = context;
    } // Repository.
}