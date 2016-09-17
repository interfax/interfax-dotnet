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
        private readonly string _testPdf;

        public DocumentsTests()
        {
            var assembly = Assembly.GetAssembly(typeof(DocumentsTests));
            var assemblyPath = Path.GetDirectoryName(assembly.Location);

            // unpack test file
            _testPdf = Path.Combine(assemblyPath, "test.pdf");
            using (var resource = assembly.GetManifestResourceStream("InterFAX.Api.Test.Unit.test.pdf"))
            {
                using (var file = new FileStream(_testPdf, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
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

            var actual = _interfax.Outbound.Documents.GetUploadSessions(new Documents.ListOptions
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

            var actual = _interfax.Outbound.Documents.GetUploadSessions().Result;
            Assert.That(_handler.ExpectedUriWasVisited());
        }

        [Test]
        public void can_build_fax_document()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[{'MediaType': 'application/pdf','FileType': 'pdf'}]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/help/mediatype")
            };

            _interfax = new InterFAX("unit-test-user", "unit-test-pass", _handler);

            var faxDocument = _interfax.Documents.BuildFaxDocument(_testPdf);
            Assert.NotNull(faxDocument);
            var fileDocument = faxDocument as FileDocument;
            Assert.NotNull(fileDocument);
            Assert.AreEqual(_testPdf, fileDocument.FilePath);
            Assert.AreEqual("application/pdf", fileDocument.MediaType);
        }

        [Test]
        public void can_unpack_and_load_supported_media_types()
        {
            _interfax = new InterFAX("unit-test-user", "unit-test-pass");
            var types = _interfax.Documents.SupportedMediaTypes;
            Assert.NotNull(types);
            Assert.That(types.ContainsKey("pdf"));
        }
    }
}