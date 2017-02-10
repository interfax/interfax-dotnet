using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    public class InboundTests
    {
        private FaxClient _interfax;
        private readonly string _testPath;

        public InboundTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }

        [SetUp]
        public void Setup()
        {
            _interfax = new FaxClient();
        }

        [Test]
        public void can_get_inbound_fax_list()
        {
            var list = _interfax.Inbound.GetList().Result;
            Assert.IsTrue(list.Any());
        }

        [Test]
        public void can_get_forwarding_emails()
        {
            var emails = _interfax.Inbound.GetForwardingEmails(291704306).Result;
            Assert.IsTrue(emails.Any());
        }

        [Test]
        public void can_get_single_fax()
        {
            var item = _interfax.Inbound.GetFaxRecord(291704306).Result;
            Assert.NotNull(item);
        }

        [Test]
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

            using (var imageStream = _interfax.Inbound.GetFaxImageStream(123456789).Result)
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
            const int messageId = 291704306;

            var response = _interfax.Inbound.MarkRead(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
            Assert.AreEqual(ImageStatus.READ, fax.ImageStatus);
        }

        [Test]
        public void can_mark_fax_as_unread()
        {
            const int messageId = 291704306;

            var response = _interfax.Inbound.MarkUnread(messageId).Result;

            var fax = _interfax.Inbound.GetFaxRecord(messageId).Result;
            Assert.AreEqual(ImageStatus.UNREAD, fax.ImageStatus);
        }

        [Test]
        public void can_resend_fax()
        {
            const int messageId = 291704306;

            Assert.DoesNotThrow(() =>
            {
                var response = _interfax.Inbound.Resend(messageId).Result;
            });
        }


        [Test]
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
            const int messageId = 291704306;

            Assert.DoesNotThrow(() =>
            {
                var response = _interfax.Inbound.Resend(messageId, "InterFAXDevelopersDotNet@interfax.net").Result;
            });
        }
    }
}