namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Windows;

    ///<summary>
    ///  The event arguments for the <see cref = "ChangedHandler" /> delegate.
    ///</summary>
    public class BindingChangedEventArgs : EventArgs
    {
        /// <summary>
        ///   Instantiates a new instance of <see cref = "BindingChangedEventArgs" />.
        /// </summary>
        /// <param name = "e"></param>
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            this.EventArgs = e;
        }

        ///<summary>
        ///  Gets the underlying <see cref = "DependencyPropertyChangedEventArgs" />.
        ///</summary>
        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}