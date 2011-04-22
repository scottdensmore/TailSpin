namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///   Associates a command to an <see cref = "ApplicationBarIconButton" />.
    /// </summary>
    [CLSCompliant(false)]
    public class ApplicationBarButtonCommand : Behavior<PhoneApplicationPage>
    {
        /// <summary>
        ///   The binding for <see cref = "ICommand" /> to invoke based on the ApplicationBarIconButton's events.
        /// </summary>
        public static readonly DependencyProperty CommandBindingProperty =
            DependencyProperty.Register("CommandBinding", typeof(Binding), typeof(ApplicationBarButtonCommand), new PropertyMetadata(HandleBindingChanged));

        ///<summary>
        ///  The parameter to use when calling methods on the <see cref = "ICommand" /> interface.
        ///</summary>
        public static readonly DependencyProperty CommandParameterBindingProperty =
            DependencyProperty.Register("CommandParameterBinding", typeof(Binding), typeof(ApplicationBarButtonCommand), new PropertyMetadata(HandleBindingChanged));

        private readonly BindingListener commandBindinglistener;
        private readonly BindingListener parameterBindinglistener;
        private ClickCommandBinding binding;

        /// <summary>
        ///   Instantiates a new instance of <see cref = "ApplicationBarButtonCommand" />.
        /// </summary>
        public ApplicationBarButtonCommand()
        {
            this.commandBindinglistener = new BindingListener(this.HandleCommandBindingValueChanged);
            this.parameterBindinglistener = new BindingListener(this.HandleCommandParameterBindingValueChanged);
        }

        /// <summary>
        ///   The text indicating which <see cref = "ApplicationBarIconButton" /> to bind with.
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        ///   The <see cref = "Binding" /> to use that results in an <see cref = "ICommand" />.
        /// </summary>
        public Binding CommandBinding
        {
            get { return (Binding)this.GetValue(CommandBindingProperty); }
            set { this.SetValue(CommandBindingProperty, value); }
        }

        /// <summary>
        ///   The <see cref = "Binding" /> for the command parameter to use with the <see cref = "CommandBinding" />.
        /// </summary>
        public Binding CommandParameterBinding
        {
            get { return (Binding)this.GetValue(CommandParameterBindingProperty); }
            set { this.SetValue(CommandParameterBindingProperty, value); }
        }

        /// <summary>
        ///   The <see cref = "ICommand" /> to bind to the <see cref = "ApplicationBarIconButton" />.
        /// </summary>
        protected ICommand Command { get; set; }

        /// <summary>
        ///   The value to use when calling <see cref = "Command" /> methods.
        /// </summary>
        protected object CommandParameter { get; set; }

        /// <summary>
        ///   Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        ///   Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            this.commandBindinglistener.Element = this.AssociatedObject;
            this.parameterBindinglistener.Element = this.AssociatedObject;
            this.CreateBinding();
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
            this.commandBindinglistener.Element = null;
            this.parameterBindinglistener.Element = null;
            base.OnDetaching();
        }

        /// <summary>
        ///   Invoked when the <see cref = "Binding" /> changes.
        /// </summary>
        /// <param name = "e"></param>
        protected void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == CommandBindingProperty)
            {
                this.commandBindinglistener.Binding = (Binding)e.NewValue;
            }

            if (e.Property == CommandParameterBindingProperty)
            {
                this.parameterBindinglistener.Binding = (Binding)e.NewValue;
            }

            this.CreateBinding();
        }

        private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ApplicationBarButtonCommand)sender).OnBindingChanged(e);
        }

        private void CreateBinding()
        {
            if (this.commandBindinglistener.Value != null)
            {
                if (this.binding != null)
                {
                    this.binding.Detach();
                }

                this.binding = new ClickCommandBinding(
                    this.AssociatedObject.ApplicationBar.FindButton(this.ButtonText),
                    (ICommand)this.commandBindinglistener.Value,
                    () => this.parameterBindinglistener.Value);
            }
        }

        private void HandleCommandBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
            this.CreateBinding();
        }

        private void HandleCommandParameterBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
            this.CreateBinding();
        }

        /// <summary>
        ///   Binds an <see cref = "ApplicationBarIconButton" /> to a <see cref = "ICommand" />.
        /// </summary>
        private class ClickCommandBinding
        {
            private readonly ICommand command;
            private readonly ApplicationBarIconButton iconButton;
            private readonly Func<object> parameterGetter;

            /// <summary>
            /// </summary>
            /// <param name = "iconButton"></param>
            /// <param name = "command"></param>
            /// <param name = "parameterGetter"></param>
            public ClickCommandBinding(ApplicationBarIconButton iconButton, ICommand command, Func<object> parameterGetter)
            {
                this.command = command;
                this.iconButton = iconButton;
                this.parameterGetter = parameterGetter;
                this.iconButton.IsEnabled = this.command.CanExecute(parameterGetter());

                this.command.CanExecuteChanged += this.CommandCanExecuteChanged;
                this.iconButton.Click += this.IconButtonClicked;
            }

            public void Detach()
            {
                this.iconButton.Click -= this.IconButtonClicked;
                this.command.CanExecuteChanged -= this.CommandCanExecuteChanged;
            }

            private void CommandCanExecuteChanged(object s, EventArgs ea)
            {
                this.iconButton.IsEnabled = this.command.CanExecute(this.parameterGetter());
            }

            private void IconButtonClicked(object s, EventArgs e)
            {
                this.command.Execute(this.parameterGetter());
            }
        }
    }
}