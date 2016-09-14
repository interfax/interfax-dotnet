using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    /// <summary>
    /// A fairly simplistic mock HttpMessageHandler for testing without going to the network.
    /// </summary>
    internal class MockHttpMessageHandler : HttpMessageHandler
    {
        public string ExpectedContent { get; set; } = "{}";
        public string ExpectedReasonPhrase { get; set; }
        public Uri ActualUri { get; private set; }
        public HttpMethod ActualHttpMethod { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; } = HttpStatusCode.OK;
        public Uri ExpectedUri { get; set; }
        public HttpMethod ExpectedHttpMethod { get; set; } = HttpMethod.Get;
        public string ExpectedContentType { get; set; } = "application/json";
        public Uri ExpectedLocationHeader { get; set; } = null;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ActualUri = request.RequestUri;
            ActualHttpMethod = request.Method;

            var responseMessage = new HttpResponseMessage(ExpectedStatusCode);
            if (ExpectedContent != null) responseMessage.Content = new StringContent(ExpectedContent);
            if (!string.IsNullOrEmpty(ExpectedReasonPhrase)) responseMessage.ReasonPhrase = ExpectedReasonPhrase;
            if (ExpectedLocationHeader != null) responseMessage.Headers.Location = ExpectedLocationHeader;
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(ExpectedContentType);

            return await Task.FromResult(responseMessage);
        }

        public bool ExpectedUriWasVisited()
        {
            var verified = ExpectedHttpMethod == ActualHttpMethod;

            verified = verified && ExpectedUri.AbsolutePath == ActualUri.AbsolutePath;

            var expectedQuery = HttpUtility.ParseQueryString(ExpectedUri.Query);
            var actualQuery = HttpUtility.ParseQueryString(ActualUri.Query);
            foreach (var option in expectedQuery.AllKeys)
            {
                verified = verified && actualQuery[option] != null && expectedQuery[option] == actualQuery[option];
            }
            return verified;
        }
    }
}
