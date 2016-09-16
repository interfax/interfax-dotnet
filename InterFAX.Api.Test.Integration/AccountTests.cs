using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void can_get_balance()
        {
            var interfax = new InterFAX();
            var actual = interfax.Account.GetBalance().Result;
            Assert.IsTrue(actual > 0);
        }
    }
}