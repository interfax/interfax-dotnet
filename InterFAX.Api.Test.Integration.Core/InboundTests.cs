using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Integration
{
    [TestClass]
    public class InboundTests
    {
        private FaxClient _interfax;
        private readonly string _testPath;
		private Int64 _inboundFaxId = TestingConfig.inboundFaxID;

        public InboundTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }

        [TestInitialize]
        public void Setup()
        {
			_interfax = new FaxClient(TestingConfig.username, TestingConfig.password);
		}

        [TestMethod]
        public void can_get_inbound_fax_list()
        {
            var list = _interfax.Inbound.GetList().Result;
			//Assert.IsTrue(list.Any()); Call can still be valid if no inbound faxes
        }

        [TestMethod]
        public void can_get_forwarding_emails()
        {
            var emails = _interfax.Inbound.GetForwardingEmails(_inboundFaxId).Result;
			 Assert.IsTrue(emails.Any());
        }

        [TestMethod]
        public void can_get_single_fax()
        {
            var item = _interfax.Inbound.GetFaxRecord(_inboundFaxId).Result;
			 Assert.IsNotNull(item);
        }

        [TestMethod]
        public void can_get_inbound_fax_list_with_listoptions()
        {
            var list = _interfax.Inbound.GetList(new Inbound.ListOptions
            {
                UnreadOnly = true,
                Limit = 10,
                AllUsers = true
            }).Result;
			//Assert.IsTrue(list.Any()); Call can still be valid if no inbound faxes
		}

		[Ignore("Need to add an existing inbound fax Id to make this pass. Can't send inbound fax to myself...")]
        [TestMethod]
        public void can_stream_fax_image_to_file()
        {
            var filename = $"{Guid.NewGuid().ToString()}.tiff";

            using (var imageStream = _interfax.Inbound.GetFaxImageStream(_inboundFaxId).Result)
            {
                using (var fileStream = File.Create(filename))
                {
                    Utils.CopyStream(imageStream, fileStream);
                }
            }
            Assert.IsTrue(File.Exists(filename));
            Assert.IsTrue(new FileInfo(filename).Length > 0);
            File.Delete(filename);
        }

        [TestMethod]
        public void can_mark_fax_as_read()
        {
            var messageId = _inboundFaxId;

            var response = _interfax.Inbound.MarkRead(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
			 Assert.AreEqual(ImageStatus.READ, fax.ImageStatus);
        }

        [TestMethod]
        public void can_mark_fax_as_unread()
        {
            var messageId = _inboundFaxId;

            var response = _interfax.Inbound.MarkUnread(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
			 Assert.AreEqual(ImageStatus.UNREAD, fax.ImageStatus);
		}

        [TestMethod]
        public void can_resend_fax()
        {
            var messageId = _inboundFaxId;
            
                var response = _interfax.Inbound.Resend(messageId).Result;
        
        }


        [TestMethod]
		public void resending_non_existing_fax_builds_error_response()
        {
            const Int64 messageId = 1;

            var exception = Assert.ThrowsException<AggregateException>(() =>
            {
                var response = _interfax.Inbound.Resend(messageId).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.IsNotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-3001, error.Code);
            Assert.AreEqual("Invalid MessageID or User is not authorized to access message", error.Message);
            Assert.AreEqual("Invalid ID [1]", error.MoreInfo);
        }

        [TestMethod]
        public void can_resend_fax_with_email_address()
        {
			var messageId = _inboundFaxId;
            
            var response = _interfax.Inbound.Resend(messageId, "InterFAXDevelopersDotNet@interfax.net").Result;
        }
    }
}