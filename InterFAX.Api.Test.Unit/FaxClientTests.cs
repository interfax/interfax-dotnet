using System;
using System.Configuration;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class FaxClientTests
    {
        private const string Username = "fakeusername";
        private const string Password = "fakepassword";

        [Test]
        public void can_instantiate_with_credentials()
        {
            var interfax = new FaxClient(Username, Password);
            Assert.NotNull(interfax.Account);
            Assert.NotNull(interfax.Documents);
            Assert.NotNull(interfax.Inbound);
            Assert.NotNull(interfax.Outbound);
        }

        private void CreateClient() 
        {
            var interfax = new FaxClient();
            Assert.NotNull(interfax.Account);
            Assert.NotNull(interfax.Documents);
            Assert.NotNull(interfax.Inbound);
            Assert.NotNull(interfax.Outbound);  
        }

        [Test]
        public void can_instantiate_from_config()
        {
            ConfigurationManager.AppSettings[FaxClient.UsernameConfigKey] = Username;
            ConfigurationManager.AppSettings[FaxClient.PasswordConfigKey] = Password;

            Assert.DoesNotThrow(CreateClient);

            ConfigurationManager.AppSettings[FaxClient.UsernameConfigKey] = null;
            ConfigurationManager.AppSettings[FaxClient.PasswordConfigKey] = null;
        }

        [Test]
        public void can_instantiate_from_environment()
        {
            var existingUsername = Environment.GetEnvironmentVariable(FaxClient.UsernameConfigKey);
            var existingPassword = Environment.GetEnvironmentVariable(FaxClient.PasswordConfigKey);

            Environment.SetEnvironmentVariable(FaxClient.UsernameConfigKey, Username);
            Environment.SetEnvironmentVariable(FaxClient.PasswordConfigKey, Password);

            Assert.DoesNotThrow(CreateClient);

            Environment.SetEnvironmentVariable(FaxClient.UsernameConfigKey, existingUsername);
            Environment.SetEnvironmentVariable(FaxClient.PasswordConfigKey, existingPassword);
        }

        [Test]
        public void should_throw_if_no_config()
        {
            Assert.Throws<ArgumentException>(CreateClient);
        }
    }
}