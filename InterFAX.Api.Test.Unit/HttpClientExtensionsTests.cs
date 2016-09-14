using System;
using System.Globalization;
using System.Net;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class HttpClientExtensionsTests
    {
        private InterFAX _interfax;
        private MockHttpMessageHandler _handler;

        [Test]
        public void should_throw_exception_on_error()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedReasonPhrase = "Something was not found.",
                ExpectedStatusCode = HttpStatusCode.NotFound,
                ExpectedUri = new Uri("https://rest.interfax.net/accounts/self/ppcards/balance")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var exception = Assert.Throws<AggregateException>(() => { var balance = _interfax.Account.GetBalance().Result; });
            Assert.AreEqual(1, exception.InnerExceptions.Count);

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);
            Assert.AreEqual(_handler.ExpectedReasonPhrase, apiException.ReasonPhrase);
        }
    }
}