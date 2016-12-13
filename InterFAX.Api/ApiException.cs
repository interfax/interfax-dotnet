using System;
using System.Net;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    /// <summary>
    /// Instances of this exception contain information about what went wrong with the request made.
    /// </summary>
    /// TODO : This is actually slightly clunky to use as it gets packaged up in an AggregateException. Could be improved by modifying requests to take a callback function?
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public Error Error { get; private set; }

        internal ApiException(HttpStatusCode statusCode, Error error) : base($"ApiException : {error}")
        {
            StatusCode = statusCode;
            Error = error;
        }
    }
}