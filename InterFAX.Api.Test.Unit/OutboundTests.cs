using System;
using System.Globalization;
using System.Web;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class OutboundTests
    {
        private InterFAX _interfax;
        private MockHttpMessageHandler _handler;

        [Test]
        public void GetList_should_call_correct_uri_with_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes?lastId=5&limit=10&sortOrder=asc&userId=unit-test-associate")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetList(new Outbound.ListOptions
            {
                Limit = 10,
                LastId = 5,
                SortOrder = ListSortOrder.Ascending,
                UserId = "unit-test-associate"
            }).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetList_should_call_correct_uri_without_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetList().Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetFaxImageStream_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/image")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetFaxImageStream(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetFaxRecord_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetFaxRecord(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }
    }
}