using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using InterFAX.Api.Test.Unit;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class InterFAXTests
    {
        private const string Username = "fakeusername";
        private const string Password = "fakepassword";
        private string _testPath;

        public InterFAXTests()
        {
            _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
        }


        [Test]
        public void can_instantiate_with_credentials()
        {
            var interfax = new InterFAX(Username, Password);
        }

        [Test]
        public void can_instantiate_from_config()
        {
            ConfigurationManager.AppSettings[InterFAX.UsernameConfigKey] = Username;
            ConfigurationManager.AppSettings[InterFAX.PasswordConfigKey] = Password;

            Assert.DoesNotThrow(() =>
            {
                var interfax = new InterFAX();
            });

            ConfigurationManager.AppSettings[InterFAX.UsernameConfigKey] = null;
            ConfigurationManager.AppSettings[InterFAX.PasswordConfigKey] = null;
        }

        [Test]
        public void can_instantiate_from_environment()
        {
            var existingUsername = Environment.GetEnvironmentVariable(InterFAX.UsernameConfigKey);
            var existingPassword = Environment.GetEnvironmentVariable(InterFAX.PasswordConfigKey);

            Environment.SetEnvironmentVariable(InterFAX.UsernameConfigKey, Username);
            Environment.SetEnvironmentVariable(InterFAX.PasswordConfigKey, Password);

            Assert.DoesNotThrow(() =>
            {
                var interfax = new InterFAX();
            });

            Environment.SetEnvironmentVariable(InterFAX.UsernameConfigKey, existingUsername);
            Environment.SetEnvironmentVariable(InterFAX.PasswordConfigKey, existingPassword);
        }

        [Test]
        public void should_throw_if_no_config()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var interfax = new InterFAX();
            });
        }

        [Test]
        public void can_create_FileDocument()
        {
            var handler = new MockHttpMessageHandler
            {
                ExpectedContent = "[{'MediaType': 'application/pdf','FileType': 'pdf'}]",
                ExpectedUri = new Uri("https://rest.interfax.net/outbound/help/mediatype")
            };
            var interfax = new InterFAX(Username, Password, handler);
            var filePath = Path.Combine(_testPath, "test.pdf");
            var faxDocument = interfax.CreateFaxDocument(Path.Combine(_testPath, "test.pdf"));
            Assert.NotNull(faxDocument);
            var fileDocument = faxDocument as FileDocument;
            Assert.NotNull(fileDocument);
            Assert.AreEqual(filePath, fileDocument.FilePath);
            Assert.AreEqual("application/pdf", fileDocument.MediaType);
        }
    }
}