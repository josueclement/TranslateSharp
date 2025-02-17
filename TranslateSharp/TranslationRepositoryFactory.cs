using System;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

public interface ITranslationRepositoryFactory
{
    ITranslationRepository CreateDatabaseRepository(Func<DbConnection> connectionFactory);
    ITranslationRepository CreateJsonFileRepository(string filePath);
}

public class TranslationRepositoryFactory : ITranslationRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    // ReSharper disable once ConvertToPrimaryConstructor
    public TranslationRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public ITranslationRepository CreateDatabaseRepository(Func<DbConnection> connectionFactory)
        => ActivatorUtilities.CreateInstance<DatabaseTranslationRepository>(_serviceProvider, connectionFactory);

    public ITranslationRepository CreateJsonFileRepository(string filePath)
        => ActivatorUtilities.CreateInstance<JsonFileTranslationRepository>(_serviceProvider, filePath);
}