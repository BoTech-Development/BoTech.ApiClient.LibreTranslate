using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses.SettingsResponse
{
    public class ServerSettingsResult
    {
        /// <summary>
        /// Whether the API key database is enabled.
        /// </summary>
        [JsonPropertyName("status")]
        public bool AreApiKeysEnabledInDatabase { get; set; }
        /// <summary>
        /// User must provide an key to get access.
        /// </summary>
        public bool DoesApiRequireApiKeys { get; set; }
        /// <summary>
        /// -1 for no limit or the maximum number of characters that can be translated in a request
        /// </summary>
        public int LimitOfCharsToTranslate { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public bool DoesApiSupportFileTranslation { get; set; }
        /// <summary>
        /// Frontend translation timeout
        /// </summary>
        public int FrontendTimeout { get; set; }
        /// <summary>
        /// Is the suggest endpoint usable?
        /// </summary>
        public bool AreTranslationSuggestionsEnabled { get; set; }
        /// <summary>
        /// Which file type are supported for translation
        /// </summary>
        public List<string> SupportedFilesFormat { get; set; } = new List<string>();
        /// <summary>
        /// The default settings of the ui.
        /// </summary>
        public FrontendLanguageSettingResult FrontendLanguageSettings { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
