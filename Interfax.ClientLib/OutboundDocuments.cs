using System;
using System.Collections.Generic;
using System.Web;

namespace Interfax.ClientLib
{
    /// <summary>
    /// Represents the document upload facility for outbound faxing
    /// </summary>
    public class OutboundDocuments : Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Interfax User Id</param>
        /// <param name="Password">Interfax password</param>
        /// <param name="timeout">Request timeout (optional, default is 30 sec.)</param>
        /// <param name="endPoint">The service end point (optional). Default is live service</param>
        public OutboundDocuments(string userId, string Password, TimeSpan? timeout = null, string endPoint = null) : base("outbound", userId, Password, timeout, endPoint) { }

        /// <summary>
        /// Uploads an entire document in chuncks
        /// </summary>
        /// <param name="location">Output: the Uri for the created document</param>
        /// <param name="data">Document data</param>
        /// <param name="name">
        /// The document file name, which can subsequently be queried with the GetList() method.
        /// The filename must end with an extension defining the file type, e.g. dailyrates.docx or newsletter.pdf,
        /// and the file type must be in the list of supported file types as specifies in https://www.interfax.net/en/help/supported_file_types.
        /// </param>
        /// <param name="chunckSize">size in bytes for data to uload in each HTTP request</param>
        /// <param name="disposition">This sets the retention policy of the uploaded document, that is, how long it can be used by the POST </param>
        /// <param name="sharing">private or shared</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus Upload(out Uri location, byte[] data, string name, int chunckSize = 218*1024, Enums.DocumentDisposition disposition = Enums.DocumentDisposition.SingleUse, Enums.Sharing sharing = Enums.Sharing.Private)
        {
            // start an upload session
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["size"] = data.Length.ToString();
            query["name"] = name;
            query["disposition"] = disposition.ToString();
            query["sharing"] = sharing.ToString();

            var st = base.PostAndGetLocation("documents?" + query.ToString(), out location);

            // if not created, return 
            if (st != Enums.RequestStatus.Created)
            {
                return st;
            }

            // upload in chuncks
            int pos = 0;
            while (pos<data.Length)
            {
                // upload a single chunk
                var count = Math.Min(chunckSize, data.Length - pos);
                st = base.PostBinary(location.AbsoluteUri, data, pos, count);
                pos += count;

                if (st == Enums.RequestStatus.Accepted)
                    continue; // partial data uploaded

                // in any other case (either success or failure) - break out of loop.
                break;
            }
            return st;
        }

        /// <summary>
        /// Get a list of previous document uploads which are currently available.
        /// </summary>
        /// <param name="limit">How many document references to return.</param>
        /// <param name="offset">Skip this many document references in the list.</param>
        /// <param name="list">Output: the list of documents</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetList(out IEnumerable<Entities.UploadedDocument> list, int limit = 25, int offset = 0)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["limit"] = limit.ToString();
            query["offset"] = offset.ToString();
            return base.GetObject<IEnumerable<Entities.UploadedDocument>>("documents?" + query.ToString(), out list);
        }

        /// <summary>
        /// Retrieve the meta data about a document
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="document"></param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus GetStatus(out Entities.UploadedDocument document, string documentId)
        {
            return base.GetObject<Entities.UploadedDocument>("documents/" + documentId, out document);
        }

        /// <summary>
        /// Delete an uploaded document (also cancels an uploaded document session)
        /// </summary>
        /// <param name="documentId">Id obtained during call to D</param>
        /// <returns>The request status</returns>
        public Enums.RequestStatus Delete(string documentId)
        {
            return base.DeleteUri("documents/" + documentId);
        }
    }
}
