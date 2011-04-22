namespace Microsoft.Practices.Prism.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class WeakDelegatesManager
    {
        private readonly List<DelegateReference> listeners = new List<DelegateReference>();

        public void AddListener(Delegate listener)
        {
            this.listeners.Add(new DelegateReference(listener, false));
        }

        public void Raise(params object[] args)
        {
            this.listeners.RemoveAll(listener => listener.Target == null);

            foreach (Delegate handler in this.listeners.ToList().Select(listener => listener.Target).Where(listener => listener != null))
            {
                handler.DynamicInvoke(args);
            }
        }

        public void RemoveListener(Delegate listener)
        {
            this.listeners.RemoveAll(reference =>
                                         {
                                             //Remove the listener, and prune collected listeners
                                             Delegate target = reference.Target;
                                             return listener.Equals(target) || target == null;
                                         });
        }
    }
}