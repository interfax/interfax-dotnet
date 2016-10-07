using System;
using Newtonsoft.Json;

namespace InterFAX.Api.Dtos
{
    public class OutboundFaxSummary : OutboundFaxResult
    {

        /// <summary>
        /// The submitting user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Number of successfully sent pages.
        /// </summary>
        public int PagesSent { get; set; }

        /// <summary>
        /// End time of last of all transmission attempts.
        /// </summary>
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// Receiving party fax ID (up to 20 characters).
        /// </summary>
        [JsonProperty(PropertyName = "remoteCSID")]
        public string RemoteCSID { get; set; }

        /// <summary>
        /// Transmission time in seconds.
        /// </summary>
        public decimal Duration { get; set; }

        /// <summary>
        /// Decimal number of units to be billed (pages or tenths of minutes)
        /// </summary>
        public decimal Units { get; set; }

        /// <summary>
        /// Monetary units, in account currency.
        /// </summary>
        public decimal CostPerUnit { get; set; }

        /// <summary>
        /// Retry attempts actually performed.
        /// </summary>
        public int AttemptsMade { get; set; }

        /// <summary>
        /// Units * CostPerUnit
        /// </summary>
        [JsonIgnore]
        public decimal Cost => Units * CostPerUnit;
    }
}