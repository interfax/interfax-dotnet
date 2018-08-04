using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class OutboundTests
    {
        private FaxClient _interfax;
        private MockHttpMessageHandler _handler;

        [TestMethod]
        public void GetList_should_call_correct_uri_with_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes?lastId=5&limit=10&sortOrder=asc&userId=unit-test-associate")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetList(new Outbound.ListOptions
            {
                Limit = 10,
                LastId = 5,
                SortOrder = ListSortOrder.Ascending,
                UserId = "unit-test-associate"
            }).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void GetList_should_call_correct_uri_without_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetList().Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void GetFaxImageStream_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/image")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.GetFaxImageStream(1).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void GetFaxRecord_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "{}",
                ExpectedUri = new Uri("https://rest.interfax.net/inbound/faxes/1")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Inbound.GetFaxRecord(1).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void CancelFax_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/cancel")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var response = _interfax.Outbound.CancelFax(1).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void ResendFax_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedLocationHeader = new Uri("https://rest.interfax.net/outbound/faxes/2"),
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/resend")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var faxId = _interfax.Outbound.ResendFax(1).Result;
            Assert.AreEqual(2, faxId);
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void ResendFax_should_call_correct_uri_with_faxNumber()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedLocationHeader = new Uri("https://rest.interfax.net/outbound/faxes/2"),
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/resend?faxNumber=123456789")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var faxId = _interfax.Outbound.ResendFax(1, "123456789").Result;
            Assert.AreEqual(2, faxId);
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void ResendFax_returns_fax_id()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedStatusCode = HttpStatusCode.Created,
                ExpectedLocationHeader = new Uri("https://rest.interfax.net/outbound/faxes/1"),
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/resend?faxNumber=123456789")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var faxId = _interfax.Outbound.ResendFax(1, "123456789").Result;
            Assert.AreEqual(1, faxId);
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void HideFax_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Post,
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/1/hide")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var response = _interfax.Outbound.HideFax(1).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void GetCompleted_should_call_correct_uri()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedHttpMethod = HttpMethod.Get,
                ExpectedContent = "",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/faxes/completed?ids=1,2,3")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var response = _interfax.Outbound.GetCompleted(1, 2, 3).Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }
    }
}