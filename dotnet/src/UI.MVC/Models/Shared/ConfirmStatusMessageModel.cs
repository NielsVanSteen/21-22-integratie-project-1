namespace UI.MVC.Models.Shared
{
    /// <author>Niels Van Steen</author>
    /// <summary>
    /// Generic model that contains a status message. This can of a view types <see cref="NotificationType"/>.
    /// This model holds the values to show a bootstrap-alert popup.
    /// </summary>
    public class ConfirmStatusMessageModel
    {
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The Title of the alert-popup. This is a short title, that is shown in bold at the beginning of the alert box.
        /// </summary>
        public string Title { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The actual text of the alert-popup, this is shown after the bold title.
        /// </summary>
        public string Description { get; set; }
        
        /// <author>Niels Van Steen</author>
        /// <summary>
        /// This is the type of the alert box. <see cref="NotificationType"/>.
        /// </summary>
        public NotificationType.Type Type { get; set; }

        // Constructors.
        public ConfirmStatusMessageModel(string title, string description, NotificationType.Type type)
        {
            Title = title;
            Description = description;
            Type = type;
        } // ConfirmStatusMessageModel.
        public ConfirmStatusMessageModel() {}

        /// <author>Niels Van Steen</author>
        /// <summary>
        /// The 'enum' containing the different types of alert boxes.
        /// The reason a struct is used, is because unlike Java C# doesn't allow enums with parameters.
        /// The <see cref="TypeClasses"/> are the bootstrap classes to give the alert-popups their colors.
        /// </summary>
        public struct NotificationType
        {
            public static string[] TypeClasses = new string[]
            {
                "success", "info", "warning", "danger"
            };
            
            public enum Type : byte
            {
                Success = 0,
                Info = 1,
                Warning = 2,
                Error = 3
            }
        } // NotificationType.
    }
}