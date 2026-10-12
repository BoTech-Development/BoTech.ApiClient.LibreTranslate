using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class TranslateFileDto : DtoBase
    {
        /// <summary>
        /// The filename, 
        /// </summary>
        public required string FileName { get; init; }
        /// <summary>
        /// The File stream of the file to translate
        /// </summary>
        [JsonPropertyName("file")]
        public required Stream FileStreamToSend { get; init; }
        /// <summary>
        /// The language of <see cref="TextToTranslate"/> or "auto" when the language should be detected automatically
        /// </summary>
        [JsonPropertyName("source")]
        public required string SourceLanguageCode { get; init; } = "";

        /// <summary>
        /// The language into which the text (<see cref="TextToTranslate"/>) should be converted
        /// </summary>
        [JsonPropertyName("target")]
        public required string TargetLanguageCode { get; init; } = "";
    }
}
