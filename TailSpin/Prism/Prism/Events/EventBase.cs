namespace Microsoft.Practices.Prism.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    ///<summary>
    ///  Defines a base class to publish and subscribe to events.
    ///</summary>
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        /// <summary>
        ///   Gets the list of current subscriptions.
        /// </summary>
        /// <value>The current subscribers.</value>
        protected ICollection<IEventSubscription> Subscriptions
        {
            get { return this._subscriptions; }
        }

        /// <summary>
        ///   Returns <see langword = "true" /> if there is a subscriber matching <see cref = "SubscriptionToken" />.
        /// </summary>
        /// <param name = "token">The <see cref = "SubscriptionToken" /> returned by <see cref = "EventBase" /> while subscribing to the event.</param>
        /// <returns><see langword = "true" /> if there is a <see cref = "SubscriptionToken" /> that matches; otherwise <see langword = "false" />.</returns>
        public virtual bool Contains(SubscriptionToken token)
        {
            lock (this.Subscriptions)
            {
                IEventSubscription subscription = this.Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                return subscription != null;
            }
        }

        /// <summary>
        ///   Removes the subscriber matching the <seealso cref = "SubscriptionToken" />.
        /// </summary>
        /// <param name = "token">The <see cref = "SubscriptionToken" /> returned by <see cref = "EventBase" /> while subscribing to the event.</param>
        public virtual void Unsubscribe(SubscriptionToken token)
        {
            lock (this.Subscriptions)
            {
                IEventSubscription subscription = this.Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                if (subscription != null)
                {
                    this.Subscriptions.Remove(subscription);
                }
            }
        }

        /// <summary>
        ///   Calls all the execution strategies exposed by the list of <see cref = "IEventSubscription" />.
        /// </summary>
        /// <param name = "arguments">The arguments that will be passed to the listeners.</param>
        /// <remarks>
        ///   Before executing the strategies, this class will prune all the subscribers from the
        ///   list that return a <see langword = "null" /> <see cref = "Action{T}" /> when calling the
        ///   <see cref = "IEventSubscription.GetExecutionStrategy" /> method.
        /// </remarks>
        protected virtual void InternalPublish(params object[] arguments)
        {
            List<Action<object[]>> executionStrategies = this.PruneAndReturnStrategies();
            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(arguments);
            }
        }

        /// <summary>
        ///   Adds the specified <see cref = "IEventSubscription" /> to the subscribers' collection.
        /// </summary>
        /// <param name = "eventSubscription">The subscriber.</param>
        /// <returns>The <see cref = "SubscriptionToken" /> that uniquely identifies every subscriber.</returns>
        /// <remarks>
        ///   Adds the subscription to the internal list and assigns it a new <see cref = "SubscriptionToken" />.
        /// </remarks>
        protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (eventSubscription == null)
            {
                throw new ArgumentNullException("eventSubscription");
            }

            eventSubscription.SubscriptionToken = new SubscriptionToken();
            lock (this.Subscriptions)
            {
                this.Subscriptions.Add(eventSubscription);
            }
            return eventSubscription.SubscriptionToken;
        }

        private List<Action<object[]>> PruneAndReturnStrategies()
        {
            List<Action<object[]>> returnList = new List<Action<object[]>>();

            lock (this.Subscriptions)
            {
                for (var i = this.Subscriptions.Count - 1; i >= 0; i--)
                {
                    Action<object[]> listItem =
                        this._subscriptions[i].GetExecutionStrategy();

                    if (listItem == null)
                    {
                        // Prune from main list. Log?
                        this._subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }
    }
}