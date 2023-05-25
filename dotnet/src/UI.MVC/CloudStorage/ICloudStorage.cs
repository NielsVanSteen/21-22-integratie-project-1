using Google.Api.Gax;
using Google.Apis.Storage.v1.Data;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace UI.MVC.CloudStorage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Manages the file upload, delete permissions.
    /// </summary>
    public interface ICloudStorage
    {
        /// <author>Niels Van Steen</author>
        /// <summary>
        ///  Uploads a file to the cloud storage.
        /// </summary>
        /// <param name="file">The file that will be uploaded.</param>
        /// <param name="fileNameForStorage">The name used to store the file.</param>
        /// <param name="caching"> Information about the object's caching. See <see cref="CachingTime"/> for more information.</param>
        /// <returns></returns>
        /// <exception cref="Exception">When trying to delete an image that doesn't exist a 404 exception is thrown.</exception>
        public Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage, CachingTime caching = null);

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Deletes a file in the cloud storage.
        /// </summary>
        /// <param name="fileNameForStorage">The name used to store the file.</param>
        public Task DeleteFileAsync(string fileNameForStorage);

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Returns all the files given a prefix.
        /// </summary>
        /// <param name="prefix">This prefix can either by the name or a directory in the bucket.</param>
        /// <param name="delimiter">
        /// For example, given these objects:
        ///   a/1.txt
        ///   a/b/2.txt
        ///
        /// If you just specify prefix="a/", you'll get back:
        ///   a/1.txt
        ///   a/b/2.txt
        ///
        /// However, if you specify prefix="a/" and delimiter="/", you'll get back:
        ///   a/1.txt
        /// </param>
        /// <seealso ref="https://cloud.google.com/storage/docs/samples/storage-list-files-with-prefix"/>
        /// <returns></returns>
        public PagedEnumerable<Objects, Object> GetFilesByPrefix(string prefix, string delimiter = null);
    }
}