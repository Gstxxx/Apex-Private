using System;
using System.Collections.Generic;
using static Private_Apex.Translator.LanguagePayloads;

namespace Private_Apex.Translator
{
    public class Language
    {
        private int _langId { get; set; }
        private List<LanguagePackage> _translations;
        private List<LanguagePackage> _avaliableLangs;

        public Language()
        {
            _langId = 2;
            _translations = new List<LanguagePackage>();
            _avaliableLangs = new List<LanguagePackage>();

            _translations.AddRange(new List<LanguagePackage>()
            {
                { new("LanguageWarning", "Hello, please choose your language!") },
                { new("SuccessLangChange", "Language changed with successfully!") },
                { new("WelcomeMessage", "Welcome to Apex Loader") },
                { new("NotAuthenticated", "Press Enter to continue...") },
                { new("InvalidWriteProcMem", "Failed to WriteMemory... aborting.") },
                { new("InvalidhThread", "Unable to create thread... aborting.") },
                { new("SuccessInjection", "Injected with successfully!") },
                { new("InvalidProcess", "Unable to open process... aborting.") },
                { new("NumberNeeded", "Invalid selected language.") },
                { new("SuccessLogin", "Your have been logged with successfully" ) },
                { new("InitInjection", "Initializing Injection..." ) },
                { new("SuccessInjection", "Successfully Injection!" ) },
                { new("FailedInjection", "Failed to Inject!" ) },
                { new("PressAnyKeyInjection", "Press any key to intialize injection." ) },
                { new("PressAnyKey", "Press any key to continue." ) },
                { new("GameNotFound", "Please open the game before continuing..." ) },
            });
            _avaliableLangs.AddRange(new List<LanguagePackage>()
            {
                { new( "Português", "PT") },
                { new( "Español", "ES" ) },
                { new( "English", "EN" ) },
                { new( "Français", "FR" ) },
                { new( "Deutsche", "DE" ) },
                { new( "Italiano", "IT" ) },
                { new( "Latin", "LA" ) },
                { new( "Tiếng Việt", "VT" ) },
                { new( "Suomalainen", "FI" ) },
                { new( "Polskie", "PL" ) },
                { new( "Română", "RO" ) },
                { new( "русский", "RU" ) },
                { new( "Esperanto", "EO" ) },
                { new( "Türk", "TR" ) },
            });
        }

        public void OutputAvaliableLangs()
        {
            int index = 0;
            _avaliableLangs.ForEach(lang => Console.WriteLine($"[{index++}] {lang.ID}"));
        }

        public string GetTranslationFor(string text) => _langId switch
        {
            0 => YandexAPI.TranslationRequest(_translations.At(text), EN_PT),
            1 => YandexAPI.TranslationRequest(_translations.At(text), EN_ES),
            2 => _translations.At(text),
            3 => YandexAPI.TranslationRequest(_translations.At(text), EN_FR),
            4 => YandexAPI.TranslationRequest(_translations.At(text), EN_DE),
            5 => YandexAPI.TranslationRequest(_translations.At(text), EN_IT),
            6 => YandexAPI.TranslationRequest(_translations.At(text), EN_LA),
            7 => YandexAPI.TranslationRequest(_translations.At(text), EN_VI),
            8 => YandexAPI.TranslationRequest(_translations.At(text), EN_FI),
            9 => YandexAPI.TranslationRequest(_translations.At(text), EN_PL),
            10 => YandexAPI.TranslationRequest(_translations.At(text), EN_RO),
            11 => YandexAPI.TranslationRequest(_translations.At(text), EN_RU),
            12 => YandexAPI.TranslationRequest(_translations.At(text), EN_EO),
            13 => YandexAPI.TranslationRequest(_translations.At(text), EN_TR),
            _ => _translations.At(text),
        };

        public string GetRawTranslationFor(string text) => _langId switch
        {
            0 => YandexAPI.TranslationRequest(text, EN_PT),
            1 => YandexAPI.TranslationRequest(text, EN_ES),
            2 => text,
            3 => YandexAPI.TranslationRequest(text, EN_FR),
            4 => YandexAPI.TranslationRequest(text, EN_DE),
            5 => YandexAPI.TranslationRequest(text, EN_IT),
            6 => YandexAPI.TranslationRequest(text, EN_LA),
            7 => YandexAPI.TranslationRequest(text, EN_VI),
            8 => YandexAPI.TranslationRequest(text, EN_FI),
            9 => YandexAPI.TranslationRequest(text, EN_PL),
            10 => YandexAPI.TranslationRequest(text, EN_RO),
            11 => YandexAPI.TranslationRequest(text, EN_RU),
            12 => YandexAPI.TranslationRequest(text, EN_EO),
            13 => YandexAPI.TranslationRequest(text, EN_TR),
            _ => text,
        };

        public void SetLanguageID(int id) => _langId = id;
    }
}
