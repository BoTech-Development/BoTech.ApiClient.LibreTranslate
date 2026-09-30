using System.Net;
using BoTech.HttpClientHelper;
using BoTech.ApiClient.LibreTranslate.Models.Requests;
using BoTech.ApiClient.LibreTranslate.Models.Responses;
using BoTech.HttpClientHelper.Models;

namespace BoTech.ApiClient.LibreTranslate
{
    /// <summary>
    /// This api client is used to communicate with the LibreTranslate API.
    /// </summary>
    public class TranslatorClient
    {
        /// <summary>
        /// the api key when the user defined one.
        /// </summary>
        private string _apiKey;
        /// <summary>
        /// A helper class to communicate with any api's
        /// </summary>
        private HttpRequestHelper _httpClient;
        /// <summary>
        /// Gets the current error result if the last request failed.
        /// </summary>
        public ErrorResult? CurrentErrorResult { get; private set; }
        private TranslatorClient(string serverBaseUrl, string apiKey)
        {
            _httpClient = new HttpRequestHelper(serverBaseUrl);
            _apiKey = apiKey;
        }

        /// <summary>
        /// Creates an instance of the <see cref="TranslatorClient"/> configured to communicate with the official LibreTranslate server (https://libretranslate.com).
        /// </summary>
        /// <param name="apiKey">The API key used to authenticate requests to the official LibreTranslate server.</param>
        /// <returns>A new instance of <see cref="TranslatorClient"/> configured with the official server URL and the provided API key.</returns>
        public static TranslatorClient CreateClientUsingLibreTranslateServer(string apiKey)
        {
            return new TranslatorClient("https://libretranslate.com", apiKey);
        }
        /// <summary>
        /// Creates an instance of the <see cref="TranslatorClient"/> configured to communicate with a self-hosting LibreTranslate server.
        /// </summary>
        /// <param name="serverUrl">The base URL of the self-hosted LibreTranslate server.</param>
        /// <returns>A new instance of <see cref="TranslatorClient"/> configured with the specified server URL.</returns>
        public static TranslatorClient CreateClientUsingSelfHostingServer(string serverUrl)
        {
            return CreateClientUsingSelfHostingServer(serverUrl, "");   
        }

        /// <summary>
        /// Creates an instance of the <see cref="TranslatorClient"/> configured to communicate with a self-hosting LibreTranslate server.
        /// </summary>
        /// <param name="serverUrl">The base URL of the self-hosted LibreTranslate server.</param>
        /// <param name="apiKey">The API key used to authenticate requests to the self-hosted LibreTranslate server.</param>
        /// <returns>A new instance of <see cref="TranslatorClient"/> configured with the specified server URL and API key.</returns>
        public static TranslatorClient CreateClientUsingSelfHostingServer(string serverUrl, string apiKey)
        {
            return new TranslatorClient(serverUrl, apiKey);
        }

        /// <summary>
        /// Retrieves the list of supported languages from the LibreTranslate service.
        /// </summary>
        /// <returns>A list of <see cref="SupportedLanguageResult"/> objects representing the languages supported by the service, or null if an error occurs during the request.</returns>
        public async Task<List<SupportedLanguageResult>?> GetSupportedLanguages()
        {
            RequestResult result = await _httpClient.HttpGetJsonObject("/languages", JsonDtoSelectionOptions.CreateForSingleStatusOkDtoType(typeof(List<SupportedLanguageResult>)));
            if(result.ParsedData is List<SupportedLanguageResult>  supportedLanguages)
                return supportedLanguages;
            return null;
        }

        /// <summary>
        /// Translates text from a source language to a target language using the LibreTranslate API.
        /// </summary>
        /// <param name="dto">An instance of <see cref="TranslateDto"/> that contains the details of the text to be translated, including the source language, target language, and text format.</param>
        /// <returns>An instance of <see cref="TranslationResult"/> containing the translated text, or null if the translation fails. If an error occurs, <see cref="TranslatorClient.CurrentErrorResult"/> contains details of the error.</returns>
        public async Task<TranslationResult?> Translate(TranslateDto dto)
        {
            dto.ApiKey = _apiKey;
            RequestResult result = await _httpClient.HttpPostJsonAndGetJson("/translate", dto, new JsonDtoSelectionOptions()
            {
                DtoTypeForEachHttpStatusCode = new Dictionary<HttpStatusCode, Type>()
                {
                    { HttpStatusCode.OK, typeof(TranslationResult) },
                    { HttpStatusCode.BadRequest, typeof(ErrorResult) },
                    { HttpStatusCode.Forbidden, typeof(ErrorResult) },
                    { HttpStatusCode.TooManyRequests, typeof(ErrorResult) },
                    { HttpStatusCode.InternalServerError, typeof(ErrorResult) }
                }
            });
            if(result.IsSuccess() && result.ParsedData != null && result.ParsedData is TranslationResult translationResult)
                return translationResult;
            if(result.ParsedData is ErrorResult errorResult)
                CurrentErrorResult = errorResult;
            return null;
        }
        /// <summary>
        /// Detects the language of the provided text using the LibreTranslate API.
        /// </summary>
        /// <param name="dto">The data transfer object containing the text for which the language detection is requested.</param>
        /// <returns>A list of <see cref="DetectedLanguageResult"/> representing the detected languages, or null if an error occurred.</returns>
        public async Task<List<DetectedLanguageResult>?> DetectLanguage(DetectLanguageDto dto)
        {
            dto.ApiKey = _apiKey;
            RequestResult result = await _httpClient.HttpPostJsonAndGetJson("/detect", dto, new JsonDtoSelectionOptions()
            {
                DtoTypeForEachHttpStatusCode = new Dictionary<HttpStatusCode, Type>()
                {
                    { HttpStatusCode.OK, typeof(List<DetectedLanguageResult>) },
                    { HttpStatusCode.BadRequest, typeof(ErrorResult) },
                    { HttpStatusCode.Forbidden, typeof(ErrorResult) },
                    { HttpStatusCode.TooManyRequests, typeof(ErrorResult) },
                    { HttpStatusCode.InternalServerError, typeof(ErrorResult) }
                }
            });
            if(result.IsSuccess() && result.ParsedData != null && result.ParsedData is List<DetectedLanguageResult> detectionResults)
                return detectionResults;
            if(result.ParsedData is ErrorResult errorResult)
                CurrentErrorResult = errorResult;
            return null;
        }
    }
}
