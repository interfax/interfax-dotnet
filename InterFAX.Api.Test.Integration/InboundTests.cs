using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using Scotch;
using InterFAX.Api.Test.Integration.extensions;

namespace InterFAX.Api.Test.Integration
{
    public class InboundTests
    {
        private FaxClient _interfax;
        private readonly string _testPath;
		private int _inboundFaxId = TestingConfig.inboundFaxID;

        public InboundTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }

        [SetUp]
        public void Setup()
        {
            var httpClient = HttpClients.NewHttpClient(_testPath + TestingConfig.scotchCassettePath, TestingConfig.scotchMode);
			_interfax = new FaxClient(TestingConfig.username, TestingConfig.password, httpClient);
		}

        [Test]
		[IgnoreMocked]
        public void can_get_inbound_fax_list()
        {
            var list = _interfax.Inbound.GetList().Result;
            Assert.IsTrue(list.Any());
        }

        [Test]
        public void can_get_forwarding_emails()
        {
            var emails = _interfax.Inbound.GetForwardingEmails(_inboundFaxId).Result;
			if (TestingConfig.scotchMode != ScotchMode.Replaying) Assert.IsTrue(emails.Any());
        }

        [Test]
        public void can_get_single_fax()
        {
            var item = _interfax.Inbound.GetFaxRecord(_inboundFaxId).Result;
			if (TestingConfig.scotchMode != ScotchMode.Replaying) Assert.NotNull(item);
        }

        [Test]
		[IgnoreMocked]
        public void can_get_inbound_fax_list_with_listoptions()
        {
            var list = _interfax.Inbound.GetList(new Inbound.ListOptions
            {
                UnreadOnly = true,
                Limit = 10,
                AllUsers = true
            }).Result;
            Assert.IsTrue(list.Any());
        }

        [Ignore("Need to add an existing inbound fax Id to make this pass. Can't send inbound fax to myself...")]
        [Test]
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

        [Test]
        public void can_mark_fax_as_read()
        {
            int messageId = _inboundFaxId;

            var response = _interfax.Inbound.MarkRead(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
			if (TestingConfig.scotchMode != ScotchMode.Replaying) Assert.AreEqual(ImageStatus.READ, fax.ImageStatus);
        }

        [Test]
        public void can_mark_fax_as_unread()
        {
            int messageId = _inboundFaxId;

            var response = _interfax.Inbound.MarkUnread(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
			if (TestingConfig.scotchMode != ScotchMode.Replaying) Assert.AreEqual(ImageStatus.UNREAD, fax.ImageStatus);
		}

        [Test]
        public void can_resend_fax()
        {
            int messageId = _inboundFaxId;

            Assert.DoesNotThrow(() =>
            {
                var response = _interfax.Inbound.Resend(messageId).Result;
            });
        }


        [Test]
		[IgnoreMocked]
		public void resending_non_existing_fax_builds_error_response()
        {
            const int messageId = 1;

            var exception = Assert.Throws<AggregateException>(() =>
            {
                var response = _interfax.Inbound.Resend(messageId).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.NotFound, apiException.StatusCode);
            Assert.AreEqual(-3001, error.Code);
            Assert.AreEqual("Invalid MessageID or User is not authorized to access message", error.Message);
            Assert.AreEqual("Invalid ID [1]", error.MoreInfo);
        }

        [Test]
        public void can_resend_fax_with_email_address()
        {
			int messageId = _inboundFaxId;

            Assert.DoesNotThrow(() =>
            {
                var response = _interfax.Inbound.Resend(messageId, "InterFAXDevelopersDotNet@interfax.net").Result;
            });
        }
    }
}