using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace UI.MVC.CloudStorage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// <see cref="ICloudStorage"/>
    /// </summary>
    public class GoogleCloudStorage : ICloudStorage
    {
        // Fields.
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        // Constructor.
        public GoogleCloudStorage(IConfiguration configuration, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                var googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GOOGLE_CREDENTIAL_FILE"));
                _storageClient = StorageClient.Create(googleCredential);
            }
            else if (env.IsProduction())
            {
                _storageClient = StorageClient.Create();
            }
            _bucketName = configuration.GetValue<string>("GOOGLE_CLOUD_STORAGE_BUCKET");
        } // GoogleCloudStorage.

        // Methods.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// <see cref="ICloudStorage.UploadFileAsync"/>
        /// </summary>
        public async Task<string> UploadFileAsync(IFormFile file, string fileNameForStorage, CachingTime caching)
        {
            // If caching is not passed, use the default length.
            caching ??= new CachingTime(CachingTime.CacheDefaults.Default);

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var obj = new Google.Apis.Storage.v1.Data.Object()
            {
                Name = fileNameForStorage, // Name of the image in Cloud Storage
                Bucket = _bucketName, // Name of the bucket.
                Id = _bucketName, // Might be optional?
                CacheControl = caching.GetCachingStringToUploadObject(),
                ContentType = file.ContentType
            };

            var dataObject = await _storageClient.UploadObjectAsync(obj, memoryStream);
            return dataObject.MediaLink;
        } // UploadFileAsync.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// <see cref="ICloudStorage.DeleteFileAsync"/>
        /// </summary>
        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            var obj = new Google.Apis.Storage.v1.Data.Object()
            {
                Name = fileNameForStorage, // Name of the image in Cloud Storage
                Bucket = _bucketName, // Name of the bucket.
                Id = _bucketName, // Might be optional?
                CacheControl = CachingTime.GetCachingStringToDeleteObject()
            };

            await _storageClient.DeleteObjectAsync(obj);
        } // DeleteFileAsync.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// <see cref="ICloudStorage.DeleteFileAsync"/>
        /// </summary>
        public PagedEnumerable<Objects, Object> GetFilesByPrefix(string prefix, string delimiter = null)
        {
            var options = new ListObjectsOptions { Delimiter = delimiter };
            return _storageClient.ListObjects(_bucketName, prefix, options);
        } // GetFilesByPrefix.
    }
}