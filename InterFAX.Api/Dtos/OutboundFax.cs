using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InterFAX.Api.Dtos
{
    public class OutboundFax : OutboundFaxSummary
    {
        /// <summary>
        /// a4, letter, legal, or b4
        /// </summary>
        public string PageSize { get; set; }

        /// <summary>
        /// portrait or landscape
        /// </summary>
        public string PageOrientation { get; set; }

        /// <summary>
        /// standard or fine
        /// </summary>
        public string PageResolution { get; set; }

        /// <summary>
        /// Standard (optimize for black &amp; white) or Fine (optimize for greyscale)
        /// </summary>
        public string Rendering { get; set; }

        /// <summary>
        /// The fax header text inserted at the top of the page.
        /// </summary>
        public string PageHeader { get; set; }

        /// <summary>
        // Time when the transaction was originally submitted. Always returned in GMT.
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// A name or other optional identifier.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The resolved fax number to which the fax was sent.
        /// </summary>
        public string DestinationFax { get; set; }

        /// <summary>
        /// E-mail address for confirmation message.
        /// </summary>
        public string ReplyEmail { get; set; }

        /// <summary>
        /// Total number of pages submitted.
        /// </summary>
        public int PagesSubmitted { get; set; }

        /// <summary>
        /// Sender's fax ID.
        /// </summary>
        [JsonProperty(PropertyName = "senderCSID")]
        public string SenderCSID { get; set; }

        /// <summary>
        /// Maximum number of transmission attempts requested in case of fax transmission failure.
        /// </summary>
        public int AttemptsToPerform { get; set; }

        /// <summary>
        /// The text which was inserted into the Contacts property upon submission of the fax (available in select submission methods only).
        /// </summary>
        public string Contact { get; set; }
    }
}
