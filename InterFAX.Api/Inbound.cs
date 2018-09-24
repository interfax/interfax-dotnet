using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Inbound
    {
        private readonly FaxClient _interfax;
        private const string ResourceUri = "/inbound/faxes";

        internal Inbound(FaxClient interfax)
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
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<InboundFax>>(ResourceUri, listOptions);
        }

        /// <summary>
        /// Retrieves a single fax's metadata (receive time, sender number, etc.).
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<InboundFax> GetFaxRecord(Int64 id)
        {
            return await _interfax.HttpClient.GetResourceAsync<InboundFax>($"{ResourceUri}/{id}");
        }

        /// <summary>
        /// Retrieves a single fax's metadata (receive time, sender number, etc.).
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<IEnumerable<ForwardingEmail>> GetForwardingEmails(Int64 id)
        {
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<ForwardingEmail>>($"{ResourceUri}/{id}/emails");
        }

        /// <summary>
        /// Retrieve the fax image (TIFF file) of a received fax.
        /// </summary>
        /// <param name="id">The message ID of the fax for which to retrieve data.</param>
        public async Task<Stream> GetFaxImageStream(Int64 id)
        {
            return await _interfax.HttpClient.GetStreamAsync($"{ResourceUri}/{id}/image");
        }
        #endregion

        #region POST Methods
        /// <summary>
        /// Mark a fax as read.
        /// </summary>
        /// <param name="id">The message ID of the fax to mark as read.</param>
        public async Task<string> MarkRead(Int64 id)
        {
            var response = await _interfax.HttpClient.PostAsync($"{ResourceUri}/{id}/mark");
            return response.ReasonPhrase;
        }

        /// <summary>
        /// Mark a fax as unread.
        /// </summary>
        /// <param name="id">The message ID of the fax to mark as read.</param>
        public async Task<string> MarkUnread(Int64 id)
        {
            var response = await _interfax.HttpClient.PostAsync($"{ResourceUri}/{id}/mark?unread=true");
            return response.ReasonPhrase;
        }

        /// <summary>
        /// Resend an inbound fax, optionally to a specific email address.
        /// </summary>
        /// <param name="id">The message ID of the fax to resend.</param>
        /// <param name="emailAddress">Optional email address to forward the inbound fax on to.</param>
        public async Task<string> Resend(Int64 id, string emailAddress = null)
        {
            var requestUri = $"{ResourceUri}/{id}/resend";
            if (!string.IsNullOrEmpty(emailAddress)) requestUri += $"?emailAddress={emailAddress}";
            var response = await _interfax.HttpClient.PostAsync(requestUri);
            return response.ReasonPhrase;
        }
        #endregion
    }
}