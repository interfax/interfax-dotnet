using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Inbound
    {
        private readonly InterFAX _interfax;

        internal Inbound(InterFAX interfax)
        {
            _interfax = interfax;
        }

        #region GET Methods
        /// <summary>
        /// Retrieves a user's list of inbound faxes. (Sort order is always in descending ID).
        /// </summary>
        /// <param name="listOptions"></param>
        public async Task<IEnumerable<InboundFax>> GetList(ListOptions listOptions = null)
        {
            var options = listOptions == null ? new Dictionary<string, string>() : listOptions.ToDictionary();
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<InboundFax>>("/inbound/faxes", options);
        }

        /// <summary>
        /// Retrieves a single fax's metadata (receive time, sender number, etc.).
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<InboundFax> GetFaxRecord(int id)
        {
            return await _interfax.HttpClient.GetResourceAsync<InboundFax>($"/inbound/faxes/{id}");
        }

        /// <summary>
        /// Retrieves a single fax's metadata (receive time, sender number, etc.).
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<IEnumerable<ForwardingEmail>> GetForwardingEmails(int id)
        {
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<ForwardingEmail>>($"/inbound/faxes/{id}/emails");
        }

        /// <summary>
        /// Retrieve the fax image (TIFF file) of a received fax.
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<Stream> GetFaxImageStream(int id)
        {
            return await _interfax.HttpClient.GetStreamAsync($"/inbound/faxes/{id}/image");
        }
        #endregion

        #region POST Methods
        /// <summary>
        /// Mark a fax as read.
        /// </summary>
        /// <param name="id">The message ID of the fax to mark as read.</param>
        public async Task<string> MarkRead(int id)
        {
            return await _interfax.HttpClient.PostAsync($"/inbound/faxes/{id}/mark");
        }

        /// <summary>
        /// Mark a fax as unread.
        /// </summary>
        /// <param name="id">The message ID of the fax to mark as read.</param>
        public async Task<string> MarkUnread(int id)
        {
            return await _interfax.HttpClient.PostAsync($"/inbound/faxes/{id}/mark?unread=true");
        }

        /// <summary>
        /// Resend an inbound fax, optionally to a specific email address.
        /// </summary>
        /// <param name="id">The message ID of the fax to resend.</param>
        /// <param name="emailAddress">Optional email address to forward the inbound fax on to.</param>
        public async Task<string> Resend(int id, string emailAddress = null)
        {
            var options = string.IsNullOrEmpty(emailAddress)
                ? null
                : new Dictionary<string, string> {{"emailAddress", emailAddress}};
            return await _interfax.HttpClient.PostAsync($"/inbound/faxes/{id}/resend", options);
        }
        #endregion
    }
}