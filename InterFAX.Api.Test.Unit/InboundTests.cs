using System;
using System.Globalization;
using System.Net.Http;
using System.Web;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class InboundTests
    {
        private FaxClient _interfax;
        private MockHttpMessageHandler _handler;

        [Test]
        public void GetList_should_call_correct_uri_with_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes?limit=10&lastId=5&unreadOnly=true&allUsers=true")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetList(new Inbound.ListOptions
            {
                Limit = 10,
                LastId = 5,
                UnreadOnly = true,
                AllUsers = true
            }).Result;

            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetList_should_call_correct_uri_without_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetList().Result;
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

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetFaxRecord(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetFaxImageStream_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1/image")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetFaxImageStream(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void GetForwardingEmails_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1/emails")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetForwardingEmails(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void MarkRead_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1/mark")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var response = _interfax.Inbound.MarkRead(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void MarkUnread_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1/mark?unread=true")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var response = _interfax.Inbound.MarkUnread(1).Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }
    }
}