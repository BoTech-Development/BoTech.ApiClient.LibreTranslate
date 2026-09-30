using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace BoTech.ApiClient.LibreTranslate.Models.Responses
{
    public class TranslationResult
    {
        /// <summary>
        /// when requested, the collection contains other alternative translations.
        /// </summary>
        [JsonPropertyName("alternatives")]
        public List<string> AlternativeTranslations { get; set; } = new List<string>();
        /// <summary>
        /// When necessary the server tries to detect the language of the give string.
        /// This object contains the language that might be the language of the given text to translate.
        /// </summary>
        [JsonPropertyName("detectedLanguage")]
        public DetectedLanguageResult DetectedLanguage { get; set; } = new DetectedLanguageResult();
        /// <summary>
        /// The translation of the given text 
        /// </summary>
        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; } = "";
    }
}
