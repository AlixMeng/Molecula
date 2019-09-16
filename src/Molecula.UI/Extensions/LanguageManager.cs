using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Molecula.UI.Extensions
{
    public static class LanguageManager
    {
        public const string DefaultLanguage = "English";
        private const string LanguageSubfolder = "Languages";
        private const string LanguageExtension = ".data";
        private static readonly Dictionary<string, string> DefaultLanguageData;
        private static readonly ResourceDictionary LanguageResources;

        static LanguageManager()
        {
            LanguageResources = new ResourceDictionary();
            DefaultLanguageData = LoadLanguage(DefaultLanguage);
            ApplyLanguage(DefaultLanguageData);
        }

        public static readonly DependencyProperty CurrentLanguageProperty =
            DependencyProperty.RegisterAttached("CurrentLanguage", typeof(string), typeof(LanguageManager),
                new PropertyMetadata(DefaultLanguage, OnCurrentLanguageChanged));

        public static string GetCurrentLanguage(FrameworkElement element) =>
            (string) element.GetValue(CurrentLanguageProperty);

        public static void SetCurrentLanguage(FrameworkElement element, string value) =>
            element.SetValue(CurrentLanguageProperty, value);

        private static void OnCurrentLanguageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(args.NewValue is string language)) return;
            var data = LoadLanguage(language);
            ApplyLanguage(data);
        }

        public static IEnumerable<string> AvailableLanguages => GetLanguages();

        private static Dictionary<string, string> LoadLanguage(string language)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LanguageSubfolder,
                $"{language}{LanguageExtension}");
            var lines = File.ReadAllLines(filePath);
            var data = lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Where(line => !line.TrimStart().StartsWith("#"))
                .ToDictionary(line => line.Substring(0, line.IndexOf('=')).Trim(),
                    line => line.Substring(line.IndexOf('=') + 1));
            return data;
        }

        private static void ApplyLanguage(Dictionary<string, string> data)
        {
            Application.Current.Resources.MergedDictionaries.Remove(LanguageResources);
            LanguageResources.Clear();
            foreach (var entry in DefaultLanguageData)
                LanguageResources[entry.Key] =
                    data.TryGetValue(entry.Key, out var translation) ? translation : entry.Value;
            Application.Current.Resources.MergedDictionaries.Add(LanguageResources);
        }

        private static IEnumerable<string> GetLanguages()
            => Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LanguageSubfolder),
                    $"*{LanguageExtension}")
                .Select(Path.GetFileNameWithoutExtension);
    }

}
