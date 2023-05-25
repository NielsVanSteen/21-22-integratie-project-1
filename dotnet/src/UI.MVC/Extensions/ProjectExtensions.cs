using BL.Project;
using Domain.Project;
using Domain.User;
using UI.MVC.CloudStorage;
using UI.MVC.Identity;

namespace UI.MVC.Extensions;

/// <summary>
/// Class that holds extension methods for type <see cref="Project"/>
/// </summary>
public static class ProjectExtensions
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Prefix that is used to generate a filename of a user uploaded image
    /// </summary>
    private const string UserUploadedImagePrefix = "project-images/";

    // Properties.
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Instead of actually storing the path of an image in the database.
    /// The name of the project image exists of 3 parts:
    ///
    /// 1. the general website prefix where all the image are stored. (E.g., www.google.com)
    /// 2. The prefix indicating it's a project logo. (E.g., MainLogo_).
    /// 3. The <see cref="Project.ProjectId"/>.
    ///
    /// So an example of a full url would be: 'www.google.com/MainLogo_21232421'.
    ///
    /// This way database storage is not wasted and to get the the logo of a project we need. The return value of this method and the project's id.
    /// </summary>
    /// <returns></returns>
    public const string ProjectLogoPrefix = "project-logos/";

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ProjectLogoPrefix"/>
    /// </summary>
    public const string ProjectBannerImagePrefix = "project-banners/";

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// All the project footer logo's have a prefix, so they can easily be distinguished from other images in the cloud storage.
    /// </summary>
    public const string ProjectFooterLogoImagePrefix = "project-footer-logos/";

    /// <author>Michiel Verschueren </author>
    /// <summary>
    /// Method that returns an unique filename for each user uploaded image
    /// </summary>
    /// <param name="project"><see cref="Project"/> is used to use its id in the filename</param>
    /// <returns>an unique filename that can be used to upload an image to google cloud</returns>
    public static string GenerateUserUploadedImageFileName(this Project project)
    {
        return UserUploadedImagePrefix + project.ProjectId + "/" + Guid.NewGuid();
    } // GenerateUserUploadedImageFileName.

    /// <author> Niels Van Steen </author>
    /// <summary>
    /// Returns the prefix all doc-review images have (only the images a <see cref="ProjectManager"/> uploaded in the doc-review text).
    /// </summary>
    /// <param name="project">The project to get the prefix for.</param>
    /// <returns></returns>
    public static string GetAllDocReviewImagesPrefixByProject(this Project project)
    {
        return UserUploadedImagePrefix + project.ProjectId;
    } // GetAllDocReviewImagesPrefixByProject.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the name of the image of this project's logo.
    /// Which is used to store the image.
    /// </summary>
    /// <remarks>
    /// For more info why this is done this way <see cref="ProjectLogoPrefix"/>.
    /// </remarks>
    public static string GetProjectLogoName(this Project project)
    {
        return ProjectLogoPrefix + project.ProjectId;
    } // GetProjectLogoPrefix.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get the entire link for a project logo.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public static string GetProjectLogoFullLink(this Project project, SquareImageSize size)
    {
        return ApplicationConstants.CloudStorageBasicUrl + project.GetProjectLogoName() + "_" + (int)size;
    } // GetProjectLogoFullLink.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the name of the image of this project's banner image.
    /// Which is used to store the image.
    /// </summary>
    /// <remarks>
    /// For more info why this is done this way <see cref="ProjectLogoPrefix"/>.
    /// </remarks>
    public static string GetProjectBannerImageName(this Project project)
    {
        return ProjectBannerImagePrefix + project.ProjectId;
    } // GetProjectLogoPrefix.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Get the entire link for a project banner iamge.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public static string GetProjectBannerImageFullLink(this Project project, LandscapeImageSize size)
    {
        return ApplicationConstants.CloudStorageBasicUrl + project.GetProjectBannerImageName() + "_" + (int)size;
    } // GetProjectLogoFullLink.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Returns the full link to a specific project footer logo.
    /// </summary>
    /// <returns></returns>
    public static string GetFooterLogoFullLink(this FooterLogo footerLogo, Project project, SquareImageSize size)
    {
        return ApplicationConstants.CloudStorageBasicUrl + footerLogo.ImageName + "_" + (int)size;
    } // GetFooterLogoPrefix.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Generates a footer logo name.
    /// </summary>
    /// <param name="footerLogo"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static string GenerateFooterLogoName(this FooterLogo footerLogo, Project project)
    {
        return ProjectFooterLogoImagePrefix + project.ProjectId + Guid.NewGuid();
    } // GetFooterLogoPrefix.


    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Returns the latest <see cref="ProjectHistory"/> of a <see cref="Project"/>
    /// </summary>
    /// <param name="project">The project ot get the projecthistory for</param>
    /// <returns></returns>
    public static ProjectHistory GetLatestProjectHistory(this Project project)
    {
        return project.ProjectHistories.MaxBy(p => p.EditedOn);
    } // GetLatestProjectHistory.

    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Check whether a <see cref="UserRole.RegularUser"/> is allowed to visit a project.
    /// </summary>
    /// <param name="project"></param>
    /// <returns>true = <see cref="UserRole.RegularUser"/> can view the project. </returns>
    public static bool IsProjectVisibleForNormalUsers(this Project project)
    {
        return project.GetLatestProjectHistory().ProjectStatus == ProjectStatus.Published;
    } // IsProjectVisibleForNormalUsers.
}