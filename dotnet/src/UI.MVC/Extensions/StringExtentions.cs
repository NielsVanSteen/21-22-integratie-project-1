namespace UI.MVC.Extensions;

/// <summary>
/// Class that holds extensions for the <see cref="string"/> type
/// </summary>
public static class StringExtentions
{
    /// <author> Michiel Verschueren </author>
    /// <summary>
    /// Extension method that gets the full link for the request to the google api
    /// </summary>
    /// <param name="fileId">The fileId that is entered by the user</param>
    /// <returns></returns>
    public static string GetExportUrl(this string fileId)
    {
        return $"https://www.googleapis.com/drive/v3/files/{fileId}/export?mimeType=text%2Fhtml";
    }
}