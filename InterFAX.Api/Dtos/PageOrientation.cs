using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api.Dtos
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PageOrientation
    {
        Portrait,
        Landscape
    }
}
