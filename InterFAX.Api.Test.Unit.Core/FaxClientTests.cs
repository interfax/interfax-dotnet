using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Unit
{
    [TestClass]
    public class FaxClientTests
    {
        private const string Username = "fakeusername";
        private const string Password = "fakepassword";

        [TestMethod]
        public void can_instantiate_with_credentials()
        {
            var interfax = new FaxClient(Username, Password);
            Assert.IsNotNull(interfax.Account);
            Assert.IsNotNull(interfax.Documents);
            Assert.IsNotNull(interfax.Inbound);
            Assert.IsNotNull(interfax.Outbound);
        }

        private void CreateClient() 
        {
            var interfax = new FaxClient();
            Assert.IsNotNull(interfax.Account);
            Assert.IsNotNull(interfax.Documents);
            Assert.IsNotNull(interfax.Inbound);
            Assert.IsNotNull(interfax.Outbound);  
        }

        [TestMethod]
        public void can_instantiate_from_config()
        {
            ConfigurationManager.AppSettings[FaxClient.UsernameConfigKey] = Username;
            ConfigurationManager.AppSettings[FaxClient.PasswordConfigKey] = Password;

            CreateClient();

            ConfigurationManager.AppSettings[FaxClient.UsernameConfigKey] = null;
            ConfigurationManager.AppSettings[FaxClient.PasswordConfigKey] = null;
        }

        [TestMethod]
        public void can_instantiate_from_environment()
        {
            var existingUsername = Environment.GetEnvironmentVariable(FaxClient.UsernameConfigKey);
            var existingPassword = Environment.GetEnvironmentVariable(FaxClient.PasswordConfigKey);

            Environment.SetEnvironmentVariable(FaxClient.UsernameConfigKey, Username);
            Environment.SetEnvironmentVariable(FaxClient.PasswordConfigKey, Password);

            CreateClient();

            Environment.SetEnvironmentVariable(FaxClient.UsernameConfigKey, existingUsername);
            Environment.SetEnvironmentVariable(FaxClient.PasswordConfigKey, existingPassword);
        }

        [TestMethod]
        public void should_throw_if_no_config()
        {
            Assert.ThrowsException<ArgumentException>(() => CreateClient());
        }
    }
}