using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class TranslateDto : DtoBase
    {
        [JsonPropertyName("q")]
        public string TextToTranslate { get; set; } = "";
        /// <summary>
        /// The language of <see cref="TextToTranslate"/> or "auto" when the language should be detected automatically
        /// </summary>
        [JsonPropertyName("source")]
        public string SourceLanguageCode { get; set; } = "";

        /// <summary>
        /// The language into which the text (<see cref="TextToTranslate"/>) should be converted
        /// </summary>
        [JsonPropertyName("target")]
        public string TargetLanguageCode { get; set; } = "";
        /// <summary>
        /// The format of <see cref="TextToTranslate"/>
        /// </summary>
        [JsonPropertyName("format")]
        public TextFormat FormatOfTextToTranslate { get; set; }
        /// <summary>
        /// Defines how many translation option should be created.
        /// </summary>
        [JsonPropertyName("alternatives")]
        public int CountOfAlternativeTranslations { get; set; } = 0;
    }
}
