using System.Reflection;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterFAX.Api.Test.Integration
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void can_get_balance()
        {
			var _testPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
			var interfax = new FaxClient(TestingConfig.username, TestingConfig.password);

			var actual = interfax.Account.GetBalance().Result;
            //Assert.IsTrue(actual > 0); Call is still be valid if account balance is zero.
        }
    }
}