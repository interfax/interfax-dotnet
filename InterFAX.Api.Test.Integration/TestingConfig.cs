using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scotch;

namespace InterFAX.Api.Test.Integration
{
	/// <summary>
	/// Configuration for scotch API replay testing
	/// </summary>
	internal static class TestingConfig
	{
		// Modify to disable API replay, and test against live API. Requires valid API Credentials
		// See https://www.interfax.net/en/dev/register

		internal const String username = "NA";
		internal const String password = "NA";
		internal const ScotchMode scotchMode = ScotchMode.Replaying; // ScotchMode.None

		// Do not modify unless rebuilding cassette, or testing with replay disabled.
		// Values tied to request URLs

		internal const String faxNumber = "+449999999999";
		internal const int inboundFaxID = 99;
		internal const int outboundFaxID = 99;
		internal const String scotchCassettePath = "/cassettes/integration.json";
	}
}
