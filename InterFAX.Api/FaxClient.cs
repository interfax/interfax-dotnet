using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InterFAX.Api.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api
{
    public class FaxClient
    {
        public const string UsernameConfigKey = "INTERFAX_USERNAME";
        public const string PasswordConfigKey = "INTERFAX_PASSWORD";

        public Inbound Inbound { get; private set; }
        public Outbound Outbound { get; private set; }
        public Account Account { get; private set; }
        public Documents Documents { get; private set; }

        internal HttpClient HttpClient { get; private set; }

        public enum ApiRoot
        {
            InterFAX_Default,
            InterFAX_PCI
        }

        /// <summary>
        /// Initialises the client with the given <paramref name="username"/> and <paramref name="password"/>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="messageHandler"></param>
        /// <param name="ApiRoot">(Optional) Alternative API Root</param>
        public FaxClient(string username, string password, HttpMessageHandler messageHandler = null, ApiRoot apiRoot=ApiRoot.InterFAX_Default)
        {
            Initialise(username, password, apiRoot: apiRoot, messageHandler: messageHandler);
        }

        /// <summary>
        /// Initialises the client from Environment Variables.
        /// </summary>
        public FaxClient(HttpMessageHandler messageHandler = null, ApiRoot apiRoot = ApiRoot.InterFAX_Default)
        {
            var username = Environment.GetEnvironmentVariable(UsernameConfigKey);
            var password = Environment.GetEnvironmentVariable(PasswordConfigKey);
            Initialise(username, password, apiRoot: apiRoot, messageHandler: messageHandler);
        }

        /// <summary>
        /// Initialise the client with a custom httpClient
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="httpClient">Custom httpClient instance</param>
        /// <param name="ApiRoot">(Optional) Alternative API Root</param>
        public FaxClient(string username, string password, HttpClient httpClient, ApiRoot apiRoot = ApiRoot.InterFAX_Default)
		{
			Initialise(username, password, apiRoot:apiRoot, messageHandler:null, httpClient:httpClient);
		}

		private void Initialise(string username, string password, ApiRoot apiRoot, HttpMessageHandler messageHandler = null, HttpClient httpClient = null)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException($"{UsernameConfigKey} has not been set.");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"{PasswordConfigKey} has not been set.");

            Account = new Account(this);
            Inbound = new Inbound(this);
            Outbound = new Outbound(this);
            Documents = new Documents(this);

            HttpClient = messageHandler == null ? new HttpClient() : new HttpClient(messageHandler);
			HttpClient = httpClient != null ? httpClient : HttpClient;

            var root = "";
            switch (apiRoot)
            {
                case ApiRoot.InterFAX_PCI:
                    root = "https://rest-sl.interfax.net";
                    break;
                case ApiRoot.InterFAX_Default:
                    root = "https://rest.interfax.net";
                    break;
            }

            HttpClient.BaseAddress = new Uri(root);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("Authorization",
                new List<string> {$"Basic {Utils.Base64Encode($"{username}:{password}")}"});

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            };
        }
    }
}
