using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class DtoBase
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set;  }
    }
}
