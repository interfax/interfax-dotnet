using System.Collections.Generic;

namespace InterFAX.Api
{
    internal interface IOptions
    {
        Dictionary<string, string> ToDictionary();
    }
}