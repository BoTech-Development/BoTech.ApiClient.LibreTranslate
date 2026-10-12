using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class TranslateDto : DtoBase
    {
        [JsonPropertyName("q")]
        public required string TextToTranslate { get; init; }
        /// <summary>
        /// The language of <see cref="TextToTranslate"/> or "auto" when the language should be detected automatically
        /// </summary>
        [JsonPropertyName("source")]
        public required string SourceLanguageCode { get; init; } = "auto";
        /// <summary>
        /// The language into which the text (<see cref="TextToTranslate"/>) should be converted
        /// </summary>
        [JsonPropertyName("target")]
        public required string TargetLanguageCode { get; init; }
        /// <summary>
        /// The format of <see cref="TextToTranslate"/>
        /// </summary>
        [JsonPropertyName("format")]
        public required TextFormat FormatOfTextToTranslate { get; init; }
        /// <summary>
        /// Defines how many translation option should be created.
        /// </summary>
        [JsonPropertyName("alternatives")]
        public required int CountOfAlternativeTranslations { get; init; }
    }
}
