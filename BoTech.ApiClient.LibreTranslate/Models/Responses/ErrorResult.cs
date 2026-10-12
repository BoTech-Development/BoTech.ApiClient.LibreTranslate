using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses
{
    public class ErrorResult
    {
        /// <summary>
        /// The error message returned by the server.
        /// </summary>
        [JsonPropertyName("error")]
        public string ErrorText { get; init; } = "";
    }
}
