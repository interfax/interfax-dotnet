using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Outbound
    {
        private readonly FaxClient _interfax;
        private const string ResourceUri = "/outbound/faxes";

        internal Outbound(FaxClient interfax)
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
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFaxSummary>>(ResourceUri, listOptions);
        }

        /// <summary>
        /// Get details for a subset of completed faxes from a submitted list. (Submitted id's which have not completed are ignored).
        /// </summary>
        /// <param name="ids">Array of fax id's to retrieve, if they have completed.</param>
        public async Task<IEnumerable<OutboundFaxSummary>> GetCompleted(params Int64[] ids)
        {
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFaxSummary>>($"{ResourceUri}/completed?ids={String.Join(",", ids)}");
        }

        /// <summary>
        /// Retrieves information regarding a previously-submitted fax, including its current status.
        /// </summary>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        public async Task<OutboundFaxSummary> GetFaxRecord(Int64 id)
        {
            return await _interfax.HttpClient.GetResourceAsync<OutboundFaxSummary>($"{ResourceUri}/{id}");
        }

        /// <summary>
        /// Retrieve the fax image (TIFF file) of a submitted fax.
        /// </summary>
        /// <param name="id">The transaction ID of the fax for which to retrieve data.</param>
        public async Task<Stream> GetFaxImageStream(Int64 id)
        {
            return await _interfax.HttpClient.GetStreamAsync($"{ResourceUri}/{id}/image");
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

            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundFax>>("/outbound/search", searchOptions);
        }
        #endregion

        #region Sending Faxes
        /// <summary>
        /// Submit a fax with multiple documents to a destination number.
        /// </summary>
        /// <returns>The messageId of the newly created fax.</returns>
        public async Task<Int64> SendFax(List<IFaxDocument> faxDocuments, SendOptions options)
        {
            var content = new MultipartContent();
            foreach(var faxDocument in faxDocuments)
                content.Add(faxDocument.ToHttpContent());
            var response = await _interfax.HttpClient.PostAsync(ResourceUri, options, content);
            return Convert.ToInt64(response.Headers.Location.Segments.Last());
        }

        /// <summary>
        /// Submit a fax with a single document to a destination number.
        /// </summary>
        /// <returns>The messageId of the newly created fax.</returns>
        public async Task<Int64> SendFax(IFaxDocument faxDocument, SendOptions options)
        {
            return await SendFax(new List<IFaxDocument> {faxDocument}, options);
        }
        #endregion

        #region Modify Existing Faxes
        /// <summary>
        /// Cancel a fax in progress.
        /// </summary>
        /// <param name="id">The message ID of the fax to cancel.</param>`
        public async Task<string> CancelFax(Int64 id)
        {
            var response = await _interfax.HttpClient.PostAsync($"{ResourceUri}/{id}/cancel");
            return response.ReasonPhrase;
        }

        /// <summary>
        /// Resend a previously-submitted fax, without needing to re-upload the original document.
        /// 
        /// The resent fax is allocated a new message ID.
        /// </summary>
        /// <param name="id">The message ID of the fax to resend.</param>
        /// <param name="faxNumber">(optional) The new destination fax number to which this fax should be sent.</param>
        /// <returns>The messageId of the newly created fax.</returns>
        public async Task<Int64> ResendFax(Int64 id, string faxNumber = null)
        {
            var requestUri = $"{ResourceUri}/{id}/resend";
            if (!string.IsNullOrEmpty(faxNumber)) requestUri += $"?faxNumber={faxNumber}";
            var response = await _interfax.HttpClient.PostAsync(requestUri);
            return Convert.ToInt64(response.Headers.Location.Segments.Last());
        }

        /// <summary>
        /// Hide a fax from listing in queries (there is no way to unhide a fax).
        /// </summary>
        /// <param name="id">The message ID of the fax to hide.</param>
        public async Task<string> HideFax(Int64 id)
        {
            var response = await _interfax.HttpClient.PostAsync($"{ResourceUri}/{id}/hide");
            return response.ReasonPhrase;
        }
        #endregion
    }
}