using System;
using System.Collections.Generic;
using System.Text;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses.SettingsResponse
{
    public class LanguageResult
    {
        /// <summary>
        /// The Language code (2 letter)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The natural language name of the language
        /// </summary>
        public string Name { get; set; }
    }
}
