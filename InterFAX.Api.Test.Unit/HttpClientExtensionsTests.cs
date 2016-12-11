using System;
using System.Globalization;
using System.Net;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class HttpClientExtensionsTests
    {
        private FaxClient _interfax;
        private MockHttpMessageHandler _handler;

        [Test]
        public void should_create_HttpError_from_error_response()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "Not json...",
                ExpectedReasonPhrase = "Something was not found.",
                ExpectedContentType = "text/plain",
                ExpectedStatusCode = HttpStatusCode.NotFound,
                ExpectedUri = new Uri("https://rest.interfax.net/accounts/self/ppcards/balance")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var exception = Assert.Throws<AggregateException>(() => { var balance = _interfax.Account.GetBalance().Result; });
            Assert.AreEqual(1, exception.InnerExceptions.Count);

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);
            Assert.AreEqual((int) _handler.ExpectedStatusCode, apiException.Error.Code);
            Assert.AreEqual(_handler.ExpectedReasonPhrase, apiException.Error.Message);
            Assert.AreEqual(_handler.ExpectedContent, apiException.Error.MoreInfo);
        }

        [Test]
        public void should_create_HttpError_from_empty_error_response()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "",
                ExpectedReasonPhrase = "Something was not found.",
                ExpectedContentType = "text/plain",
                ExpectedStatusCode = HttpStatusCode.NotFound,
                ExpectedUri = new Uri("https://rest.interfax.net/accounts/self/ppcards/balance")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var exception = Assert.Throws<AggregateException>(() => { var balance = _interfax.Account.GetBalance().Result; });
            Assert.AreEqual(1, exception.InnerExceptions.Count);

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);
            Assert.AreEqual((int) _handler.ExpectedStatusCode, apiException.Error.Code);
            Assert.AreEqual(_handler.ExpectedReasonPhrase, apiException.Error.Message);
            Assert.AreEqual("No content returned", apiException.Error.MoreInfo);
        }
    }
}