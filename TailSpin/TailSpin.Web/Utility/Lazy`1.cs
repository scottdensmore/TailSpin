namespace TailSpin.Web.Utility
{
    using System;

    internal sealed class Lazy<T>
    {
        private readonly Func<T> creator;
        private readonly object lockObj = new object();
        private bool hasExecuted;
        private T result;

        public Lazy(Func<T> creator)
        {
            this.creator = creator;
        }

        public T Eval()
        {
            if (!hasExecuted)
            {
                lock (lockObj)
                {
                    if (!hasExecuted)
                    {
                        result = creator();
                        hasExecuted = true;
                    }
                }
            }

            return result;
        }
    }
}