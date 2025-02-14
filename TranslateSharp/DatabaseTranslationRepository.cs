using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

/// <summary>
/// Database translation repository implementation
/// </summary>
// ReSharper disable once InconsistentNaming
public class DatabaseTranslationRepository : ITranslationRepository
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    //
    // static DatabaseTranslationRepository()
    // {
    //     DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
    //     Factory = DbProviderFactories.GetFactory("System.Data.SQLite");
    // }

    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseTranslationRepository(DbProviderFactory factory, string connectionString)
    {
        _connectionString = connectionString;
        _factory = factory;
    }

    protected virtual DbConnection GetConnection()
    {
        var connection = _factory.CreateConnection();
        
        if (connection is null)
            throw new InvalidOperationException("Database connection could not be created");
        
        connection.ConnectionString = _connectionString;
        return connection;
    }

    private async Task<IEnumerable<T>> Query<T>(string query)
    {
        DbConnection? connection = null;
        
        try
        {
            connection = GetConnection();
            await connection.OpenAsync();
            
            return await connection.QueryAsync<T>(query);
        }
        finally
        {
            if (connection is not null)
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }

    private DynamicParameters GetDynamicParameters(object[] parameters)
    {
        var dynamicParameters = new DynamicParameters();
        foreach (var param in parameters)
            dynamicParameters.AddDynamicParams(param);
        return dynamicParameters;
    }

    private async Task<IEnumerable<T>> Query<T>(string query, params object[] parameters)
    {
        DbConnection? connection = null;
        
        try
        {
            connection = GetConnection();
            await connection.OpenAsync();
            
            var dynamicParameters = GetDynamicParameters(parameters);
            
            return await connection.QueryAsync<T>(query, dynamicParameters);
        }
        finally
        {
            if (connection is not null)
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        }
    }

    private async Task<T> QueryFirst<T>(string query, params object[] parameters)
    {
        DbConnection? connection = null;
        
        try
        {
            connection = GetConnection();
            await connection.OpenAsync();
            
            var dynamicParameters = GetDynamicParameters(parameters);

            return await connection.QueryFirstAsync<T>(query, dynamicParameters);
        }
        finally
        {
            if (connection is not null)
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        } 
    }

    private async Task<int> Execute(string query, params object[] parameters)
    {
        DbConnection? connection = null;
        
        try
        {
            connection = GetConnection();
            await connection.OpenAsync();
            
            var dynamicParameters = GetDynamicParameters(parameters);

            return await connection.ExecuteAsync(query, dynamicParameters);
        }
        finally
        {
            if (connection is not null)
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
        } 
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<ITranslation>> GetAllTranslationsAsync()
        => await Query<ITranslation>("SELECT Key, Language, Text FROM Translations");

    /// <inheritdoc />
    public async Task<IEnumerable<ITranslation>> GetTranslationsAsync(string key)
        => await Query<ITranslation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key", key);

    /// <inheritdoc />
    public async Task<ITranslation> GetTranslationAsync(string key, string language)
        => await QueryFirst<ITranslation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key AND Language = @Language", key, language);

    /// <inheritdoc />
    public async Task<int> AddTranslationAsync(ITranslation translation)
        => await Execute("INSERT INTO Translations (Key, Language, Text) VALUES (@Key, @Language, @Text)", translation);

    /// <inheritdoc />
    public async Task<int> DeleteTranslationAsync(ITranslation translation)
        => await Execute("DELETE FROM Translations WHERE Key = @Key AND Language = @Language", translation);

    /// <inheritdoc />
    public async Task<int> UpdateTranslationAsync(ITranslation translation)
        => await Execute("UPDATE Translations SET Text = @Text WHERE Key = @Key AND Language = @Language", translation);
}