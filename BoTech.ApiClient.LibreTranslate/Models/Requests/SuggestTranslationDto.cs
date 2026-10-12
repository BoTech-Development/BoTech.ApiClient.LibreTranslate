using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class SuggestTranslationDto : DtoBase
    {
        /// <summary>
        /// The orginal not translated text.
        /// </summary>
        [JsonPropertyName("q")]
        public required string OrginalText { get; init; }
        /// <summary>
        /// The better translation.
        /// </summary>
        [JsonPropertyName("s")]
        public required string SuggestedTranslation {  get; init; }
        /// <summary>
        /// The language of <see cref="OrginalText"/>.
        /// </summary>
        [JsonPropertyName("source")]
        public required string SourceLanguageCode { get; init; }
        /// <summary>
        /// The language of <see cref="OrginalText"/>.
        /// </summary>
        [JsonPropertyName("target")]
        public required string TargetLanguageCode { get; init; }
    }
}
