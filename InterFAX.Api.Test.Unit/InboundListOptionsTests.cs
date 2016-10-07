using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class InboundListOptionsTests
    {
        [Test]
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
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("true", actual[key]);

            key = "limit";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.Limit.ToString(), actual[key]);

            key = "lastId";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.LastId.ToString(), actual[key]);

            key = "allUsers";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("true", actual[key]);
        }
    }
}