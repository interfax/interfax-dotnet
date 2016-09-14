using NUnit.Framework;

namespace InterFAX.Api.Test.Integration
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void can_get_balance()
        {
            const decimal expected = 7.9M;

            var interfax = new InterFAX();
            var actual = interfax.Account.GetBalance().Result;
            Assert.AreEqual(expected, actual);
        }
    }
}