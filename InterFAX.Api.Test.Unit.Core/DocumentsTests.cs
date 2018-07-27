using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class DocumentsTests
    {
        private FaxClient _interfax;
        private MockHttpMessageHandler _handler;
        private readonly string _testPdf;

        public DocumentsTests()
        {
            var assembly = Assembly.GetAssembly(typeof(DocumentsTests));
            var assemblyPath = Path.GetDirectoryName(assembly.Location);

            // unpack test file
            _testPdf = Path.Combine(assemblyPath, "test.pdf");
            using (var resource = assembly.GetManifestResourceStream("InterFAX.Api.Test.Unit.Core.test.pdf"))
            {
                using (var file = new FileStream(_testPdf, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        [TestMethod]
        public void GetList_should_call_correct_uri_with_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/documents?limit=10&offset=5")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.Documents.GetUploadSessions(new Documents.ListOptions
            {
                Limit = 10,
                Offset = 5
            }).Result;

            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void GetList_should_call_correct_uri_without_options()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/documents")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Outbound.Documents.GetUploadSessions().Result;
            Assert.IsTrue(_handler.ExpectedUriWasVisited());
        }

        [TestMethod]
        public void can_build_fax_document()
        {
            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[{'MediaType': 'application/pdf','FileType': 'pdf'}]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/help/mediatype")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var faxDocument = _interfax.Documents.BuildFaxDocument(_testPdf);
            Assert.IsNotNull(faxDocument);
            var fileDocument = faxDocument as FileDocument;
            Assert.IsNotNull(fileDocument);
            Assert.AreEqual(_testPdf, fileDocument.FilePath);
            Assert.AreEqual("application/pdf", fileDocument.MediaType);
        }

        [TestMethod]
        public void can_unpack_and_load_supported_media_types()
        {
            _interfax = new FaxClient("unit-test-user", "unit-test-pass");
            var types = _interfax.Documents.SupportedMediaTypes;
            Assert.IsNotNull(types);
            Assert.IsTrue(types.ContainsKey("pdf"));
        }
    }
}