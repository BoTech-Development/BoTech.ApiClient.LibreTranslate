using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using BoTech.ApiClient.LibreTranslate.Models.Requests;

namespace BoTech.ApiClient.LibreTranslate.Models.Requests
{
    public class DetectLanguageDto : DtoBase
    {
        /// <summary>
        /// The text to run the language detection on. This is the text that will be analyzed to determine its language.
        /// </summary>
        [JsonPropertyName("q")]
        public string TextToDetectTheLanguageFor { get; set; } = "";
    }
}
