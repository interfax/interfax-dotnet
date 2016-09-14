using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Outbound
    {
        private readonly InterFAX _interfax;

        internal Outbound(InterFAX interfax)
        {
            _interfax = interfax;

            Documents = new Documents(interfax);
        }

        public Documents Documents { get; set; }

        #region Query Methods
        /// <summary>
        /// Get a list of recent outbound faxes (which does not include batch faxes).
        /// </summary>
        /// <param name="listOptions"></param>
        public async Task<IEnumerable<OutboundFaxSummary>> GetList(ListOptions listOptions = null)
        {
            var options = listOptions == null? new Dictionary<string, string>() : listOptions.ToDictionary();
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFaxSummary>>("/outbound/faxes", options);
        }

        /// <summary>
        /// Get details for a subset of completed faxes from a submitted list. (Submitted id's which have not completed are ignored).
        /// </summary>
        /// <param name="ids">Array of fax id's to retrieve, if they have completed.</param>
        public async Task<IEnumerable<OutboundFaxSummary>> GetCompleted(IEnumerable<int> ids = null)
        {
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFaxSummary>>("/outbound/faxes/completed");
        }

        /// <summary>
        /// Retrieves information regarding a previously-submitted fax, including its current status.
        /// </summary>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        public async Task<OutboundFaxSummary> GetFaxRecord(int id)
        {
            return await _interfax.HttpClient.GetResourceAsync<OutboundFaxSummary>($"/outbound/faxes/{id}");
        }

        /// <summary>
        /// Retrieve the fax image (TIFF file) of a submitted fax.
        /// </summary>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        public async Task<Stream> GetFaxImageStream(int id)
        {
            return await _interfax.HttpClient.GetStreamAsync($"/outbound/faxes/{id}/image");
        }

        /// <summary>
        /// Search fax list.
        /// 
        /// This operation will return individual faxes (i.e., /faxes resources) as well as child faxes of batches (i.e., /batches/{id}/children resources).
        /// </summary>
        /// <param name="searchOptions"></param>
        public async Task<IEnumerable<OutboundFax>> SearchFaxes(SearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentException("searchOptions");

            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFax>>("/outbound/search", searchOptions.ToDictionary());
        }
        #endregion

        #region Sending Faxes
        /// <summary>
        /// Submit a fax to a single destination number.
        /// </summary>
        public async Task<Uri> SendFax(SendOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancel a fax in progress.
        /// </summary>
        /// <param name="id">The message ID of the fax to cancel.</param>
        public async Task<string> CancelFax(int id)
        {
            var response = await _interfax.HttpClient.PostAsync($"/outbound/faxes/{id}/cancel");
            return response.ReasonPhrase;
        }

        /// <summary>
        /// Resend a previously-submitted fax, without needing to re-upload the original document.
        /// 
        /// The resent fax is allocated a new message ID.
        /// </summary>
        /// <param name="id">The message ID of the fax to resend.</param>
        /// <param name="faxNumber">(optional) The new destination fax number to which this fax should be sent.</param>
        public async Task<Uri> ResendFax(int id, string faxNumber = null)
        {
            var options = string.IsNullOrEmpty(faxNumber)
                ? null
                : new Dictionary<string, string> { { "faxNumber", faxNumber } };
            var response = await _interfax.HttpClient.PostAsync($"/outbound/faxes/{id}/resend", options);
            return response.Headers.Location;
        }

        /// <summary>
        /// Hide a fax from listing in queries (there is no way to unhide a fax).
        /// </summary>
        /// <param name="id">The message ID of the fax to hide.</param>
        public async Task<string> HideFax(int id)
        {
            var response = await _interfax.HttpClient.PostAsync($"/outbound/faxes/{id}/hide");
            return response.ReasonPhrase;
        }
        #endregion
    }
}