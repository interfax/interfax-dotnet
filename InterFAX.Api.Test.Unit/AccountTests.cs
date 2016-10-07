using System;
using System.Globalization;
using NUnit.Framework;

namespace InterFAX.Api.Test.Unit
{
    [TestFixture]
    public class AccountTests
    {
        private FaxClient _interfax;
        private MockHttpMessageHandler _handler;


        [Test]
        public void should_call_correct_balance_uri()
        {
            const decimal expected = 1.23M;

            _handler = new MockHttpMessageHandler
            {
                ExpectedContent = expected.ToString(CultureInfo.InvariantCulture),
                ExpectedUri = new Uri("https://rest.interfax.net/accounts/self/ppcards/balance")
            };

            _interfax = new FaxClient("unit-test-user", "unit-test-pass", _handler);

            var actual = _interfax.Account.GetBalance().Result;
            Assert.AreEqual(_handler.ExpectedUri, _handler.ActualUri);
            Assert.AreEqual(expected, actual);
        }
    }
}