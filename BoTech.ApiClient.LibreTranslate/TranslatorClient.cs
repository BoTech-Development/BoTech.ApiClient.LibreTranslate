using System.Net;
using BoTech.HttpClientHelper;
using BoTech.ApiClient.LibreTranslate.Models.Requests;
using BoTech.ApiClient.LibreTranslate.Models.Responses;
using BoTech.HttpClientHelper.Models;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using BoTech.ApiClient.LibreTranslate.Models.Responses.SettingsResponse;

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
        /// This method translates a file using an stream as input.
        /// </summary>
        /// <param name="dto">The file stream which contains the text to translate.</param>
        /// <returns>The url which points to the downloadable file. You can use the following method to download the file: <see cref="DownloadTranslatedFile(TranslatedFileResult)"/></returns>
        public async Task<TranslatedFileResult?> Translate(TranslateFileDto dto)
        {
            dto.ApiKey = _apiKey;
            HttpContent sourceLanguageCodeContent = new StringContent(dto.SourceLanguageCode);
            HttpContent targetLanguageCodeContent = new StringContent(dto.TargetLanguageCode);
            HttpContent fileStreamContent = new StreamContent(dto.FileStreamToSend);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(sourceLanguageCodeContent, "source");
                formData.Add(targetLanguageCodeContent, "target");
                formData.Add(fileStreamContent, "file", dto.FileName);

                RequestResult result = await _httpClient.HttpPostContentAndGetJson("/translate_file", formData, new JsonDtoSelectionOptions()
                {
                    DtoTypeForEachHttpStatusCode = new Dictionary<HttpStatusCode, Type>()
                    {
                        { HttpStatusCode.OK, typeof(TranslatedFileResult) },
                        { HttpStatusCode.BadRequest, typeof(ErrorResult) },
                        { HttpStatusCode.Forbidden, typeof(ErrorResult) },
                        { HttpStatusCode.TooManyRequests, typeof(ErrorResult) },
                        { HttpStatusCode.InternalServerError, typeof(ErrorResult) }
                    }
                });
                if (result.IsSuccess() && result.ParsedData != null && result.ParsedData is TranslatedFileResult translationResult)
                    return translationResult;
                if (result.ParsedData is ErrorResult errorResult)
                    CurrentErrorResult = errorResult;
                return null;
            }
        }
        /// <summary>
        /// This method downloads the translated file from the server.
        /// </summary>
        /// <param name="resultUrl">The url which point to the file.</param>
        /// <returns>The file stream or null when error</returns>
        public async Task<Stream?> DownloadTranslatedFile(TranslatedFileResult resultUrl)
        {
            RequestResult result = await _httpClient.HttpGetFileStream(resultUrl.DownloadUrlOfTranslatedFile);
            if (result.IsSuccess() && result.ParsedData != null && result.ParsedData is Stream fileStream)
                return fileStream;
            if (result.ParsedData is ErrorResult errorResult)
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
        /// <summary>
        /// This method stores a better translation when this option is enabled on the server.
        /// </summary>
        /// <param name="dto">The better translation.</param>
        /// <returns>true when the new translation is stored. Please use the <see cref="CurrentErrorResult"/> property when an error occur.</returns>
        public async Task<bool> TryToSuggestBetterTranslation(SuggestTranslationDto dto)
        {
            RequestResult result = await _httpClient.HttpPostJsonAndGetJson("/suggest", dto, new JsonDtoSelectionOptions()
            {
                DtoTypeForEachHttpStatusCode = new Dictionary<HttpStatusCode, Type>()
                {
                    { HttpStatusCode.OK, typeof(SuccessResult) },
                    { HttpStatusCode.Forbidden, typeof(ErrorResult) }
                }
            });
            if (result.IsSuccess() && result.ParsedData != null && result.ParsedData is SuccessResult successResult)
                return successResult.Success;
            if (result.ParsedData is ErrorResult errorResult)
                CurrentErrorResult = errorResult;
            return false;
        }
        /// <summary>
        /// This method returns the status of the server.
        /// </summary>
        /// <returns>Returns the text ok when the server is working.</returns>
        public async Task<StatusResult?> GetServerStatus()
        {
            RequestResult result = await _httpClient.HttpGetJsonObject("/health", JsonDtoSelectionOptions.CreateForSingleStatusOkDtoType(typeof(StatusResult)));
            if (result.IsSuccess() && result.ParsedData != null && result.ParsedData is StatusResult status)
                return status;
            return null;
        }
        /// <summary>
        /// fetches the server settings. 
        /// </summary>
        /// <returns>Model which contains info about the frontend settings and backend settings.</returns>
        public async Task<ServerSettingsResult?> GetServerSettings()
        {
            RequestResult result = await _httpClient.HttpGetJsonObject("/frontend/settings", JsonDtoSelectionOptions.CreateForSingleStatusOkDtoType(typeof(ServerSettingsResult)));
            if (result.IsSuccess() && result.ParsedData != null && result.ParsedData is ServerSettingsResult settings)
                return settings;
            return null;
        }
    }
}
