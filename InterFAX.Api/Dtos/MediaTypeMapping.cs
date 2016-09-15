using Newtonsoft.Json;

namespace InterFAX.Api.Dtos
{
    internal class MediaTypeMapping
    {
        [JsonProperty(PropertyName = "MediaType")]
        internal string MediaType { get; set; }
        [JsonProperty(PropertyName = "FileType")]
        internal string FileType { get; set; }
    }
}