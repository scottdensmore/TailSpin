namespace Microsoft.Practices.Prism.Interactivity.InteractionRequest
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;

    /// <summary>
    ///   Displays a toast popup in response to a trigger event.
    /// </summary>
    public class ToastPopupAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        ///   The element name of the <see cref = "Popup" /> to show upon the interaction request.
        /// </summary>
        public static readonly DependencyProperty PopupElementNameProperty =
            DependencyProperty.Register(
                "PopupElementName",
                typeof(string),
                typeof(ToastPopupAction),
                new PropertyMetadata(null));

        private Timer closePopupTimer;

        /// <summary>
        ///   Gets or sets the name of the <see cref = "Popup" /> element to show when
        ///   an <see cref = "IInteractionRequest" /> is received.
        /// </summary>
        public string PopupElementName
        {
            get { return (string)this.GetValue(PopupElementNameProperty); }
            set { this.SetValue(PopupElementNameProperty, value); }
        }


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

            var popUp = (Popup)this.AssociatedObject.FindName(this.PopupElementName);
            popUp.DataContext = requestedEventArgs.Context;
            popUp.IsOpen = true;
            this.DisposeTimer();
            this.closePopupTimer = new Timer(
                s => Deployment.Current.Dispatcher.BeginInvoke(() => popUp.IsOpen = false),
                null,
                6000,
                5000);
            popUp.Closed += this.OnPopupClosed;
        }

        private void DisposeTimer()
        {
            lock (this)
            {
                if (this.closePopupTimer != null)
                {
                    this.closePopupTimer.Dispose();
                    this.closePopupTimer = null;
                }
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            this.DisposeTimer();
            ((Popup)sender).Closed -= this.OnPopupClosed;
        }
    }
}