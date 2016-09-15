using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InterFAX.Api.Dtos
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PageRendering
    {
        /// <summary>
        /// Better for greyscale text and images. 
        /// </summary>
        GreyScale,

        /// <summary>
        /// Recommended for textual, black & white documents.
        /// </summary>
        BlackAndWhite
    }
}
