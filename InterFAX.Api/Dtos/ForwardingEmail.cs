using System;

namespace InterFAX.Api.Dtos
{
    public class ForwardingEmail
    {
        /// <summary>
        /// An email address to which forwarding of the fax was attempted.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 0 = OK; number smaller than zero = in progress; number greater than zero = error.
        /// </summary>
        public int MessageStatus { get; set; }

        /// <summary>
        /// Completion timestamp. May be null if incomplete.
        /// </summary>
        public DateTime? CompletionTime { get; set; }
    }
}