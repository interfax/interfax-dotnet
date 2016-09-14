using System.Dynamic;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void can_base64_encode()
        {
            const string expected = "";
            var actual = Utils.Base64Encode("astring:toencode");
            Assert.AreEqual(expected, actual);
        }   
    }
}