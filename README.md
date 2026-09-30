![BoTech Logo](https://assets.botech.dev/Logos/BoTechLogoCompleteWithSlogan.png)
# BoTech.ApiClient.LibreTranslate
## How to use?
### 1. Install the package:

````shell
dotnet add package BoTech.ApiClient.LibreTranslate
````

### 2. Init the Client:
+ self hosted or using official server:
+ Here is the code for a self hosted server:
````csharp
TranslatorClient _translatorClient = TranslatorClient.CreateClientUsingSelfHostingServer("https://translate.botech.dev");
````
### 3. Request any endpoint:
#### Supported languages:
````csharp
List<SupportedLanguageResult>? supportedLanguages = await _translatorClient.GetSupportedLanguages();
````
#### Detect language:
````csharp
List<DetectedLanguageResult>? detectedLanguages = await _translatorClient.DetectLanguage(new DetectLanguageDto()
            {
                TextToDetectTheLanguageFor = "Hallo welt. Dies ist ein text in deustcher Sprache."
            });
````
#### Translate any string:
````csharp
TranslationResult? result = await _translatorClient.Translate(new TranslateDto
{
    TextToTranslate = "Hallo welt, wie geht es euch?",
    SourceLanguageCode = "auto", // or the language code
    TargetLanguageCode = "en",
    FormatOfTextToTranslate = TextFormat.text,
    CountOfAlternativeTranslations = 3
});
````
[![Sponsor BoTech-Development](https://assets.botech.dev/Logos/SponsorBoTechButton.png)](https://github.com/sponsors/BoTech-Development)