using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses
{
    public class TranslatedFileResult
    {
        /// <summary>
        /// You can us this url to download the translated file.
        /// Furtheremore the <see cref="TranslatorClient"/> implements the method 
        /// </summary>
        [JsonPropertyName("translatedFileUrl")]
        public string DownloadUrlOfTranslatedFile { get; init; } = "";
    }
}
