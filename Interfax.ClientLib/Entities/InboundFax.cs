using System;

namespace Interfax.ClientLib.Entities
{
    /// <summary>
    /// Meta-data of an inbound fax
    /// </summary>
    public class InboundFax
    {
        /// <summary>
        /// The ID of this transaction
        /// </summary>
        public int MessageID;
        /// <summary>
        /// The time and date that the fax was received (in GMT)
        /// </summary>		
        public DateTime ReceiveTime;
        /// <summary>
	    /// The Phone number at which this fax was received
	    /// </summary>
        public String PhoneNumber;
        /// <summary>
        /// The caller ID of the sender
        /// </summary>
        public String CallerID;
        /// <summary>
        /// The CSID of the sender
        /// </summary>
        public String RemoteCSID;
        /// <summary>
        /// The number of pages received in this fax.
        /// </summary>
        public int Pages;
	    /// <summary>
	    /// Status of the fax. See the list of InterFAX Error Codes
	    /// </summary>
        public int Status;
        /// <summary>
        /// The time (in seconds) that it took to receive the fax
        /// </summary>
        public int RecordingDuration;
    }
}
