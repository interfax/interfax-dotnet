using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Interfax.ClientLib
{
    /// <summary>
    /// Base class for services classes
    /// </summary>
    abstract public class Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="basePath">The base path for the specific service</param>
        /// <param name="userId">Interfax User Id</param>
        /// <param name="password">Interfax password</param>
        /// <param name="timeout">Request timeout (optional, default is 30 sec.)</param>
        /// <param name="endPoint">The service end point (optional). Default is live service</param>
        protected Base(string basePath, string userId, string password, TimeSpan? timeout = null, string endPoint = null)
        {
            // set default value for timeout
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(30);

            // set default value for timeout
            if (string.IsNullOrEmpty( endPoint))
                endPoint = @"https://rest.interfax.net/";
            
            //build with basepath
            UriBuilder builder = new UriBuilder(endPoint);
            if (!basePath.EndsWith("/"))
                basePath += "/";

            builder.Path = basePath;

            // set base uri
            client.BaseAddress = builder.Uri;

            // set timeout
            client.Timeout = timeout.Value;

            // Add an Accept header for JSON format.
            string accept_format = "application/json";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept_format));

            // add authentication header
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userId, password)));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // add AcceptEncoding header
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        /// <summary>
        /// Error message of last operation
        /// </summary>
        public string LastError { get { return _lastError; } }

        /// <summary>
        /// Detailed error block of last operation
        /// </summary>
        public Entities.ErrorBlock ErrorBlock { get { return _errorBlock; } }

        /// <summary>
        /// The Http client to use
        /// </summary>
        private HttpClient client = new HttpClient();
        private string _lastError;
        private Entities.ErrorBlock _errorBlock;

        #region Helper methods
        /// <summary>
        /// Send a Get request and populate a response to a .NET object of a given type
        /// </summary>
        /// <typeparam name="T">Type of result</typeparam>
        /// <param name="uri">Request Uri</param>
        /// <param name="result">Output: the resulting object</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus GetObject<T>(string uri, out T result)
        {
            result = default(T);
            byte[] data;
            var st = GetBinary(uri, out data);

            if (st == Enums.RequestStatus.OK)
            {
                st = GetObject<T>(data, out result);
            }
            return st;
        }

        /// <summary>
        /// Send a Get request and populate a binary response
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <param name="result">Output: the resulting binary data</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus GetBinary(string uri, out byte[] result)
        {
            result = null;

            try
            {
                var response = client.GetAsync(uri).Result;
                _lastError = response.ReasonPhrase + " " + (int)response.StatusCode;
                _errorBlock = null;

                result = response.Content.ReadAsByteArrayAsync().Result;

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        _lastError = "";
                        return Enums.RequestStatus.OK;

                    case System.Net.HttpStatusCode.NotFound:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.NotFound;

                    case System.Net.HttpStatusCode.Forbidden:
                    case System.Net.HttpStatusCode.Unauthorized:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.AuthenticationError;

                    default:
                        return Enums.RequestStatus.SystemError;
                }

            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return Enums.RequestStatus.SystemError;
            }
        }

        /// <summary>
        /// Send a POST request with content of a given type and populate a response on another type
        /// </summary>
        /// <typeparam name="Tin">Type of input</typeparam>
        /// <typeparam name="Tout">Type of result</typeparam>
        /// <param name="uri">Request Uri</param>
        /// <param name="input">Input object</param>
        /// <param name="result">Output: the resulting object</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus Post<Tin, Tout>(string uri, Tin input, out Tout result)
        {
            result = default(Tout);
            Enums.RequestStatus st;

            try
            {
                var jsonString = JsonConvert.SerializeObject(input);
                var response = client.PostAsync(uri, new StringContent(jsonString, Encoding.UTF8)).Result;

                byte[] data;
                st = ParseBinary(response, out data);

                if (st == Enums.RequestStatus.OK)
                {
                    st = GetObject<Tout>(data, out result);
                }

                return st;
            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return Enums.RequestStatus.SystemError;
            }
        }

        /// <summary>
        /// Send a POST request with no content and no returned object
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus Post(string uri)
        {
            string output; // dummy
            return Post<string, string>(uri, "", out output);
        }


        /// <summary>
        /// Send a POST request with no content and no returned object
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <param name="location">Output: The value in the Locationn Http header</param>
        /// <param name="content">Optional: the content to be posted</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus PostAndGetLocation(string uri, out Uri location, HttpContent content = null)
        {
            location = null;
            try
            {
                var response = client.PostAsync(uri,content).Result;

                byte[] data;
                var st = ParseBinary(response, out data);

                if (st == Enums.RequestStatus.Created)
                {
                    location = response.Headers.Location;
                }

                return st;
            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return Enums.RequestStatus.SystemError;
            }
        }


        /// <summary>
        /// Send a POST request with binary content and no returned object
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <param name="data">Binary data to be sent</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus PostBinary(string uri, byte[] data)
        {
            var content = new ByteArrayContent(data);
            return PostBinary(uri, content);
        }

        /// <summary>
        /// Send a POST request with binary content and no returned object
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <param name="data">Binary data to be sent</param>
        /// <param name="offset">position in data to start transfer from</param>
        /// <param name="count">Number of bytes from offset to transfer</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus PostBinary(string uri, byte[] data, int offset, int count)
        {
            var content = new ByteArrayContent(data,offset,count);
            return PostBinary(uri, content, new RangeHeaderValue(offset, offset + count - 1));
        }

        /// <summary>
        /// Sends a Delete Http request to a given Uri
        /// </summary>
        /// <param name="uri">the Uri</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus DeleteUri(string uri)
        {
            var response = client.DeleteAsync(uri).Result;
            byte[] data;
            return ParseBinary(response, out data);
        }

        /// <summary>
        /// Send a POST request with binary content and no returned object
        /// </summary>
        /// <param name="uri">Request Uri</param>
        /// <param name="content">Content to be posted</param>
        /// <param name="range">Optional: a range header to be set (default is null)</param>
        /// <returns>The request status</returns>
        protected Enums.RequestStatus PostBinary(string uri, HttpContent content, RangeHeaderValue range = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = content;
                request.Headers.Range = range;
                var response = client.SendAsync(request).Result;
                byte[] data; //dummy
                return ParseBinary(response, out data);
            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return Enums.RequestStatus.SystemError;
            }
        }
        #endregion

        #region Private helper methods
        /// <summary>
        /// Get an object of a given type from a binary content 
        /// </summary>
        /// <typeparam name="T">The type of the resulting object</typeparam>
        /// <param name="data">binary content</param>
        /// <param name="result">Output: the resulting object</param>
        /// <returns>The request status</returns>
        private Enums.RequestStatus GetObject<T>(byte[] data, out T result)
        {
            string jsonString = "";
            try
            {
                jsonString = Encoding.UTF8.GetString(data);
                result = JsonConvert.DeserializeObject<T>(jsonString);
                return Enums.RequestStatus.OK;
            }
            catch
            {
                _lastError = "Could not parse Json response: " + jsonString;
                result = default(T);
                return Enums.RequestStatus.SystemError;
            }

        }

        /// <summary>
        /// Perse a response and get its content as binary data
        /// </summary>
        /// <param name="response">The ResponseMessage object received from server</param>
        /// <param name="result">The resulting byte array</param>
        /// <returns>The request status</returns>
        private Enums.RequestStatus ParseBinary(HttpResponseMessage response, out byte[] result)
        {
            result = null;

            try
            {
                _lastError = response.ReasonPhrase + " " + (int)response.StatusCode;
                _errorBlock = null;

                result = response.Content.ReadAsByteArrayAsync().Result;

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        _lastError = "";
                        return Enums.RequestStatus.OK;

                    case System.Net.HttpStatusCode.NotFound:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.NotFound;

                    case System.Net.HttpStatusCode.Forbidden:
                    case System.Net.HttpStatusCode.Unauthorized:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.AuthenticationError;

                    case System.Net.HttpStatusCode.Accepted:
                        _lastError = "";
                        return Enums.RequestStatus.Accepted;

                    case System.Net.HttpStatusCode.Created:
                        _lastError = "";
                        return Enums.RequestStatus.Created;

                    case System.Net.HttpStatusCode.RequestEntityTooLarge:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.OverLimits;

                    default:
                        ExtractErrorBlock(response);
                        return Enums.RequestStatus.SystemError;
                }

            }
            catch (Exception ex)
            {
                _lastError = ex.ToString();
                return Enums.RequestStatus.SystemError;
            }
        }

        /// <summary>
        /// Try to extract an error block from the response content and deserialize it 
        /// </summary>
        /// <param name="res">The response message object</param>
        private void ExtractErrorBlock(HttpResponseMessage res)
        {
            try
            {
                var jsonString = res.Content.ReadAsStringAsync().Result;
                _errorBlock = JsonConvert.DeserializeObject<Entities.ErrorBlock>(jsonString);
            }
            catch
            {
                _errorBlock = null;
            }
        }        
        #endregion
    }
}
