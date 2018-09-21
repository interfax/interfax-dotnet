using System;
using System.Collections.Generic;
using System.Linq;

namespace InterFAX.Api
{
    public class SearchOptions : IOptions
    {
        /// <summary>
        /// List of fax IDs.
        /// </summary>
        public Int64[] Ids { get; set; }

        /// <summary>
        /// The 'reference' parameter entered at submit time.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Lower bound of date range from which to return faxes.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Upper bound of date range frrom which to return faxes.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// A specific value from the list of possible fax status codes.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Completed = return only completed faxes, whether successful or failed, 
        /// Success = return only successfully-completed faxes, 
        /// Failed = return only failed faxes, 
        /// Inprocess = return only faxes in process, 
        /// All = return faxes of any status. (default)
        /// </summary>
        public StatusFamily? StatusFamily { get; set; }

        /// <summary>
        /// Limit returned faxes to these user ID's. This parameter has effect only when the querying username is a "primary" user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Limit returned faxes to this destination fax number.
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// Ascending or descending. Sorts by fax ID. (Default is descending)
        /// </summary>
        public ListSortOrder SortOrder { get; set; } = ListSortOrder.Descending;

        /// <summary>
        /// Skip this many records.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// How many transactions to return.
        /// </summary>
        public int? Limit { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var options = new Dictionary<string, string>();
            if (Ids != null) options.Add("ids", string.Join(",", Ids));
            if (!string.IsNullOrEmpty(Reference)) options.Add("reference", Reference);
            if (DateFrom.HasValue) options.Add("dateFrom", DateFrom.Value.ToString("s") + "Z");
            if (DateTo.HasValue) options.Add("dateTo", DateTo.Value.ToString("s") + "Z");
            if (Status.HasValue)
                options.Add("status", Status.ToString());
            else if (StatusFamily.HasValue)
                options.Add("status", StatusFamily.ToString());
            if (!string.IsNullOrEmpty(UserId)) options.Add("userId", UserId);
            if (!string.IsNullOrEmpty(FaxNumber)) options.Add("faxNumber", FaxNumber);
            if (SortOrder == ListSortOrder.Ascending) options.Add("sortOrder", "asc");
            if (Offset.HasValue) options.Add("offset", Offset.ToString());
            if (Limit.HasValue) options.Add("limit", Limit.ToString());
            return options;
        }
    }
}