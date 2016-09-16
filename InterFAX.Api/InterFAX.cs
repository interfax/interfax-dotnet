using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InterFAX.Api.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api
{
    public class InterFAX
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
        public InterFAX(string username, string password, HttpMessageHandler messageHandler = null)
        {
            Initialise(username, password, messageHandler);
        }

        /// <summary>
        /// Initialises the client from the App.config file.
        /// </summary>
        public InterFAX(HttpMessageHandler messageHandler = null)
        {
            var username = ConfigurationManager.AppSettings[UsernameConfigKey];
            if (string.IsNullOrEmpty(username))
                username = Environment.GetEnvironmentVariable(UsernameConfigKey);

            var password = ConfigurationManager.AppSettings[PasswordConfigKey];
            if (string.IsNullOrEmpty(password))
                password = Environment.GetEnvironmentVariable(PasswordConfigKey);

            Initialise(username, password, messageHandler);
        }

        private void Initialise(string username, string password, HttpMessageHandler messageHandler = null)
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
            HttpClient.BaseAddress = new Uri("https://rest.interfax.net/");
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
