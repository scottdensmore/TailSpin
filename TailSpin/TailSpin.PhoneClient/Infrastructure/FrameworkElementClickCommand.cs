namespace TailSpin.PhoneClient.Infrastructure
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Microsoft.Practices.Prism.Interactivity;

    public class FrameworkElementClickCommand : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty CommandBindingProperty =
            DependencyProperty.Register("CommandBinding", typeof(Binding), typeof(FrameworkElementClickCommand), new PropertyMetadata(HandleBindingChanged));

        private readonly BindingListener commandBindinglistener;

        private StylusPoint mouseDownPoint;
        private int mouseDownTicks;

        public FrameworkElementClickCommand()
        {
            this.commandBindinglistener = new BindingListener(CommandBindingValueChanged);
        }

        public Binding CommandBinding
        {
            get { return (Binding)GetValue(CommandBindingProperty); }
            set { SetValue(CommandBindingProperty, value); }
        }

        protected override void OnAttached()
        {
            this.commandBindinglistener.Element = this.AssociatedObject;
            this.AssociatedObject.MouseLeftButtonDown += this.ListBoxMouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp += this.ListBoxMouseLeftButtonUp;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseLeftButtonDown -= this.ListBoxMouseLeftButtonDown;
            this.AssociatedObject.MouseLeftButtonUp -= this.ListBoxMouseLeftButtonUp;
            base.OnDetaching();
        }

        protected void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            this.commandBindinglistener.Binding = (Binding)e.NewValue;
        }

        private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((FrameworkElementClickCommand)sender).OnBindingChanged(e);
        }

        private static void CommandBindingValueChanged(object sender, BindingChangedEventArgs e)
        {
        }

        private void ListBoxMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.mouseDownPoint = e.StylusDevice.GetStylusPoints(this.AssociatedObject).First();
            this.mouseDownTicks = Environment.TickCount;
        }

        private void ListBoxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mouseUpPoint = e.StylusDevice.GetStylusPoints(this.AssociatedObject).First();
            if (Math.Sqrt(Math.Pow(mouseUpPoint.X - this.mouseDownPoint.X, 2) + Math.Pow(mouseUpPoint.Y - this.mouseDownPoint.Y, 2)) <= 20
                && Environment.TickCount - this.mouseDownTicks < 450)
            {
                // Invoke Command
                ((ICommand)this.commandBindinglistener.Value).Execute(null);
            }
        }
    }
}
