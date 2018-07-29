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

        /// <summary>
        /// Initialises the client with the given <paramref name="username"/> and <paramref name="password"/>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="messageHandler"></param>
        public FaxClient(string username, string password, HttpMessageHandler messageHandler = null)
        {
            Initialise(username, password, messageHandler);
        }

        /// <summary>
        /// Initialises the client from the App.config file.
        /// </summary>
        public FaxClient(HttpMessageHandler messageHandler = null)
        {
            
            var username = Environment.GetEnvironmentVariable(UsernameConfigKey);
            var password = Environment.GetEnvironmentVariable(PasswordConfigKey);
            Initialise(username, password, messageHandler);
        }

		/// <summary>
		/// Initialise the client with a custom httpClient
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="httpClient">Custom httpClient instance</param>
		public FaxClient(string username, string password, HttpClient httpClient)
		{
			Initialise(username, password, null, httpClient);
		}

		private void Initialise(string username, string password, HttpMessageHandler messageHandler = null, HttpClient httpClient = null)
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

            HttpClient.BaseAddress = new Uri("https://rest.interfax.net/");
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
