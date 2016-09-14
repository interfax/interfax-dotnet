using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using InterFAX.Api.Dtos;

namespace InterFAX.Api
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> GetResourceAsync<T>(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.GetAsync(requestUri.AddOptions(options));
            var response = await task;
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<T>();

            throw new ApiException(response.StatusCode, await response.ToError());        
        }

        public static async Task<T> PostAsync<T>(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.PostAsync(requestUri.AddOptions(options), new StringContent(""));
            var response = await task;
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<T>();

            throw new ApiException(response.StatusCode, await response.ToError());
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.PostAsync(requestUri.AddOptions(options), new StringContent(""));
            var response = await task;
            if (response.IsSuccessStatusCode) return response;

            throw new ApiException(response.StatusCode, await response.ToError());
        }

        public static string AddOptions(this string requestUri, Dictionary<string, string> options)
        {
            if (options == null || options.Keys.Count <= 0) return requestUri;

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var key in options.Keys)
                queryString[key] = options[key];
            return $"{requestUri}?{queryString}";
        }

        private static async Task<Error> ToError(this HttpResponseMessage response)
        {
            try { return await response.Content.ReadAsAsync<Error>(); }
            catch { return new Error { Code = (int)response.StatusCode, Message = await response.Content.ReadAsStringAsync(), MoreInfo = response.ReasonPhrase }; }
        }
    }
}