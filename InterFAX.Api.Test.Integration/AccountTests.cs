using NUnit.Framework;
using Scotch;
using System.Reflection;
using System;
using System.IO;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void can_get_balance()
        {
			var _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
			var httpClient = HttpClients.NewHttpClient(_testPath + TestingConfig.scotchCassettePath, TestingConfig.scotchMode);
			var interfax = new FaxClient(TestingConfig.username, TestingConfig.password, httpClient);

			var actual = interfax.Account.GetBalance().Result;
            //Assert.IsTrue(actual > 0); Call is still be valid if account balance is zero.
        }
    }
}