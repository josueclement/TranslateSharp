using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using TranslateSharp;
using TranslateSharp.Abstractions;

namespace ConsoleApp;

internal static class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // CREATE TABLE Translations (
            //     Key TEXT NOT NULL,
            //     Language TEXT NOT NULL,
            //     Text TEXT NOT NULL,
            //     PRIMARY KEY (Key, Language)
            // );
            DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            var repository = new DatabaseTranslationRepository(factory, @"Data Source=C:\Temp\translations.db");

            // var res = await repository.AddTranslationAsync(new Translation("MyKey2", "en", "This is my key"));
            var translation = await repository.GetTranslationAsync(key: "MyKey", language: "en");

            var translations = await repository.GetAllTranslationsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}