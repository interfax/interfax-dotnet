using System.Collections.Generic;

namespace InterFAX.Api
{
    public partial class Documents
    {
        public class ListOptions : IOptions
        {
            /// <summary>
            /// How many document references to return.
            /// </summary>
            public int? Limit { get; set; }

            /// <summary>
            /// Skip this many document references in the list.
            /// </summary>
            public int? Offset { get; set; }

            public Dictionary<string, string> ToDictionary()
            {
                var options = new Dictionary<string, string>();
                if (Offset.HasValue) options.Add("offset", Offset.ToString());
                if (Limit.HasValue) options.Add("limit", Limit.ToString());
                return options;
            }
        }
    }
}