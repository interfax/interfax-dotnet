using System.Collections.Generic;

namespace InterFAX.Api
{
    public partial class Outbound
    {
        public class ListOptions
        {
            /// <summary>
            /// How many transactions to return.
            /// </summary>
            public int? Limit { get; set; }

            /// <summary>
            /// Return results from this ID onwards (not including this ID). Used for pagination.
            /// </summary>
            public int? LastId { get; set; }

            /// <summary>
            /// Ascending or descending. Sorts by fax ID. (Default is descending)
            /// </summary>
            public ListSortOrder SortOrder { get; set; } = ListSortOrder.Descending;

            /// <summary>
            /// Enables a "primary" user to query for other account users' faxes.
            /// </summary>
            public string UserId { get; set; }

            public Dictionary<string, string> ToDictionary()
            {
                var options = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(UserId)) options.Add("userId", UserId);
                if (LastId.HasValue) options.Add("lastId", LastId.ToString());
                if (Limit.HasValue) options.Add("limit", Limit.ToString());
                if (SortOrder == ListSortOrder.Ascending) options.Add("sortOrder", "asc");
                return options;
            }
        }
    }
}