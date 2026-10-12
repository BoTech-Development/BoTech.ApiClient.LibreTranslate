using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class DtoBase
    {
        /// <summary>
        /// This porperty will be set by the <see cref="TranslatorClient"/>
        /// </summary>
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; }
    }
}
