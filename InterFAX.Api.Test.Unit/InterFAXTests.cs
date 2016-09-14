using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class InterFAXTests
    {
        private const string Username = "fakeusername";
        private const string Password = "fakepassword";


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
    }
}