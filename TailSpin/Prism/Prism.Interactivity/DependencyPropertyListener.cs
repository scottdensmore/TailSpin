namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    ///   Registers a new dependency property for tracking data and provides
    ///   notification on data changes.
    /// </summary>
    public class DependencyPropertyListener
    {
        private static int index;
        private readonly DependencyProperty property;
        private FrameworkElement target;

        /// <summary>
        ///   Instantiates a new <see cref = "DependencyPropertyListener" />.
        /// </summary>
        /// <remarks>
        ///   This registers creates an attached property with the name starting DependencyPropertyListener.  This
        ///   attached property set on a <see cref = "FrameworkElement" /> when <see cref = "Attach" /> is called.
        /// </remarks>
        public DependencyPropertyListener()
        {
            this.property =
                DependencyProperty.RegisterAttached(
                    "DependencyPropertyListener" + index++,
                    typeof(object),
                    typeof(DependencyPropertyListener),
                    new PropertyMetadata(null, this.HandleValueChanged));
        }

        /// <summary>
        ///   This event is raised when the attached property value changes.
        /// </summary>
        public event EventHandler<BindingChangedEventArgs> Changed;

        /// <summary>
        ///   Attaches a <see cref = "DependencyProperty" /> to a framework element with
        ///   the provided <see cref = "Binding" />.
        /// </summary>
        /// <param name = "element">The <see cref = "FrameworkElement" /> to attach the monitoring dependency property to.</param>
        /// <param name = "binding">The binding to use with the attached property.</param>
        public void Attach(FrameworkElement element, Binding binding)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (this.target != null)
            {
                throw new InvalidOperationException("Cannot attach an already attached listener");
            }

            this.target = element;

            this.target.SetBinding(this.property, binding);
        }

        ///<summary>
        ///  Detaches binding listener from target.
        ///</summary>
        public void Detach()
        {
            this.target.ClearValue(this.property);
            this.target = null;
        }

        private void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(this, new BindingChangedEventArgs(e));
            }
        }
    }
}