using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Net.Http;

namespace Interfax.ClientLib
{
    /// <summary>
    /// Proxy class for fax outbound service
    /// </summary>
    public class OutboundFax : Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Interfax User Id</param>
        /// <param name="Password">Interfax password</param>
        /// <param name="timeout">Request timeout (optional, default is 30 sec.)</param>
        /// <param name="endPoint">The service end point (optional). Default is live service</param>
        public OutboundFax(string userId, string Password, TimeSpan? timeout = null, string endPoint = null) : base("outbound",userId, Password, timeout, endPoint) { }

        #region Fax submission
        /// <summary>
        /// Submit a fax containing one document from file
        /// </summary>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="path">
        /// The fully qualified path to the file containing the document to be faxed
        /// The filename must end with an extension defining the file type, e.g. dailyrates.docx or newsletter.pdf,
        /// and the file type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types.
        /// </param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        /// <returns>The request status; If successful, a 201 Created status is returned</returns>
        public Enums.RequestStatus Submit(out Uri location, string faxNumber, string path, string charSet = null)
        {
            using (var fs = File.OpenRead(path))
            {
                return Submit(out location, faxNumber, fs, Path.GetExtension(path).TrimStart('.'), true, charSet);
            }
        }

        /// <summary>
        /// Submit a fax containing one document from stream
        /// </summary>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="dataStream">The IO stream containing the document to be faxed</param>
        /// <param name="fileType">The type of the document to be faxed (e.g 'pdf')</param>
        /// <param name="closeStream">Optional (default=true), tells the method whether to close the stream after using its data</param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        /// <returns>The request status; If successful, a 201 Created status is returned</returns>
        public Enums.RequestStatus Submit(out Uri location, string faxNumber, Stream dataStream, string fileType, bool closeStream = false, string charSet = null)
        {
            var data = new byte[dataStream.Length-dataStream.Position];
            dataStream.Read(data, 0, data.Length);
            if (closeStream)
                dataStream.Close();

            return Submit(out location, faxNumber, data, fileType, charSet);
        }

        /// <summary>
        /// List of supported textual file types
        /// </summary>
        private static HashSet<string> textualTypes = new HashSet<string>(new[] { "html","txt","rtf","xml" }, StringComparer.InvariantCultureIgnoreCase);
        /// <summary>
        /// Submit a fax containing one document from byte array
        /// </summary>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <param name="data">The byte-array containing the document to be faxed</param>
        /// <param name="fileType">The type of the textual document to be faxed (e.g. html)</param>
        /// <returns>The request status; If successful, a 201 Created status is returned</returns>
        public Enums.RequestStatus SubmitTextDocument(out Uri location, string faxNumber,string text, string fileType)
        {
            if (!textualTypes.Contains(fileType))
            {
                location = null;
                return Enums.RequestStatus.BadParameters;
            }
            var charSetEncoding = System.Text.Encoding.UTF8;
            var data = charSetEncoding.GetBytes(text);

            return SubmitExtended(out location, faxNumber, new[] { new Entities.InlineOutboundDocument(data, fileType, charSetEncoding.WebName) });
        }

        /// <summary>
        /// Submit a fax containing one document from byte array
        /// </summary>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <param name="data">The byte-array containing the document to be faxed</param>
        /// <param name="fileType">The type of the document to be faxed (e.g 'pdf')</param>
        /// <param name="charSet">In case of a text (e.g Html), this should be specified</param>
        /// <returns>The request status; If successful, a 201 Created status is returned</returns>
        public Enums.RequestStatus Submit(out Uri location, string faxNumber, byte[] data, string fileType, string charSet = null)
        {
            return SubmitExtended(out location,faxNumber,new[] {new Entities.InlineOutboundDocument(data,fileType, charSet)});
        }
     
        /// <summary>
        /// Submit a fax with multiple documents and extended options
        /// </summary>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="documents">list of documents to be faxed, in that order of pages</param>
        /// <param name="contact">
        /// A name or other reference. The entered string will appear: 
        /// (1) for reference in the outbound queue; 
        /// (2) in the outbound fax header, if headers are configured; and 
        /// (3) in subsequent queries of the fax object.</param>
        /// <param name="postponeTime">Time to schedule the transmission.</param>
        /// <param name="retriesToPerform">Number of transmission attempts to perform, in case of fax transmission failure.</param>
        /// <param name="csid">Sender CSID (up to 20 ascii characters)</param>
        /// <param name="pageHeader">The fax header text to insert at the top of the page</param>
        /// <param name="reference">Provide your internal ID to a document. This parameter can be obtained by status query, but is not included in the transmitted fax message.</param>
        /// <param name="replyAddress">E-mail address to which feedback messages will be sent.</param>
        /// <param name="pageSize">page size</param>
        /// <param name="fitToPage">True to fit an image to the designated page size</param>
        /// <param name="pageOrientation">portrait or landscape</param>
        /// <param name="resolution">Documents rendered as fine may be more readable but take longer to transmit (and may therefore be more costly).</param>
        /// <param name="rendering">Determines the rendering mode. bw is recommended for textual, black and white documents, while greyscale is better for greyscale text and for images.</param>
        /// <returns></returns>
        public Enums.RequestStatus SubmitExtended(
            out Uri location, 
            string faxNumber, 
            IEnumerable<Entities.OutboundDocument> documents,
            string contact = null,
            DateTime? postponeTime = null,
            int? retriesToPerform = null,
            string csid = null,
            string pageHeader = null,
            string reference = null,
            string replyAddress = null,
            Enums.PageSize? pageSize = null,
            bool? fitToPage = null,
            Enums.PageOrientation? pageOrientation = null,
            Enums.PageResolution? resolution = null,
            Enums.PageRendering? rendering = null
            )
        {
            location = null;
            if (documents == null || documents.Count() == 0)
                return Enums.RequestStatus.BadParameters;

            HttpContent content;
            if (documents.Count() == 1) 
            {
                // a simple case - just create the content for the single document
                content = MakeContent(documents.First());
            }
            else
            {
                // create a multi-part content and populate it
                content = new MultipartContent();
                foreach (var doc in documents)
                {
                    HttpContent docContent = MakeContent(doc);
                    ((MultipartContent)content).Add(docContent);
                }
            }

            // construct uri from parameters
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["faxNumber"] = faxNumber;

            if (!string.IsNullOrEmpty(contact))
                query["contact"] = contact;

            if (postponeTime.HasValue)
                query["postponeTime"] = postponeTime.Value.ToString("s");

            if (retriesToPerform.HasValue)
                query["retriesToPerform"] = retriesToPerform.Value.ToString();

            if (!string.IsNullOrEmpty(csid))
                query["csid"] = csid;

            if (!string.IsNullOrEmpty(pageHeader))
                query["pageHeader"] = pageHeader;

            if (!string.IsNullOrEmpty(reference))
                query["reference"] = reference;

            if (!string.IsNullOrEmpty(replyAddress))
                query["replyAddress"] = replyAddress;

            if (pageSize.HasValue)
                query["pageSize"] = pageSize.ToString();

            if (fitToPage.HasValue)
                query["fitToPage"] = (fitToPage.Value) ? "scale" : "noscale";

            if (pageOrientation.HasValue)
                query["pageOrientation"] = pageOrientation.ToString();

            if (resolution.HasValue)
                query["resolution"] = resolution.ToString();

            if (rendering.HasValue)
                query["rendering"] = rendering.ToString();

            return base.PostAndGetLocation("faxes?" + query.ToString(), out location, content);
        }

        /// <summary>
        /// Create an Http content out of an abstract OutboundDocument
        /// </summary>
        /// <param name="document">The OutboundDocument to fax</param>
        /// <returns>Http Content</returns>
        private ByteArrayContent MakeContent(Entities.OutboundDocument document)
        {
            ByteArrayContent content = null;
            if (document.GetType() == typeof(Entities.InlineOutboundDocument))
            {
                var inlineDoc = (Entities.InlineOutboundDocument)document;
                // create the inline content
                content = new ByteArrayContent(inlineDoc.Data);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Utils.MediaTypeFinder.GetMediaType(inlineDoc.FileType));
                content.Headers.ContentType.CharSet = inlineDoc.CharSet;
            } 
            else if (document.GetType() == typeof(Entities.LinkedOutboundDocument))
            {
                var uploadedDoc = (Entities.LinkedOutboundDocument)document;
                // create an empty content
                content = new ByteArrayContent(new byte[0]);
                // add the link to the uploaded document
                content.Headers.ContentLocation = uploadedDoc.UploadedDocument;
            }

            return content;
        }
        #endregion

        #region Queries
        /// <summary>
        /// Get a list of recent outbound faxes (which does not include batch faxes).
        /// </summary>
        /// <param name="list">Output: the returned list</param>
        /// <param name="lastId">(optional) Return results from this ID onwards (not including this ID). Used for pagination.
        /// If not provided, MaxValue is set when sortOrder is descending; zero when sortOrder is ascending.</param>
        /// <param name="limit">(Optional) How many transactions to return.</param>
        /// <param name="descendingOrder">Set to false for ascending order</param>
        /// <param name="userId">(Optional, Default is Current user provided in credentials). Enables a "primary" user to query for other account users' faxes.</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetList(out IEnumerable<Entities.OutboundFaxFull> list, string lastId = null, int limit = 25, bool descendingOrder = true, string userId = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["lastId"] = (lastId==null)? ((descendingOrder)? long.MaxValue : 0).ToString() : lastId;
            query["sortOrder"] = (descendingOrder) ? "desc":"asc";
            query["limit"] = limit.ToString();
            query["userId"] = userId;

            return base.GetObject<IEnumerable<Entities.OutboundFaxFull>>("faxes?" + query.ToString(), out list);
        }

        /// <summary>
        /// Get details for a subset of completed faxes from a submitted list. (Submitted id's which have not completed are ignored).
        /// </summary>
        /// <param name="list">Output: the returned list</param>
        /// <param name="Ids">List of transactions to query</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetCompleted(out IEnumerable<Entities.OutboundFaxSummary> list, IEnumerable<string> Ids)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["ids"] = string.Join(",", Ids);
            return base.GetObject<IEnumerable<Entities.OutboundFaxSummary>>("faxes/completed?" + query.ToString(), out list);
        }

        /// <summary>
        /// Retrieves information regarding a previously-submitted fax, including its current status.
        /// </summary>
        /// <param name="meta">The </param>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetStatus(out Entities.OutboundFaxFull meta, string id)
        {
            return base.GetObject<Entities.OutboundFaxFull>("faxes/" + HttpUtility.UrlEncode(id), out meta);
        }

        /// <summary>
        /// Retrieve the fax image (TIFF file) of a submitted fax.
        /// </summary>
        /// <param name="data">If successful, the response returns a TIFF file (image/tiff) of the outgoing fax image.</param>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetImage(out byte[] data, string id)
        {
            return base.GetBinary("faxes/" + HttpUtility.UrlEncode(id) + "/image",out data);
        }

        /// <summary>
        /// Search outbound faxes
        /// </summary>
        /// <param name="list">Output: the returned list</param>
        /// <param name="ids">(Optional, default is no restriction) List of fax IDs</param>
        /// <param name="reference">(Optional, default is no restriction) The 'reference' parameter entered at submit time</param>
        /// <param name="dateFrom">(Optional, default is no restriction) Lower bound of date range from which to return faxes (GMT)</param>
        /// <param name="dateTo">(Optional, default is no restriction) Upper bound of date range frrom which to return faxes</param>
        /// <param name="statusFamily">(Optional, default is 'All') The status family of faxes to return</param>
        /// <param name="status">Must be used in case the statusFamily is set to 'Specific'</param>
        /// <param name="userId">(Optional, default is no restriction) Limit returned faxes to these user ID's. This parameter has effect only when the querying username is a "primary" user.</param>
        /// <param name="faxNumber">(Optional, default is no restriction) Limit returned faxes to this destination fax number.</param>
        /// <param name="descendingOrder">Set to false for ascending order</param>
        /// <param name="offset">(Optional) How many transactions to return.</param>
        /// <param name="limit">(Optional) Skip this many records</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus Search(out IEnumerable<Entities.OutboundFaxFull> list,
            IEnumerable<string> ids = null,
            string reference = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            Enums.StatusFamily? statusFamily = Enums.StatusFamily.All,
            int status = int.MinValue,
            string userId = null,
            string faxNumber = null,
            bool descendingOrder = true,
            int offset = 0,
            int limit = 25
            )
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (ids != null && ids.Count() > 0)
                query["ids"] = string.Join(",",ids);

            if (!string.IsNullOrEmpty(reference))
                query["reference"] = reference;

            if (dateFrom.HasValue)
                query["dateFrom"] = dateFrom.Value.ToString("s");

            if (dateTo.HasValue)
                query["dateTo"] = dateTo.Value.ToString("s");

            // set status
            if (statusFamily.HasValue)
            {
                query["status"] = (statusFamily == Enums.StatusFamily.Specific)? status.ToString() : statusFamily.ToString();
            }

            if (!string.IsNullOrEmpty(userId))
                query["userId"] = userId;

            if (!string.IsNullOrEmpty(faxNumber))
                query["faxNumber"] = faxNumber;

            query["sortOrder"] = (descendingOrder) ? "desc" : "asc";
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            return base.GetObject<IEnumerable<Entities.OutboundFaxFull>>("search?" + query.ToString(), out list);
        }
        #endregion

        #region Other operational methods
        /// <summary>
        /// Cancel a fax in progress.
        /// </summary>
        /// <param name="id">ID of the fax to be cancelled.</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus CancelFax(string id)
        {
            return base.Post("faxes/" + HttpUtility.UrlEncode(id) + "/cancel" ) ;
        }

        /// <summary>
        /// The resent fax is allocated a new transaction ID.
        /// </summary>
        /// <param name="location">Output: the URI of the newly-created fax resource</param>
        /// <param name="id">ID of the fax to be cancelled.</param>
        /// <param name="faxNumber">A single fax number, e.g: +1-212-3456789</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus ResendFax(out Uri location, string id, string faxNumber)
        {
            return base.PostAndGetLocation("faxes/" + HttpUtility.UrlEncode(id) + "/resend", out location);
        }

        /// <summary>
        /// Hide a fax from listing in queries (there is no way to unhide a fax).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Enums.RequestStatus HideFax(string id)
        {
            return base.Post("faxes/" + HttpUtility.UrlEncode(id) + "/hide");
        }
        #endregion
    }
}
