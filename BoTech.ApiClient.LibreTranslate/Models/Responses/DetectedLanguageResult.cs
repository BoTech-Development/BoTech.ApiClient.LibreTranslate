using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses
{
    public class DetectedLanguageResult
    {
        /// <summary>
        /// describes how sure the server is with the translation.
        /// </summary>
        [JsonPropertyName("confidence")]
        public double Confidence { get; init; }

        /// <summary>
        /// The code of the detected language.
        /// </summary>
        [JsonPropertyName("language")]
        public string LanguageCode { get; init; } = "";
    }
}
