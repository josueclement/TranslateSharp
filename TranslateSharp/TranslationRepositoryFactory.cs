using System;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

public interface ITranslationRepositoryFactory
{
    ITranslationRepository CreateDatabaseRepository(DbProviderFactory factory, string connectionString);
    ITranslationRepository CreateJsonFileRepository(string path);
}

public class TranslationRepositoryFactory : ITranslationRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    // ReSharper disable once ConvertToPrimaryConstructor
    public TranslationRepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public ITranslationRepository CreateDatabaseRepository(DbProviderFactory factory, string connectionString)
        => ActivatorUtilities.CreateInstance<DatabaseTranslationRepository>(_serviceProvider, factory, connectionString);

    public ITranslationRepository CreateJsonFileRepository(string path)
        => throw new NotImplementedException();
}