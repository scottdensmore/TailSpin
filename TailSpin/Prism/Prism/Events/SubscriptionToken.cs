namespace Microsoft.Practices.Prism.Events
{
    using System;

    /// <summary>
    ///   Subscription token returned from <see cref = "EventBase" /> on subscribe.
    /// </summary>
    public class SubscriptionToken : IEquatable<SubscriptionToken>
    {
        private readonly Guid _token;

        /// <summary>
        ///   Initializes a new instance of <see cref = "SubscriptionToken" />.
        /// </summary>
        public SubscriptionToken()
        {
            this._token = Guid.NewGuid();
        }

        ///<summary>
        ///  Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        ///</summary>
        ///<returns>
        ///  true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        ///</returns>
        ///<param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. </param>
        ///<exception cref = "T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.</exception>
        ///<filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return this.Equals(obj as SubscriptionToken);
        }

        ///<summary>
        ///  Serves as a hash function for a particular type.
        ///</summary>
        ///<returns>
        ///  A hash code for the current <see cref = "T:System.Object" />.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this._token.GetHashCode();
        }

        ///<summary>
        ///  Indicates whether the current object is equal to another object of the same type.
        ///</summary>
        ///<returns>
        ///  <see langword = "true" /> if the current object is equal to the <paramref name = "other" /> parameter; otherwise, <see langword = "false" />.
        ///</returns>
        ///<param name = "other">An object to compare with this object.</param>
        public bool Equals(SubscriptionToken other)
        {
            if (other == null)
            {
                return false;
            }
            return Equals(this._token, other._token);
        }
    }
}