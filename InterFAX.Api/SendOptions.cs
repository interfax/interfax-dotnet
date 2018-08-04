using System;
using System.Collections.Generic;
using System.Globalization;
using InterFAX.Api.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api
{
    /// <summary>
    /// Options which can be provided when submitting a fax.
    /// </summary>
    public class SendOptions : IOptions
    {
        /// <summary>
        /// A single fax number, e.g: +1-212-3456789.
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// A name or other reference. The entered string will appear: 
        /// (1) for reference in the outbound queue; 
        /// (2) in the outbound fax header, if headers are configured; and 
        /// (3) in subsequent queries of the fax object.
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Time to schedule the transmission.
        /// </summary>
        public DateTime? PostponeTime { get; set; }

        /// <summary>
        /// Number of transmission attempts to perform, in case of fax transmission failure.
        /// </summary>
        public int? RetriesToPerform { get; set; }

        /// <summary>
        /// Sender CSID. (defaults is taken from control panel settings)
        /// </summary>
        public string Csid { get; set; }

        /// <summary>
        /// The fax header text to insert at the top of the page. 
        /// Enter a string template to send a fax with a dynamically-populated header. 
        /// e.g. "To: {To} From: {From} Pages: {TotalPages}"
        /// </summary>
        public string PageHeader { get; set; }

        /// <summary>
        /// Provide your internal ID to a document. 
        /// This parameter can be obtained by status query, but is not included in the transmitted fax message.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// E-mail address to which feedback messages will be sent.
        /// </summary>
        public string ReplyAddress { get; set; }

        /// <summary>
        /// a4, letter, legal, or b4.
        /// </summary>
        public PageSize? PageSize { get; set; }

        /// <summary>
        /// Scaling enlarges or reduces an image file to the given page size.
        /// </summary>
        public bool? ShouldScale { get; set; }

        /// <summary>
        /// portrait or landscape.
        /// </summary>
        public PageOrientation? PageOrientation { get; set; }

        /// <summary>
        /// standard or fine. Documents rendered as fine may be more readable but take longer to transmit (and may therefore be more costly).
        /// </summary>
        public PageResolution? PageResolution { get; set; }

        /// <summary>
        /// greyscale or bw. 
        /// Determines the rendering mode. 
        /// bw is recommended for textual, black &amp; white documents, while greyscale is better for greyscale text and for images.
        /// </summary>
        public PageRendering? PageRendering { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var options = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(FaxNumber)) options.Add("faxNumber", FaxNumber);
            if (!string.IsNullOrEmpty(Contact)) options.Add("contact", Contact);
            if (PostponeTime.HasValue) options.Add("postponeTime", PostponeTime.Value.ToString("s") + "Z");
            if (RetriesToPerform != null) options.Add("retriesToPerform", RetriesToPerform.ToString());
            if (!string.IsNullOrEmpty(Csid)) options.Add("csid", Csid);
            if (!string.IsNullOrEmpty(PageHeader)) options.Add("pageHeader", PageHeader);
            if (!string.IsNullOrEmpty(Reference)) options.Add("reference", Reference);
            if (!string.IsNullOrEmpty(ReplyAddress)) options.Add("replyAddress", ReplyAddress);
            if (PageSize.HasValue) options.Add("pageSize", PageSize.ToCamelCase());
            if (ShouldScale.HasValue) options.Add("fitToPage", ShouldScale.Value? "scale" : "noscale");
            if (PageOrientation.HasValue) options.Add("pageOrientation", PageOrientation.ToCamelCase());
            if (PageResolution.HasValue) options.Add("resolution", PageResolution.ToCamelCase());
            if (PageRendering.HasValue) options.Add("rendering", PageRendering.ToCamelCase());
            return options;
        }
    }
}