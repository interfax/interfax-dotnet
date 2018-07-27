using InterFAX.Api.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class UploadSessionOptionsTests
    {
        [TestMethod]
        public void should_return_dictionary_of_options()
        {
            var options = new Documents.UploadSessionOptions
            {
                Size = 102400,
                Name = "document.pdf",
                Disposition = DocumentDisposition.Permanent,
                Sharing = DocumentSharing.Private
            };

            var actual = options.ToDictionary();
            Assert.AreEqual(4, actual.Keys.Count);

            var key = "size";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(options.Size.ToString(), actual[key]);

            key = "name";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual(options.Name, actual[key]);

            key = "disposition";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("permanent", actual[key]);

            key = "sharing";
            Assert.IsTrue(actual.ContainsKey(key));
            Assert.AreEqual("private", actual[key]);
        }
    }
}