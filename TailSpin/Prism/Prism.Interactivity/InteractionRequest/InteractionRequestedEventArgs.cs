namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    using System;

    /// <summary>
    ///   Event args for the <see cref = "IInteractionRequest.Raised" /> event.
    /// </summary>
    public class InteractionRequestedEventArgs : EventArgs
    {
        /// <summary>
        ///   Constructs a new instance of <see cref = "InteractionRequestedEventArgs" />
        /// </summary>
        /// <param name = "context"></param>
        /// <param name = "callback"></param>
        public InteractionRequestedEventArgs(Notification context, Action callback)
        {
            this.Context = context;
            this.Callback = callback;
        }

        /// <summary>
        ///   Gets the callback to execute when an interaction is completed.
        /// </summary>
        public Action Callback { get; private set; }

        /// <summary>
        ///   Gets the context for a requested interaction.
        /// </summary>
        public Notification Context { get; private set; }
    }
}