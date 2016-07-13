using System;

namespace Interfax.ClientLib
{
    /// <summary>
    /// Represents Accounts service
    /// </summary>
    public class Accounts : Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Interfax User Id</param>
        /// <param name="Password">Interfax password</param>
        /// <param name="timeout">Request timeout (optional, default is 30 sec.)</param>
        /// <param name="endPoint">The service end point (optional). Default is live service</param>
        public Accounts(string userId, string Password, TimeSpan? timeout = null, string endPoint = null) : base("accounts", userId, Password, timeout, endPoint) { }

        
        /// <summary>
        /// Determine the remaining faxing credits in your account (in the account's currency).
        /// </summary>
        /// <param name="balance">The found balance</param>
        /// <returns>Request status</returns>
        public Enums.RequestStatus getBalance(out decimal balance)
        {
            return base.GetObject<decimal>("self/ppcards/balance", out balance);
        }
    }
}
