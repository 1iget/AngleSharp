﻿namespace AngleSharp.Events
{
    using AngleSharp.Dom.Events;
    using AngleSharp.Network;

    /// <summary>
    /// The event that is published in case of new request.
    /// </summary>
    public class RequestStartEvent : Event
    {
        /// <summary>
        /// Creates a new event for starting a request.
        /// </summary>
        /// <param name="requester">The associated requester.</param>
        /// <param name="request">The data of the request.</param>
        public RequestStartEvent(IRequester requester, IRequest request)
        {
            Requester = requester;
            Request = request;
        }

        /// <summary>
        /// Gets the associated requester.
        /// </summary>
        public IRequester Requester
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the request data to transmit.
        /// </summary>
        public IRequest Request
        {
            get;
            private set;
        }
    }
}
