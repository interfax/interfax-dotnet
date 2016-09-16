using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class DocumentsTests
    {
        private InterFAX _interfax;

        [SetUp]
        public void Setup()
        {
            _interfax = new InterFAX();
        }

        [Test]
        public void can_get_outbound_document_list()
        {
            Assert.DoesNotThrow(() =>
            {
                var list = _interfax.Outbound.Documents.GetList().Result;
            });
        }

        [Test]
        public void can_get_outbound_document_list_with_listoptions()
        {
            Assert.DoesNotThrow(() =>
            {
                var list = _interfax.Outbound.Documents.GetList(new Documents.ListOptions
                {
                    Offset = 10,
                    Limit = 5
                }).Result;
            });
        }

        [Test]
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
            Assert.NotNull(sessionId);

            var sessionStatus = _interfax.Outbound.Documents.GetUploadSessionStatus(sessionId).Result;
            Assert.NotNull(sessionStatus);
            Assert.AreEqual(options.Name, sessionStatus.FileName);
            Assert.AreEqual(options.Size, sessionStatus.FileSize);
            Assert.AreEqual(options.Disposition, sessionStatus.Disposition);
            Assert.AreEqual(options.Sharing, sessionStatus.Sharing);
            Assert.AreEqual(DocumentStatus.Created, sessionStatus.Status);

            var result = _interfax.Outbound.Documents.CancelUploadSession(sessionId).Result;
            Assert.AreEqual("OK", result);

            var exception = Assert.Throws<AggregateException>(() =>
            {
                var response = _interfax.Outbound.Documents.GetUploadSessionStatus(sessionId).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-1062, error.Code);
            Assert.AreEqual("Wrong uploaded document resource", error.Message);
            Assert.IsNull(error.MoreInfo);
        }
    }
}
