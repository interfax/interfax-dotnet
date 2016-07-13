using System;

namespace Interfax.ClientLib.Entities
{
    /// <summary>
    /// Represents an outbound fax
    /// </summary>
    public class OutboundFaxFull : OutboundFaxSummary
    {
	    /// <summary>
        /// Page size: a4, letter, legal, or b4	
	    /// </summary>
        public Enums.PageSize PageSize;
		
        /// <summary>
        /// portrait or landscape
        /// </summary>
        public Enums.PageOrientation PageOrientation;
	 	
	    /// <summary>
        /// standard or fine
	    /// </summary>
        public Enums.PageResolution Resolution;
		
 		/// <summary>
        /// greyscale or bw
 		/// </summary>
        public Enums.PageRendering Rendering;
	 	
	    /// <summary>
        /// The fax header text inserted at the top of the page.
	    /// </summary>
        public string PageHeader;
		
 		/// <summary>
        /// Time when the transaction was originally submitted. Always returned in GMT.
 		/// </summary>
        public DateTime SubmitTime;
		
 	 	/// <summary>
        /// A name or other optional identifier.
 	 	/// </summary>
        public string Subject;
	 	
        /// <summary>
        /// The resolved fax number to which the fax was sent.
        /// </summary>
        public string DestinationFax;

        /// <summary>
        /// Contact name provided during submission
        /// </summary>
        public string Contact;

 	 	/// <summary>
        /// E-mail address(es) for confirmation message.
 	 	/// </summary>
        public string ReplyEmail;

        /// <summary>
        /// Total number of pages submitted.
        /// </summary>
        public int PagesSubmitted;
		
 	 	/// <summary>
        /// Sender's fax ID.
 	 	/// </summary>
        public string SenderCSID;

        /// <summary>
        /// Maximum number of transmission attempts requested in case of fax transmission failure.
        /// </summary>
        public int AttemptsToPerform;	 	 	
    }

    /// <summary>
    /// Represents an outbound fax summary information
    /// </summary>
    public class OutboundFaxSummary
    {
        /// <summary>
        /// A unique identifier for the fax.
        /// </summary>
        public string Id;

        /// <summary>
        /// A unique resource locator for the fax.
        /// </summary>
        public Uri Uri;

        /// <summary>
        /// Fax status. Generally, 0= OK; less than 0 = in process; greater than 0 = Error (See Interfax Status Codes)
        /// </summary>		
        public int Status;

        /// <summary>
        /// The submitting user.
        /// </summary>
        public string UserID;

        /// <summary>
        /// Number of successfully sent pages.
        /// </summary>
        public int PagesSent;

        /// <summary>
        /// End time of last of all transmission attempts. Always returned in GMT.
        /// </summary>
        public DateTime CompletionTime;

        /// <summary>
        /// Receiving party fax ID (up to 20 characters).
        /// </summary>
        public string RemoteCSID;

        /// <summary>
        /// Transmission time in seconds.
        /// </summary>
        public int Duration;

        /// <summary>
        /// For internal use.
        /// </summary>
        public int Priority;

        /// <summary>
        /// Decimal number of units to be billed (pages or tenths of minutes)
        /// </summary>
        public decimal Units;

        /// <summary>
        /// Monetary units, in account currency. Multiply this by 'Units' to get tde actual cost of the fax.
        /// </summary>
        public decimal CostPerUnit;
    }

}
