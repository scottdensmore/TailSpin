namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    /// <summary>
    ///   Represents an interaction request used for confirmations.
    /// </summary>
    public class Confirmation : Notification
    {
        /// <summary>
        ///   Gets or sets a value indicating that the confirmation is confirmed.
        /// </summary>
        public bool Confirmed { get; set; }
    }
}