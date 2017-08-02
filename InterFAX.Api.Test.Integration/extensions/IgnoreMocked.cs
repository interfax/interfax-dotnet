using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Interfaces;

namespace InterFAX.Api.Test.Integration.extensions
{
	internal class IgnoreMocked : Attribute, ITestAction
	{
		public ActionTargets Targets { get; set; }

		public void AfterTest(ITest test)
		{

		}

		public void BeforeTest(ITest test)
		{
			if (TestingConfig.scotchMode == Scotch.ScotchMode.Replaying)
			{
				Assert.Ignore("Test cannot be mocked, ignored in API replay mode");
			}
		}
	}
}
