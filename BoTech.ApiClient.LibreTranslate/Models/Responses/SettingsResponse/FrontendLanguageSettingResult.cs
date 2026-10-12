using System;
using System.Collections.Generic;
using System.Text;

namespace BoTech.ApiClient.LibreTranslate.Models.Responses.SettingsResponse
{
    public class FrontendLanguageSettingResult
    {
        /// <summary>
        /// Default source language in the ui
        /// </summary>
        public LanguageResult Source {  get; set; }
        /// <summary>
        /// default target language in the ui
        /// </summary>
        public LanguageResult Target { get; set; }
    }
}
