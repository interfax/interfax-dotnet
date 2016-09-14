using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class OutboundTests
    {
        private InterFAX _interfax;

        [SetUp]
        public void Setup()
        {
            _interfax = new InterFAX();
        }


        [Test]
        public void can_get_outbound_fax_list()
        {
            var list = _interfax.Outbound.GetList().Result;
            Assert.IsTrue(list.Any());
        }


        [Test]
        public void can_get_outbound_fax_list_with_listoptions()
        {
            // not testing the results, except that they should be a list of 
            // whether the REST api is working correctly or not isn't part of these tests.

            var list = _interfax.Outbound.GetList(new Outbound.ListOptions
            {
                LastId = 0,
                Limit = 2,
                SortOrder = ListSortOrder.Ascending
            }).Result;
            Assert.IsTrue(list.Any());
        }

        [Test]
        public void can_stream_fax_image_to_file()
        {
            var filename = $"{Guid.NewGuid().ToString()}.tiff";

            using (var imageStream = _interfax.Outbound.GetFaxImageStream(662208217).Result)
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
        public void cancelling_already_sent_fax_builds_error_response()
        {
            const int messageId = 661900007;

            var exception = Assert.Throws<AggregateException>(() =>
            {
                var response = _interfax.Outbound.CancelFax(messageId).Result;
            });

            var apiException = exception.InnerExceptions[0] as ApiException;
            Assert.NotNull(apiException);

            var error = apiException.Error;
            Assert.AreEqual(HttpStatusCode.Conflict, apiException.StatusCode);
            Assert.AreEqual(-162, error.Code);
            Assert.AreEqual("Transaction is in a wrong status for this operation", error.Message);
            Assert.AreEqual("Transaction ID 661900007 has already completed", error.MoreInfo);
        }

        [Test]
        public void can_resend_fax()
        {
            const int messageId = 661900007;

            var exception = Assert.Throws<AggregateException>(() =>
            {
                var response = _interfax.Outbound.CancelFax(messageId).Result;
            });
        }

        [Ignore("")]
        [Test]
        public void can_hide_fax()
        {
            const int messageId = 661900007;

            Assert.DoesNotThrow(() =>
            {
                var response = _interfax.Outbound.HideFax(messageId).Result;
            });
        }
    }
}
