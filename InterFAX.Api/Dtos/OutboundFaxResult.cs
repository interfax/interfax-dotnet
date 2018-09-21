using System;

namespace InterFAX.Api.Dtos
{
    public class OutboundFaxResult
    {
        /// <summary>
        /// A unique identifier for the fax.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// A unique resource locator for the fax.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Fax status. Generally, 0= OK; less than 0 = in process; greater than 0 = Error (See Interfax Status Codes)
        /// </summary>
        public int Status { get; set; }
    }
}