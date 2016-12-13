using System.Collections.Generic;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Documents
    {
        public class UploadSessionOptions : IOptions
        {
            /// <summary>
            /// Size of the document to be uploaded (in bytes)
            /// </summary>
            public long Size { get; set; }

            /// <summary>
            /// The document file name, which can subsequently be queried. 
            /// The filename must end with an extension defining the file type, e.g. dailyrates.docx or newsletter.pdf, and the file type must be in the list of supported file types.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Sets the retention policy of the uploaded document.
            /// </summary>
            public DocumentDisposition? Disposition { get; set; }

            /// <summary>
            /// The sharing policy of the uploaded document.
            /// </summary>
            public DocumentSharing? Sharing { get; set; }

            public Dictionary<string, string> ToDictionary()
            {
                var options = new Dictionary<string, string> {{"size", Size.ToString()}, {"name", Name}};
                if (Disposition.HasValue) options.Add("disposition", Disposition.ToCamelCase());
                if (Sharing.HasValue) options.Add("sharing", Sharing.ToCamelCase());
                return options;
            }
        }
    }
}