using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void can_base64_encode()
        {
            const string expected = "YXN0cmluZzp0b2VuY29kZQ==";
            var actual = Utils.Base64Encode("astring:toencode");
            Assert.AreEqual(expected, actual);
        }   
    }
}