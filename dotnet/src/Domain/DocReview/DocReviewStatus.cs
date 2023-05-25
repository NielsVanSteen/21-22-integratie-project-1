using Domain.User;

namespace Domain.DocReview;

/// <author>Niels Van Steen</author>
/// <summary>
/// All the states a <see cref="DocReview"/> can have.
/// </summary>
public enum DocReviewStatus
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// DocReview was created but is not yet visible to normal <see cref="UserRole.RegularUser"/>.
    /// </summary>
    Created = 0,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The DocReview is visible for everyone.
    /// </summary>
    Published,
    
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// The DocReview is archived and is no longer visible to <see cref="UserRole.RegularUser"/>.
    /// </summary>
    Archived,

    /// <author>Bjorn straetemans</author>
    /// <summary>
    /// The DocReview has been closed for <see cref="UserRole.RegularUser"/> to react on.
    /// </summary>
    Closed
} 