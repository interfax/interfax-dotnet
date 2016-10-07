using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterFAX.Api
{
    public class Account
    {
        private readonly FaxClient _interfax;

        internal Account(FaxClient interfax)
        {
            _interfax = interfax;
        }

        /// <summary>
        /// Get the remaining faxing credits in your account (in the account's currency).
        /// </summary>
        public async Task<decimal> GetBalance()
        {
            return await _interfax.HttpClient.GetResourceAsync<decimal>("/accounts/self/ppcards/balance");
        }
    }
}