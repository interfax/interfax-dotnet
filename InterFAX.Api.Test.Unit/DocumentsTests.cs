using System;
using System.Globalization;
using System.Net.Http;
using System.Web;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class DocumentsTests
    {
        private InterFAX _interfax;
        private MockHttpMessageHandler _handler;

        [Test]
        public void GetList_should_call_correct_uri_with_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/documents?limit=10&offset=5")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.Documents.GetList(new Documents.ListOptions
            {
                Limit = 10,
                Offset = 5
            }).Result;

            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetList_should_call_correct_uri_without_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/documents")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.Documents.GetList().Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

    }
}