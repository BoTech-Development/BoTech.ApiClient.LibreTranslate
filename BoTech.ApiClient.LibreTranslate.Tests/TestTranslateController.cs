using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BoTech.ApiClient.LibreTranslate.Models.Requests;
using BoTech.ApiClient.LibreTranslate.Models.Responses;

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
    }
}
