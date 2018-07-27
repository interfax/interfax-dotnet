using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class OutboundListOptionsTests
    {
        [TestMethod]
        public void should_return_dictionary_of_options()
        {
            var listOptions = new Outbound.ListOptions
            {
                LastId = 10,
                Limit = 20,
                UserId = "unit-test-userid",
                SortOrder = ListSortOrder.Ascending
            };

            var actual = listOptions.ToDictionary();
            Assert.AreEqual(4, actual.Keys.Count);

            var key = "lastId";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.LastId.ToString(), actual[key]);

            key = "limit";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.Limit.ToString(), actual[key]);

            key = "userId";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(listOptions.UserId, actual[key]);

            key = "sortOrder";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("asc", actual[key]);
        }
    }
}