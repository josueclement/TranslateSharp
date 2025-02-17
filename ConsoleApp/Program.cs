using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ITranslationRepositoryFactory, TranslationRepositoryFactory>();
        
        // Add ITranslationRepository depending of the configuration
        if (configuration.GetValue<string>("General:TranslationRepositoryType") == "Database")
        {
            services.AddSingleton<ITranslationRepository>(sp =>
                sp.GetRequiredService<ITranslationRepositoryFactory>()
                    .CreateDatabaseRepository(() =>
                        GetConnection(configuration.GetRequiredValue<string>("Database:Provider"),
                            configuration.GetRequiredValue<string>("Database:ConnectionString"))));
        }
        else if (configuration.GetValue<string>("General:TranslationRepositoryType") == "JsonFile")
        {
            services.AddSingleton<ITranslationRepository>(sp =>
                sp.GetRequiredService<ITranslationRepositoryFactory>()
                    .CreateJsonFileRepository(configuration.GetRequiredValue<string>("TranslationRepositoryJsonFile")));
        }
        
        services.AddSingleton<ITranslationService, TranslationService>();
        _serviceProvider = services.BuildServiceProvider();
    }

    private static DbConnection GetConnection(string provider, string connectionString)
    {
        var connection = DbProviderFactories.GetFactory(provider).CreateConnection() ??
                         throw new InvalidOperationException($"Cannot create DbConnection from provider: {provider}");
        connection.ConnectionString = connectionString;
        return connection; 
    }

    /// <summary>
    /// Register database from configuration
    /// </summary>
    static void RegisterDatabase()
    {
        var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

        DbProviderFactories.RegisterFactory(
            configuration.GetRequiredValue<string>("Database:Provider"),
            configuration.GetRequiredValue<string>("Database:Factory")); 
    }

    static async Task Main()
    {
        try
        {
            ConfigureServices();
            RegisterDatabase();
            
            var service = ServiceProvider.GetRequiredService<ITranslationService>();

            // var res = await repository.AddTranslationAsync(new Translation("MyKey2", "en", "This is my key"));
            // var translation = await service.GetTranslationAsync(key: "MyKey", language: "en");

            var translations = await service.GetAllTranslationsAsync();
            await service.AddTranslationAsync(new Translation("Key4", "fr", "blah"));
            var translations2 = await service.GetAllTranslationsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
        => configuration.GetValue<T>(key) ?? throw new InvalidOperationException($"Missing required value for key: {key}");
}