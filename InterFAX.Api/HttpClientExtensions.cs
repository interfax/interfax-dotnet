using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace InterFAX.Api
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> GetResourceAsync<T>(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.GetAsync(requestUri.AddOptions(options));
            var response = await task;
            if (!response.IsSuccessStatusCode)
                throw new ApiException(response.StatusCode, response.ReasonPhrase);
            return await response.Content.ReadAsAsync<T>();
        }

        public static async Task<T> PostWithResultAsync<T>(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.PostAsync(requestUri.AddOptions(options), new StringContent(""));
            var response = await task;
            if (!response.IsSuccessStatusCode)
                throw new ApiException(response.StatusCode, response.ReasonPhrase);
            return await response.Content.ReadAsAsync<T>();
        }

        public static async Task<string> PostAsync(this HttpClient httpClient, string requestUri, Dictionary<string, string> options = null)
        {
            var task = httpClient.PostAsync(requestUri.AddOptions(options), new StringContent(""));
            var response = await task;
            if (!response.IsSuccessStatusCode)
                throw new ApiException(response.StatusCode, response.ReasonPhrase);
            return await response.Content.ReadAsAsync<string>();
        }

        public static string AddOptions(this string requestUri, Dictionary<string, string> options)
        {
            if (options == null || options.Keys.Count <= 0) return requestUri;

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var key in options.Keys)
                queryString[key] = options[key];
            return $"{requestUri}?{queryString}";
        }
    }
}