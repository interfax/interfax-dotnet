using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api.Dtos
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PageResolution
    {
        /// <summary>
        /// Standard page rendering quality.
        /// </summary>
        Standard,

        /// <summary>
        /// Documents rendered as fine may be more readable but take longer to transmit (and may therefore be more costly).
        /// </summary>
        Fine
    }
}