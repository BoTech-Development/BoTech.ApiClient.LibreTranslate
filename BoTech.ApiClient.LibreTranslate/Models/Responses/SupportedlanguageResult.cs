using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses
{
    public class SupportedLanguageResult
    {
        /// <summary>
        /// The two character language code.
        /// </summary>
        [JsonPropertyName("code")]
        public string LanguageCode { get; init; } = "";
        /// <summary>
        /// The name of the source language.
        /// </summary>
        [JsonPropertyName("name")]
        public string LanguageName { get; init; } = "";
        /// <summary>
        /// This list contains all language codes into which the given language (<see cref="LanguageCode"/> or <see cref="LanguageName"/>) can be translated.
        /// </summary>
        [JsonPropertyName("targets")]
        public List<string> TargetLanguages { get; init; } = new List<string>();
    }
}
