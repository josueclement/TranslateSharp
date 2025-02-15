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

    private static string? _databaseFactory;
    // ReSharper disable once MemberCanBePrivate.Global
    public static string DatabaseFactory =>
        _databaseFactory ?? throw new InvalidOperationException("Database factory is not configured");

    private static string? _databaseProvider;
    // ReSharper disable once MemberCanBePrivate.Global
    public static string DatabaseProvider =>
        _databaseProvider ?? throw new InvalidOperationException("Database provider is not configured");
    
    private static string? _databaseConnectionString;
    // ReSharper disable once MemberCanBePrivate.Global
    public static string DatabaseConnectionString =>
        _databaseConnectionString ?? throw new InvalidOperationException("Database connection string is not configured");

    static void ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<ITranslationRepositoryFactory, TranslationRepositoryFactory>();
        if (configuration.GetValue<string>("General:TranslationRepositoryType") == "Database")
        {
            services.AddSingleton<ITranslationRepository>(sp =>
                sp.GetRequiredService<ITranslationRepositoryFactory>()
                    .CreateDatabaseRepository(
                        DbProviderFactories.GetFactory(configuration.GetValue<string>("Database:Provider")),
                        configuration.GetValue<string>("Database:ConnectionString")));
        }
        else if (configuration.GetValue<string>("General:TranslationRepositoryType") == "JsonFile")
        {
            services.AddSingleton<ITranslationRepository>(sp =>
                sp.GetRequiredService<ITranslationRepositoryFactory>()
                    .CreateJsonFileRepository(configuration.GetValue<string>("TranslationRepositoryJsonFile")));
        }
        // services.AddTransient<ITranslationRepository, DatabaseTranslationRepository>();
        _serviceProvider = services.BuildServiceProvider();
    }

    static void ReadConfiguration()
    {
        IConfiguration configuration = ServiceProvider.GetRequiredService<IConfiguration>();

        _databaseFactory = configuration.GetValue<string>("Database:Factory");
        _databaseProvider = configuration.GetValue<string>("Database:Provider");
        _databaseConnectionString = configuration.GetValue<string>("Database:ConnectionString"); 
    }

    static async Task Main()
    {
        try
        {
            ConfigureServices();
            ReadConfiguration();
            
            DbProviderFactories.RegisterFactory(DatabaseProvider, DatabaseFactory);
            
            var dbFactory = DbProviderFactories.GetFactory(DatabaseProvider);

            var repositoryFactory = ServiceProvider.GetRequiredService<ITranslationRepositoryFactory>();
            // var repository = repositoryFactory.CreateDatabaseRepository(dbFactory, DatabaseConnectionString);
            var repository = ServiceProvider.GetRequiredService<ITranslationRepository>();

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