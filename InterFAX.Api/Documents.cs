using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using InterFAX.Api.Dtos;
using Newtonsoft.Json;

namespace InterFAX.Api
{
    public partial class Documents
    {
        private readonly InterFAX _interfax;
        private const string ResourceUri = "/outbound/documents";

        internal Documents(InterFAX interfax)
        {
            _interfax = interfax;

            // Populate the SupportedMediaTypes collection from a configuration file.
            // We expect this to be alongside the assembly.
            var assemblyPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            var typesFile = Path.Combine(assemblyPath, "SupportedMediaTypes.json");
            if (!File.Exists(typesFile))
                throw new FileNotFoundException("Could not find the SupportedMediaTypes configuration file.");

            var mappings = JsonConvert.DeserializeObject<List<MediaTypeMapping>>(File.ReadAllText(typesFile));
            SupportedMediaTypes = mappings.ToDictionary(
                        mapping => mapping.FileType,
                        mapping => mapping.MediaType);
        }

        public Dictionary<string, string> SupportedMediaTypes { get; set; }

        public IFaxDocument BuildFaxDocument(Uri fileUri)
        {
            return new UriDocument(fileUri);
        }

        public IFaxDocument BuildFaxDocument(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var extension = Path.GetExtension(filePath) ?? "*";
            extension = extension.TrimStart('.');

            var mediaType = SupportedMediaTypes.Keys.Contains(extension)
                ? SupportedMediaTypes[extension]
                : "application/octet-stream";

            return new FileDocument(filePath, mediaType);
        }

        /// <summary>
        /// Get a list of previous document upload sessions which are currently available.
        /// </summary>
        /// <param name="listOptions"></param>
        public async Task<IEnumerable<UploadSession>> GetList(ListOptions listOptions = null)
        {
            return await _interfax.HttpClient.GetResourceAsync<IEnumerable<UploadSession>>(ResourceUri, listOptions);
        }

        /// <summary>
        /// Get a single upload session status object.
        /// </summary>
        /// <param name="sessionId"></param>
        public async Task<UploadSession> GetUploadSessionStatus(string sessionId)
        {
            return await _interfax.HttpClient.GetResourceAsync<UploadSession>($"{ResourceUri}/{sessionId}");
        }

        /// <summary>
        /// Create a document upload session.
        /// </summary>
        /// <param name="createOptions"></param>
        /// <returns>The id of the created session.</returns>
        public async Task<string> CreateUploadSession(UploadSessionOptions createOptions)
        {
            if (createOptions == null)
                throw new ArgumentException("createOptions");

            var response = await _interfax.HttpClient.PostAsync(ResourceUri, createOptions);
            return response.Headers.Location.Segments.Last();
        }

        /// <summary>
        /// Cancel a document upload session.
        /// </summary>
        /// <param name="createOptions"></param>
        /// <param name="sessionId">The identifier of the session to cancel.</param>
        /// <returns>The Uri of the created session.</returns>
        public async Task<string> CancelUploadSession(string sessionId)
        {
            return await _interfax.HttpClient.DeleteResourceAsync($"{ResourceUri}/{sessionId}");
        }
    }
}