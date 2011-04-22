namespace Microsoft.Practices.Prism.Events
{
    using System;
    using System.Reflection;

    /// <summary>
    ///   Represents a reference to a <see cref = "Delegate" /> that may contain a
    ///   <see cref = "WeakReference" /> to the target. This class is used
    ///   internally by the Composite Application Library.
    /// </summary>
    public class DelegateReference : IDelegateReference
    {
        private readonly Delegate _delegate;
        private readonly Type _delegateType;
        private readonly MethodInfo _method;
        private readonly WeakReference _weakReference;

        /// <summary>
        ///   Initializes a new instance of <see cref = "DelegateReference" />.
        /// </summary>
        /// <param name = "delegate">The original <see cref = "Delegate" /> to create a reference for.</param>
        /// <param name = "keepReferenceAlive">If <see langword = "false" /> the class will create a weak reference to the delegate, allowing it to be garbage collected. Otherwise it will keep a strong reference to the target.</param>
        /// <exception cref = "ArgumentNullException">If the passed <paramref name = "delegate" /> is not assignable to <see cref = "Delegate" />.</exception>
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            if (keepReferenceAlive)
            {
                this._delegate = @delegate;
            }
            else
            {
                this._weakReference = new WeakReference(@delegate.Target);
                this._method = @delegate.Method;
                this._delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        ///   Gets the <see cref = "Delegate" /> (the target) referenced by the current <see cref = "DelegateReference" /> object.
        /// </summary>
        /// <value><see langword = "null" /> if the object referenced by the current <see cref = "DelegateReference" /> object has been garbage collected; otherwise, a reference to the <see cref = "Delegate" /> referenced by the current <see cref = "DelegateReference" /> object.</value>
        public Delegate Target
        {
            get
            {
                if (this._delegate != null)
                {
                    return this._delegate;
                }
                else
                {
                    return this.TryGetDelegate();
                }
            }
        }

        private Delegate TryGetDelegate()
        {
            if (this._method.IsStatic)
            {
                return Delegate.CreateDelegate(this._delegateType, null, this._method);
            }
            object target = this._weakReference.Target;
            if (target != null)
            {
                return Delegate.CreateDelegate(this._delegateType, target, this._method);
            }
            return null;
        }
    }
}