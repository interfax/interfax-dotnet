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
            var list = _interfax.Outbound.Documents.GetList().Result;
            Assert.IsTrue(list.Any());
        }


        [Test]
        public void can_get_outbound_document_list_with_listoptions()
        {
            var list = _interfax.Outbound.Documents.GetList(new Documents.ListOptions
            {
                Offset = 10,
                Limit = 5
            }).Result;
            Assert.IsTrue(list.Any());
        }
    }
}
