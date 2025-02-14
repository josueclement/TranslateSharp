using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TranslateSharp;
using TranslateSharp.Abstractions;

namespace ConsoleApp;

internal static class Program
{
    private static IServiceProvider? _serviceProvider;
    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceProvider ServiceProvider =>
        _serviceProvider ?? throw new InvalidOperationException("ServiceProvider is not configured");
    
    static void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITranslationRepository, DatabaseTranslationRepository>();
        services.AddSingleton<ITranslationRepositoryFactory, TranslationRepositoryFactory>(); 
        _serviceProvider = services.BuildServiceProvider();
    }

    static async Task Main()
    {
        try
        {
            ConfigureServices();
            
            // CREATE TABLE Translations (
            //     Key TEXT NOT NULL,
            //     Language TEXT NOT NULL,
            //     Text TEXT NOT NULL,
            //     PRIMARY KEY (Key, Language)
            // );
            DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");

            var repositoryFactory = ServiceProvider.GetRequiredService<ITranslationRepositoryFactory>();
            var repository = repositoryFactory.CreateDatabaseRepository(factory, @"Data Source=C:\Temp\translations.db");
            

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