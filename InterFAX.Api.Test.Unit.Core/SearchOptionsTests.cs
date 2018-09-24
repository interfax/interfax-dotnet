using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class SearchOptionsTests
    {
        [TestMethod]
        public void should_return_dictionary_of_options()
        {
            var searchOptions = new SearchOptions
            {
                Ids = new Int64[] { 1, 2, 3, 4, 5 },
                Reference = "unit-test-reference",
                DateFrom = new DateTime(2016, 6, 1, 14, 30, 0),
                DateTo = new DateTime(2016, 7, 5, 16, 45, 0),
                StatusFamily = StatusFamily.Completed,
                UserId = "unit-test-userid",
                FaxNumber = "+1234567890",
                SortOrder = ListSortOrder.Ascending,
                Offset = 10,
                Limit = 20,
            };

            var actual = searchOptions.ToDictionary();
            Assert.AreEqual(10, actual.Keys.Count);

            var key = "ids";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("1,2,3,4,5", actual[key]);

            key = "reference";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.Reference, actual[key]);

            key = "dateFrom";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("2016-06-01T14:30:00Z", actual[key]);

            key = "dateTo";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("2016-07-05T16:45:00Z", actual[key]);

            key = "status";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("Completed", actual[key]);

            key = "userId";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.UserId, actual[key]);

            key = "faxNumber";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.FaxNumber, actual[key]);

            key = "sortOrder";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("asc", actual[key]);

            key = "offset";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.Offset.ToString(), actual[key]);

            key = "limit";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.Limit.ToString(), actual[key]);
        }

        [TestMethod]
        public void should_switch_on_status()
        {
            var searchOptions = new SearchOptions
            {
                Status = 2
            };

            var actual = searchOptions.ToDictionary();
            Assert.AreEqual(1, actual.Keys.Count);

            const string key = "status";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(searchOptions.Status.ToString(), actual[key]);
        }
    }
}