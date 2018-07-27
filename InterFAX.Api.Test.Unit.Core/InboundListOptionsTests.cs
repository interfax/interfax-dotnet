using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class InboundListOptionsTests
    {
        [TestMethod]
        public void should_return_dictionary_of_options()
        {
            var listOptions = new Inbound.ListOptions
            {
                UnreadOnly = true,
                Limit = 20,
                LastId = 40,
                AllUsers = true
            };

            var actual = listOptions.ToDictionary();
            Assert.AreEqual(4, actual.Keys.Count);

            var key = "unreadOnly";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("true", actual[key]);

            key = "limit";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.Limit.ToString(), actual[key]);

            key = "lastId";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.LastId.ToString(), actual[key]);

            key = "allUsers";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("true", actual[key]);
        }
    }
}