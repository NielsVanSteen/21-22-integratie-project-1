using System.Text;

namespace UI.MVC.CloudStorage
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// This class is used to specify the time an image is cached by the Cloud Storage.
    /// </summary>
    public class CachingTime
    {
        // Properties.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This property includes the caching time of the image.
        /// <see cref="CacheDefaults"/> for more information.
        /// </summary>
        public CacheDefaults? CachingDefault { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// If the default caching times provided by <see cref="CacheDefaults"/> are not flexible enough you can provide your own caching time.
        /// The value works with decimal values. E.g., 0.25 will amount to 15 seconds.
        /// </summary>
        public decimal Minutes { get; set; }
        
        // Constructors.
        public CachingTime(decimal minutes)
        {
            Minutes = minutes;
        } // CacheControlTime.
        
        public CachingTime(CacheDefaults cachingDefault)
        {
            CachingDefault = cachingDefault;
        } // CacheControlTime.
        
        // Methods.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Calculates the caching time based on <see cref="Minutes"/> as well as <see cref="CachingDefault"/>
        /// </summary>
        /// <returns>Returns the time an object should be cached. Unit is in seconds.</returns>
        public int GetCachingTime()
        {
            if (CachingDefault != null)
                return ((int) CachingDefault) * 60;

            return (int)(Minutes * 60);
        } // GetCachingTime.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Creates the <see cref="Google.Apis.Storage.v1.Data.Object.CacheControl"/> based on the <see cref="Minutes"/> and <see cref="CachingDefault"/>
        /// </summary>
        /// <returns>the <see cref="Google.Apis.Storage.v1.Data.Object.CacheControl"/> string readable by the Cloud provider. Only used When uploading a file.</returns>
        public string GetCachingStringToUploadObject()
        {
            var cachingTime = GetCachingTime();
            
            // public = object can be cached anywhere
            // no-cache = object CAN be cached but not can't be used to satisfy future requests unless first validated by Cloud Storage.
            // -> visis the link in the <remarks> part for the full documentation provided by Google.
            StringBuilder sb = new StringBuilder("public,no-cache");

            // Make age specifies the caching time in seconds.
            sb.Append(",max-age=" + cachingTime);
            
            // If the cachingTime is 0, add no-store -> the image won't be cached.
            if (cachingTime == 0)
                sb.Append(",no-store");

            // Return the string.
            return sb.ToString();
        } // GetCachingString.

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// Returns the caching string to delete an object. -> this string will always disable caching so a deleted object won't be cached somewhere after it's been deleted.
        /// </summary>
        /// <remarks>
        /// https://cloud.google.com/storage/docs/metadata#cache-control
        /// </remarks>
        /// <returns>the <see cref="Google.Apis.Storage.v1.Data.Object.CacheControl"/> string readable by the Cloud provider. Only used when deleting an object.</returns>
        public static string GetCachingStringToDeleteObject()
        {
            // Brief explanation below. Visit the link in the <remarks> tag for the full documentation on the Google website.
            // public = object can be cached anywhere.
            // no-cache = object CAN be cached but can't be used to satisfy future request unless first validated by Cloud Storage.
            // no-store = object can't be cached -> is only done when deleting the object.
            // max-age = the max age in seconds an object can be stored. -> is set to 0 when deleting the object so it won't remain in cache anymore.
            return "public,no-cache,no-store,max-age=0";
        } // GetCachingStringToDeleteObject.
        
        // Enum.
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This enum contains a few caching defaults.
        /// These default are in minutes.
        /// </summary>
        /// <remarks>
        /// https://cloud.google.com/storage/docs/metadata#cache-control
        /// </remarks>
        public enum CacheDefaults : byte
        {
            /// <author>Niels Van Steen</author>
            /// <summary>
            /// The image won't be cached at all. -> caching time is 0.
            /// </summary>
            NoCache = 0,
            
            /// <author>Niels Van Steen</author>
            /// <summary>
            /// The image will be cached for 5 minutes.
            /// </summary>
            Short = 5,
            
            /// <author>Niels Van Steen</author>
            /// <summary>
            /// The image will be cached for 15 minutes.
            /// </summary>
            Default = 15,
            
            /// <author>Niels Van Steen</author>
            /// <summary>
            /// The image will be cached for an hour.
            /// </summary>
            Long = 60
        } // CacheControlTimeDefaults.
    }
}