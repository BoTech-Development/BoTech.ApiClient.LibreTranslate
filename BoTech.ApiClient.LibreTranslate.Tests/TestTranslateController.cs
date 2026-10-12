using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BoTech.ApiClient.LibreTranslate.Models.Requests;
using BoTech.ApiClient.LibreTranslate.Models.Responses;
using BoTech.ApiClient.LibreTranslate.Models.Responses.SettingsResponse;

namespace BoTech.ApiClient.LibreTranslate.Tests
{
    [TestClass]
    public class TestTranslateController
    {
        private static TranslatorClient _translatorClient;
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _translatorClient = TranslatorClient.CreateClientUsingSelfHostingServer("https://translate.botech.dev");
            File.WriteAllText("toTranslate.txt", "Dies ist ein Text, der in die Englische Sprache übersetzt werden soll.\n Dabei soll der Server translate.botech.dev genutzt werden.");
        }
        
        [TestMethod]
        public async Task TestGetSupportedLanguages()
        {
            List<SupportedLanguageResult>? supportedLanguages = await _translatorClient.GetSupportedLanguages();
            if(supportedLanguages == null)
                Assert.Fail($"Supported languages result is null. Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
            
            //print:
            foreach (SupportedLanguageResult supportedLanguage in supportedLanguages)
            {
                string printResult = $"Language code: {supportedLanguage.LanguageCode}; Name: {supportedLanguage.LanguageName} \n";
                supportedLanguage.TargetLanguages.ForEach((targetLanguage) => printResult += $"   Target language: {targetLanguage}; \n");
                Console.WriteLine(printResult);
            }
        }
        
        [TestMethod]
        public async Task TestDetectLanguage()
        {
            List<DetectedLanguageResult>? detectedLanguages = await _translatorClient.DetectLanguage(new DetectLanguageDto()
            {
                TextToDetectTheLanguageFor = "Hallo welt. Dies ist ein text in deustcher Sprache."
            });
            if(detectedLanguages == null)
                Assert.Fail($"Supported languages result is null. Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
            
            // print:
            foreach (DetectedLanguageResult detectedLanguage in detectedLanguages)
            {
                Console.WriteLine($"Detected language: {detectedLanguage.LanguageCode}; Confidence: {detectedLanguage.Confidence}");
            }
        }

        [TestMethod]
        public async Task TestTranslateEndpoint()
        {
            TranslationResult? result = await _translatorClient.Translate(new TranslateDto
            {
                TextToTranslate = "Hallo welt, wie geht es euch?",
                SourceLanguageCode = "auto",
                TargetLanguageCode = "en",
                FormatOfTextToTranslate = TextFormat.text,
                CountOfAlternativeTranslations = 3
            });
            if(result == null)
                Assert.Fail($"Translation result is null. Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
            Console.WriteLine($"Translation result: {result.TranslatedText}");
        }
        [TestMethod]
        public async Task TestTranslateFileEndpoint()
        {
            Stream toTranslateFileStream = File.OpenRead("toTranslate.txt");
            TranslatedFileResult? downloadUrlDto = await _translatorClient.Translate(new TranslateFileDto()
            {
                FileStreamToSend = toTranslateFileStream,
                SourceLanguageCode = "auto",
                TargetLanguageCode = "en",
                FileName = "toTranslate.txt"
            });
            if (downloadUrlDto == null)
                Assert.Fail($"Download url is null. Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
            Stream? translatedFileStream = await _translatorClient.DownloadTranslatedFile(downloadUrlDto);
            if (translatedFileStream == null)
                Assert.Fail($"Translation result stream is null. Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
            Console.WriteLine($"Translation result: {new StreamReader(translatedFileStream).ReadToEnd()}");
        }
        [TestMethod]
        public async Task TestTryToSuggestBetterTranslation()
        {
            bool success = await _translatorClient.TryToSuggestBetterTranslation(new SuggestTranslationDto()
            {
                OrginalText = "Ein test Text",
                SuggestedTranslation = "A sample text",
                SourceLanguageCode = "de",
                TargetLanguageCode= "en"
            });
            if(!success)
                Assert.Fail($"Error by posting a better translation:  {_translatorClient.CurrentErrorResult?.ErrorText}");
            if(_translatorClient.CurrentErrorResult != null)
                Assert.Fail($"Error: {_translatorClient.CurrentErrorResult?.ErrorText}");
        }   
        [TestMethod]
        public async Task TestGetServerStatus()
        {
            StatusResult? result = await _translatorClient.GetServerStatus();
            if (result == null && _translatorClient.CurrentErrorResult != null)
                Assert.Fail($"Error by getting response: {_translatorClient.CurrentErrorResult.ErrorText}");
            Console.WriteLine($"Success, Status is: {result.Status}");
        }
        [TestMethod]
        public async Task TestGetServerSettings()
        {
            ServerSettingsResult? result = await _translatorClient.GetServerSettings();
            if (result == null && _translatorClient.CurrentErrorResult != null)
                Assert.Fail($"Error by getting response: {_translatorClient.CurrentErrorResult.ErrorText}");
            if(result == null)
                Assert.Fail("Error result is null!");
            Console.WriteLine($"Success, Status is: {result.ToString()}");
        }
    }
}
