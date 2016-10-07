using System;
using System.Collections.Generic;

namespace InterFAX.Api
{
    public partial class Inbound
    {
        public class ListOptions : IOptions
        {
            /// <summary>
            /// Return only unread faxes?
            /// </summary>
            public bool UnreadOnly { get; set; } = false;

            /// <summary>
            /// How many transactions to return.
            /// </summary>
            public int? Limit { get; set; }

            /// <summary>
            /// Return results from this ID onwards (not including this ID). Used for pagination.
            /// </summary>
            public int? LastId { get; set; }

            /// <summary>
            /// For a "primary" user, determines whether to return data for the current user only or for all account users.
            /// </summary>
            public bool AllUsers { get; set; } = false;

            public Dictionary<string, string> ToDictionary()
            {
                var options = new Dictionary<string, string>();
                if (UnreadOnly) options.Add("unreadOnly", "true");
                if (Limit.HasValue) options.Add("limit", Limit.ToString());
                if (LastId.HasValue) options.Add("lastId", LastId.ToString());
                if (AllUsers) options.Add("allUsers", "true");
                return options;
            }
        }
    }
}