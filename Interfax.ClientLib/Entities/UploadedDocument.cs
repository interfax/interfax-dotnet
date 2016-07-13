using System;

namespace Interfax.ClientLib.Entities
{
    /// <summary>
    /// represents a single uploaded document meta data
    /// </summary>
    public class UploadedDocument
    {
        /// <summary>
        /// Username under which document was created.
        /// </summary>
        public String userId;
        /// <summary>
        /// The filename provided when the document was uploaded, e.g. newsletter.pdf
        /// </summary>
        public 	String fileName;
        /// <summary>
        /// The planned size in bytes of the document.
        /// </summary>
        public int fileSize;
        /// <summary>
        /// The number of bytes actually uploaded.
        /// </summary>
        public int uploaded;
        /// <summary>
        /// Fully-qualified resource URI of the document.
        /// </summary>
        public Uri uri;
        /// <summary>
        /// Time (UTC) when the document was created
        /// </summary>
        public DateTime creationTime;
        /// <summary>
        /// Time (UTC) when the document was last used
        /// </summary>
        public DateTime lastUsageTime;

        /// <summary>
        /// Current status of the document
        /// </summary>
        public Enums.DocumentStatus documentStatus;

        /// <summary>
        /// Document disposition definition
        /// </summary>
        public Enums.DocumentDisposition disposition;

        /// <summary>
        /// Document sharing definition
        /// </summary>
        public Enums.Sharing sharing;
    }
}
