using InterFAX.Api.Dtos;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class UploadSessionOptionsTests
    {
        [Test]
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
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Size.ToString(), actual[key]);

            key = "name";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual(options.Name, actual[key]);

            key = "disposition";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("permanent", actual[key]);

            key = "sharing";
            Assert.That(actual.ContainsKey(key));
            Assert.AreEqual("private", actual[key]);
        }
    }
}