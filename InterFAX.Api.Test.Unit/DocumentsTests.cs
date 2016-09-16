using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class DocumentsTests
    {
        private InterFAX _interfax;
        private MockHttpMessageHandler _handler;
        private readonly string _testPath;

        public DocumentsTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }

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

        [Test]
        public void can_build_fax_document()
        {
            var handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[{'MediaType': 'application/pdf','FileType': 'pdf'}]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/help/mediatype")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var filePath = Path.Combine(_testPath, "test.pdf");
            var faxDocument = _interfax.Documents.BuildFaxDocument(Path.Combine(_testPath, "test.pdf"));
            Assert.NotNull(faxDocument);
            var fileDocument = faxDocument as FileDocument;
            Assert.NotNull(fileDocument);
            Assert.AreEqual(filePath, fileDocument.FilePath);
            Assert.AreEqual("application/pdf", fileDocument.MediaType);
        }
    }
}