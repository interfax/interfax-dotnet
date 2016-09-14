using System.Collections.Generic;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    public partial class Documents
    {
        private readonly InterFAX _interfax;

        internal Documents(InterFAX interfax)
        {
            _interfax = interfax;
        }

        /// <summary>
        /// Get a list of recent outbound faxes (which does not include batch faxes).
        /// </summary>
        /// <param name="listOptions"></param>
        public async Task<IEnumerable<OutboundDocument>> GetList(ListOptions listOptions = null)
        {
            var options = listOptions == null ? new Dictionary<string, string>() : listOptions.ToDictionary();
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<OutboundDocument>>("/outbound/documents", options);
        }
    }
}