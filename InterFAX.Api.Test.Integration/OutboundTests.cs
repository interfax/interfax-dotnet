using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
