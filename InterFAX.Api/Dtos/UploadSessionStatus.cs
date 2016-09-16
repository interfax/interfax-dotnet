using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api.Dtos
{
    /// <summary>
    /// Meta data about a document upload session.
    /// </summary>
    public class UploadSession
    {
        /// <summary>
        /// The id of the user who uploaded the document.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The filename provided when the document was uploaded, e.g. newsletter.pdf
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The planned size in bytes of the document.
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// The number of bytes actually uploaded.
        /// </summary>
        public int Uploaded { get; set; }

        /// <summary>
        /// Fully-qualified resource URI of the document.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The time the document was created.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// When the document was last used.
        /// </summary>
        public DateTime LastUsageTime { get; set; }

        /// <summary>
        /// One of created, partiallyUploaded, uploading, ready, deleting
        /// </summary>
        public DocumentStatus Status { get; set; }

        /// <summary>
        /// singleUse, multiUse, permanent
        /// </summary>
        public DocumentDisposition Disposition { get; set; }

        /// <summary>
        /// private, shared
        /// </summary>
        public DocumentSharing Sharing { get; set; }
    }
}