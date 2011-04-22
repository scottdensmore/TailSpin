namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    ///   Represents an interaction request used for notifications.
    /// </summary>
    public class Notification
    {
        /// <summary>
        ///   Gets or sets the content of the notification.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        ///   Gets or sets the title to use for the notification.
        /// </summary>
        public string Title { get; set; }
    }
}