using System;
using Newtonsoft.Json;

namespace InterFAX.Api.Dtos
{
    public class InboundFax
    {
        /// <summary>
        /// The UserId who sent the fax.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The Fax Id.
        /// </summary>
        public Int64 MessageId { get; set; }

        /// <summary>
        /// The Phone number at which this fax was received.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The CSID of the sender.
        /// </summary>
        public string RemoteCSID { get; set; }

        /// <summary>
        /// Status of the fax. See the list of InterFAX Error Codes - https://www.interfax.net/en/help/error_codes
        /// </summary>
        public int MessageStatus { get; set; }

        /// <summary>
        /// The number of pages received in this fax.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// The size of the inbound fax image (in bytes).
        /// </summary>
        public int MessageSize { get; set; }

        /// <summary>
        /// The type of message. Currently sends out static '1'.
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// The time and date that the fax was received
        /// (formatted as dd/MM/yyyy hh:mm:ss). Times listed are GMT.
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// The caller ID of the sender.
        /// </summary>
        public string CallerId { get; set; }

        /// <summary>
        /// The time (in seconds) that it took to receive the fax.
        /// </summary>
        public int RecordingDuration { get; set; }

        /// <summary>
        /// Has the image been READ or is it UNREAD.
        /// </summary>
        public ImageStatus ImageStatus { get; set; }

        /// <summary>
        /// Number of emails sent.
        /// </summary>
        [JsonProperty(PropertyName = "numOfEmails")]
        public int NumberOfEmails { get; set; }

        /// <summary>
        /// Number of times the emails have failed to send.
        /// </summary>
        [JsonProperty(PropertyName = "numOfFailedEmails")]
        public int NumberOfFailedEmails { get; set; }
    }
}