using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterFAX.Api.Test.Integration
{
	/// <summary>
	/// Configuration for scotch API replay testing
	/// </summary>
	internal static class TestingConfig
	{
		// Modify to disable API replay, and test against live API. Requires valid API Credentials
		// See https://www.interfax.net/en/dev/register

		internal const String username = "X";
		internal const String password = "X";

		// Do not modify unless rebuilding cassette, or testing with replay disabled.
		// Values tied to request URLs

		internal const String faxNumber = "+99999990";
		internal const int inboundFaxID = 99;
		internal const int outboundFaxID = 99;
		internal const String scotchCassettePath = "/cassettes/integration.json";
	}
}
