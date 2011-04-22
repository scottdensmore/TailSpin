namespace TailSpin.PhoneClient.Infrastructure
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Microsoft.Practices.Prism.Interactivity;

    public class ButtonCommand : Behavior<Button>
    {
        public static readonly DependencyProperty CommandParameterBindingProperty =
            DependencyProperty.Register("CommandParameterBinding", typeof(Binding), typeof(ButtonCommand), new PropertyMetadata(HandleBindingChanged));

        public static readonly DependencyProperty CommandBindingProperty =
            DependencyProperty.Register("CommandBinding", typeof(Binding), typeof(ButtonCommand), new PropertyMetadata(HandleBindingChanged));

        private readonly BindingListener commandBindinglistener;
        private readonly BindingListener parameterBindinglistener;
        private ButtonClickCommandBinding binding;

        public ButtonCommand()
        {
            this.commandBindinglistener = new BindingListener(this.HandleCommandBindingValueChanged);
            this.parameterBindinglistener = new BindingListener(this.HandleCommandParameterBindingValueChanged);    
        }

        public Binding CommandBinding
        {
            get { return (Binding)GetValue(CommandBindingProperty); }
            set { SetValue(CommandBindingProperty, value); }
        }

        public Binding CommandParameterBinding
        {
            get { return (Binding)GetValue(CommandParameterBindingProperty); }
            set { SetValue(CommandParameterBindingProperty, value); }
        }

        protected ICommand Command { get; set; }

        protected object CommandParameter { get; set; }

        protected override void OnAttached()
        {
            this.commandBindinglistener.Element = this.AssociatedObject;
            this.parameterBindinglistener.Element = this.AssociatedObject;
            this.CreateBinding();
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.commandBindinglistener.Element = null;
            this.parameterBindinglistener.Element = null;
            base.OnDetaching();
        }

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
            ((ButtonCommand)sender).OnBindingChanged(e);
        }

        private void HandleCommandBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
            this.CreateBinding();
        }

        private void HandleCommandParameterBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
            this.CreateBinding();
        }

        private void CreateBinding()
        {
            if (this.commandBindinglistener.Value != null)
            {
                if (this.binding != null)
                {
                    this.binding.Detach();
                }

                this.binding = new ButtonClickCommandBinding(
                    this.AssociatedObject,
                    (ICommand)this.commandBindinglistener.Value,
                    () => this.parameterBindinglistener.Value);
            }
        }

        public class ButtonClickCommandBinding
        {
            private readonly ICommand command;
            private readonly Button button;
            private readonly Func<object> parameterGetter;

            public ButtonClickCommandBinding(Button button, ICommand command, Func<object> parameterGetter)
            {
                this.command = command;
                this.button = button;
                this.parameterGetter = parameterGetter;

                this.command.CanExecuteChanged += this.CommandCanExecuteChanged;
                this.button.Click += this.IconButtonClicked;
                this.button.IsEnabled = this.command.CanExecute(this.parameterGetter());
            }

            public void Detach()
            {
                this.button.Click -= this.IconButtonClicked;
                this.command.CanExecuteChanged -= this.CommandCanExecuteChanged;
            }

            private void IconButtonClicked(object s, EventArgs e)
            {
                this.command.Execute(this.parameterGetter());
            }

            private void CommandCanExecuteChanged(object s, EventArgs ea)
            {
                this.button.IsEnabled = this.command.CanExecute(this.parameterGetter());
            }
        }
    }
}
