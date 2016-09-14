using System;
using System.Net;
using System.Net.Http;

namespace InterFAX.Api
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ReasonPhrase { get; private set; }

        public ApiException(HttpStatusCode statusCode, string reasonPhrase)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }
    }
}