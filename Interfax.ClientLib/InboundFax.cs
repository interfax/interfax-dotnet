using System;
using System.Collections.Generic;
using System.Web;

namespace Interfax.ClientLib
{
    /// <summary>
    /// Client for the Inbound fax service
    /// </summary>
    public class InboundFax : Base
    {
        #region Constructor and private members
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userId">Interfax User Id</param>
        /// <param name="password">Interfax password</param>
        /// <param name="timeout">Request timeout (optional, default is 30 sec.)</param>
        /// <param name="endPoint">The service end point (optional). Default is live service</param>
        public InboundFax(string userId, string password, TimeSpan? timeout = null, string endPoint = null) : base("inbound",userId, password, timeout, endPoint) { }
        #endregion

        #region public operational methods
        /// <summary>
        /// Retrieves a user's list of inbound faxes. (Sort order is always by descending ID).
        /// </summary>
        /// <param name="list">output: The list of meta data for each fax</param>
        /// <param name="lastId">Optional: Return results from this ID backwards (not including this ID). Used for pagination.</param> //TODO: check sort order
        /// <param name="unreadOnly">Optional (default is false). Return only unread faxes?</param>
        /// <param name="limit">Optional (default is 25). How many transactions to return.</param>
        /// <param name="allUsers">Optional (default is false). For a "primary" user, determines whether to return data for the current user only or for all account users.</param>
        /// <returns>Request status</returns>
        public Enums.RequestStatus GetList(out IEnumerable<Entities.InboundFax> list, bool unreadOnly = false, int limit = 25, bool allUsers = false, int lastId = int.MaxValue)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["lastId"] = lastId.ToString();
            query["unreadOnly"] = unreadOnly.ToString();
            query["limit"] = limit.ToString();
            query["allUsers"] = allUsers.ToString();

            return base.GetObject<IEnumerable<Entities.InboundFax>>("faxes?" + query.ToString(), out list);
        }

        /// <summary>
        /// Retrieve meta data for a specific fax
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <param name="meta">Output: The meta data for the fax</param>
        /// <returns>Request status</returns>
        public Enums.RequestStatus GetMeta(out Entities.InboundFax meta, int id)
        {
            return base.GetObject<Entities.InboundFax>("faxes/" + id.ToString(), out meta);
        }

        /// <summary>
        /// Retrieve Image data for a specific fax, Format is determined by the user's setting
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <param name="image">Output: The image data for the fax</param>
        /// <returns>Request status</returns>
        public Enums.RequestStatus GetImage(out byte[] image, int id)
        {
            return base.GetBinary("faxes/" + id.ToString() + "/image", out image);
        }

        /// <summary>
        /// Mark an image as read (or unread)
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <param name="unread">Optional: set false to mark as Unread</param>
        /// <returns></returns>
        public Enums.RequestStatus MarkRead(int id, bool unread = true)
        {
            return base.Post(string.Format("faxes/{0}/mark?unread={1}",id,unread));
        }
        #endregion
    }
}
