namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    ///   Displays a message box with the content of the <see cref = "InteractionRequestedEventArgs" />
    ///   as the item.
    /// </summary>
    public class MessageBoxAction : TriggerAction<DependencyObject>
    {
        /// <summary>
        ///   Invokes the action.
        /// </summary>
        /// <param name = "parameter">The parameter to the action. If the Action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            var requestedEventArgs = parameter as InteractionRequestedEventArgs;
            if (requestedEventArgs == null)
            {
                return;
            }

            MessageBox.Show((string)requestedEventArgs.Context.Content, requestedEventArgs.Context.Title, MessageBoxButton.OK);
            if (requestedEventArgs.Callback != null)
            {
                requestedEventArgs.Callback.Invoke();
            }
        }
    }
}