using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void can_base64_encode()
        {
            const string expected = "YXN0cmluZzp0b2VuY29kZQ==";
            var actual = Utils.Base64Encode("astring:toencode");
            Assert.AreEqual(expected, actual);
        }   
    }
}