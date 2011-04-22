namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Interactivity;

    /// <summary>
    ///   Custom behavior that updates the source of a binding on a password box as the text changes.
    /// </summary>
    public class UpdatePasswordBindingOnPropertyChanged : Behavior<PasswordBox>
    {
        private BindingExpression expression;

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.expression = this.AssociatedObject.GetBindingExpression(PasswordBox.PasswordProperty);
            this.AssociatedObject.PasswordChanged += this.OnPasswordChanged;
        }

        /// <summary>
        ///   Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>
        ///   Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.PasswordChanged -= this.OnPasswordChanged;
            this.expression = null;
        }

        private void OnPasswordChanged(object sender, EventArgs args)
        {
            this.expression.UpdateSource();
        }
    }
}