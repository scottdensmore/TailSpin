namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Windows.Interactivity;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///   Subscribes to a <see cref = "ApplicationBarIconButton" /> click event and
    ///   navigates on the event.
    /// </summary>
    [CLSCompliant(false)]
    public class ApplicationBarButtonNavigation : Behavior<PhoneApplicationPage>
    {
        private ApplicationBarIconButton button;

        /// <summary>
        ///   The button text on the <see cref = "ApplicationBarIconButton" /> to monitor for click events.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        ///   The <see cref = "Uri" /> to navigate to.
        /// </summary>
        public Uri NavigateTo { get; set; }

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            this.button = this.AssociatedObject.ApplicationBar.FindButton(this.ButtonText);
            if (this.button != null)
            {
                this.button.Click += this.IconButtonClicked;
            }
            base.OnAttached();
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            if (this.button != null)
            {
                this.button.Click -= this.IconButtonClicked;
            }
            base.OnDetaching();
        }

        private void IconButtonClicked(object s, EventArgs e)
        {
            if (this.NavigateTo.ToString().Equals("#GoBack"))
            {
                this.AssociatedObject.NavigationService.GoBack();
            }
            else
            {
                this.AssociatedObject.NavigationService.Navigate(this.NavigateTo);
            }
        }
    }
}