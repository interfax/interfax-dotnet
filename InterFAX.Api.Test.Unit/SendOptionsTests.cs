using System;
using InterFAX.Api.Dtos;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class SendOptionsTests
    {
        [Test]
        public void should_return_dictionary_of_options()
        {
            var options = new SendOptions
            {
                FaxNumber = "+1234567890",
                Contact = "unit-test-contact",
                PostponeTime = new DateTime(2016, 6, 1, 14, 30, 30),
                RetriesToPerform = 5,
                Csid = "unit-test-csid",
                PageHeader = "unit-test-page-header",
                Reference = "unit-test-reference",
                ReplyAddress = "unit-test-reply-address",
                PageSize = PageSize.A4,
                ShouldScale = true,
                PageOrientation = PageOrientation.Portrait,
                PageResolution = PageResolution.Standard,
                PageRendering = PageRendering.GreyScale
            };

            var actual = options.ToDictionary();
            Assert.AreEqual(13, actual.Keys.Count);

            var key = "faxNumber";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.FaxNumber, actual[key]);

            key = "contact";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Contact, actual[key]);

            key = "postponeTime";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("2016-06-01T14:30:30Z", actual[key]);

            key = "retriesToPerform";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("5", actual[key]);

            key = "csid";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Csid, actual[key]);

            key = "pageHeader";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.PageHeader, actual[key]);

            key = "reference";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Reference, actual[key]);

            key = "replyAddress";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.ReplyAddress, actual[key]);

            key = "pageSize";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("a4", actual[key]);

            key = "fitToPage";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("scale", actual[key]);

            key = "pageOrientation";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("portrait", actual[key]);

            key = "resolution";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("standard", actual[key]);

            key = "rendering";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("greyscale", actual[key]);
        }

        [Test]
        public void should_return_partial_dictionary_of_options()
        {
            var options = new SendOptions
            {
                FaxNumber = "+1234567890",
                Contact = "unit-test-contact",
                PostponeTime = new DateTime(2016, 6, 1, 14, 30, 30),
                RetriesToPerform = 5,
                Csid = "unit-test-csid"
            };

            var actual = options.ToDictionary();
            Assert.AreEqual(5, actual.Keys.Count);

            var key = "faxNumber";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.FaxNumber, actual[key]);

            key = "contact";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Contact, actual[key]);

            key = "postponeTime";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("2016-06-01T14:30:30Z", actual[key]);

            key = "retriesToPerform";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("5", actual[key]);

            key = "csid";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Csid, actual[key]);
        }
    }
}