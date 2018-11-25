using System;
using System.IO;
using System.Net;
using System.Reflection;
using InterFAX.Api.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Integration
{
    [TestClass]
    public class DocumentsTests
    {
        private FaxClient _interfax;
        private readonly string _testPath;

		private String _faxNumber = TestingConfig.faxNumber;

		public DocumentsTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }

        [TestInitialize]
        public void Setup()
        {
			_interfax = new FaxClient(TestingConfig.username, TestingConfig.password);
		}

        [TestMethod]
        public void can_get_outbound_document_list()
        {
                var list = _interfax.Outbound.Documents.GetUploadSessions().Result;
        }

        [TestMethod]
        public void can_get_outbound_document_list_with_listoptions()
        {
                var list = _interfax.Outbound.Documents.GetUploadSessions(new Documents.ListOptions
                {
                    Offset = 10,
                    Limit = 5
                }).Result;
        }

        [TestMethod]
        public void can_create_and_delete_document_upload_session()
        {
            var options = new Documents.UploadSessionOptions
            {
                Name = "document.pdf",
                Size = 102400,
                Disposition = DocumentDisposition.SingleUse,
                Sharing = DocumentSharing.Private
            };

            var sessionId = _interfax.Outbound.Documents.CreateUploadSession(options).Result;
            Assert.IsNotNull(sessionId);

            var sessionStatus = _interfax.Outbound.Documents.GetUploadSession(sessionId).Result;
            Assert.IsNotNull(sessionStatus);
            Assert.AreEqual(options.Name, sessionStatus.FileName);
            Assert.AreEqual(options.Size, sessionStatus.FileSize);
            Assert.AreEqual(options.Disposition, sessionStatus.Disposition);
            Assert.AreEqual(options.Sharing, sessionStatus.Sharing);
            Assert.AreEqual(DocumentStatus.Created, sessionStatus.Status);

            var result = _interfax.Outbound.Documents.CancelUploadSession(sessionId).Result;
            Assert.AreEqual("OK", result);

            var exception = Assert.ThrowsException<AggregateException>(() =>
            {
                var response = _interfax.Outbound.Documents.GetUploadSession(sessionId).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.IsNotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-1062, error.Code);
            Assert.AreEqual("Wrong uploaded document resource", error.Message);
            Assert.IsNull(error.MoreInfo);
        }


        [TestMethod]
		public void can_upload_large_document()
        {
            var fileInfo = new FileInfo(Path.Combine(_testPath, "large.pdf"));

            // Upload the document.
            var session = _interfax.Outbound.Documents.UploadDocument(fileInfo.FullName);

            // Delete the session
            var result = _interfax.Outbound.Documents.CancelUploadSession(session.Id).Result;
            Assert.AreEqual("OK", result);

            // Check that the session no longer exists
            var exception = Assert.ThrowsException<AggregateException>(() =>
            {
                var response = _interfax.Outbound.Documents.GetUploadSession(session.Id).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.IsNotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-1062, error.Code);
            Assert.AreEqual("Wrong uploaded document resource", error.Message);
            Assert.IsNull(error.MoreInfo);
        }
        [TestMethod]
        public void can_fax_small_document_as_binary()
        {
            var path = Path.Combine(_testPath, "test.pdf");
            var filebytes = File.ReadAllBytes(path);
            

            // Fax the document
            var faxDocument = _interfax.Documents.BuildFaxDocument(filebytes, ".pdf");
            var faxId = _interfax.Outbound.SendFax(faxDocument, new SendOptions { FaxNumber = _faxNumber }).Result;
            Assert.IsTrue(faxId > 0);
        }

        [TestMethod]
		public void can_fax_small_document()
        {
            var fileInfo = new FileInfo(Path.Combine(_testPath, "test.pdf"));

            // Upload the document.
            var session = _interfax.Outbound.Documents.UploadDocument(fileInfo.FullName);

            // Fax the document
            var faxDocument = _interfax.Documents.BuildFaxDocument(session.Uri);
            var faxId = _interfax.Outbound.SendFax(faxDocument, new SendOptions { FaxNumber = _faxNumber }).Result;
            Assert.IsTrue(faxId > 0);

            // Check that the session no longer exists
            var exception = Assert.ThrowsException<AggregateException>(() =>
            {
                var response = _interfax.Outbound.Documents.GetUploadSession(session.Id).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.IsNotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-1062, error.Code);
            Assert.AreEqual("Wrong uploaded document resource", error.Message);
            Assert.IsNull(error.MoreInfo);
        }

        [TestMethod]
        public void can_fax_small_document_as_stream()
        {
            Int64 faxId;
            using (var fileStream = File.OpenRead(Path.Combine(_testPath, "test.pdf")))
            {
                // Fax the document
                var faxDocument = _interfax.Documents.BuildFaxDocument("test.pdf", fileStream);
                faxId = _interfax.Outbound.SendFax(faxDocument, new SendOptions { FaxNumber = _faxNumber }).Result;
                Assert.IsTrue(faxId > 0);
            }
        }
    }
}
