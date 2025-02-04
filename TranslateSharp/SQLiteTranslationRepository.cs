using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using CsharpExtensions;
using Dapper;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

/// <summary>
/// SQLite translation repository implementation
/// </summary>
// ReSharper disable once InconsistentNaming
public class SQLiteTranslationRepository : ITranslationRepository
{
    private static readonly DbProviderFactory Factory;
    private readonly string _connectionString;
    
    static SQLiteTranslationRepository()
    {
        DbProviderFactories.RegisterFactory("System.Data.SQLite", SQLiteFactory.Instance);
        Factory = DbProviderFactories.GetFactory("System.Data.SQLite");
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public SQLiteTranslationRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected virtual DbConnection GetConnection()
    {
        var connection = Factory.CreateConnection();
        
        if (connection is null)
            throw new InvalidOperationException();
        
        connection.ConnectionString = _connectionString;
        return connection;
    }

    private async Task ExecuteWithConnection(Action<DbConnection> connectionAction)
    {
        DbConnection? connection = null;

        try
        {
            connection = GetConnection();
            await connection.OpenAsync();
            
            connectionAction(connection);
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
    public async Task<ValueResult<IEnumerable<ITranslation>>> GetAllTranslationsAsync()
    {
        try
        {
            IEnumerable<ITranslation>? translations = null;

            await ExecuteWithConnection(async void (connection) =>
            {
                translations = (await connection.QueryAsync<ITranslation>("SELECT Key, Language, Text FROM Translations"));
            });
                

            if (translations?.Any() ?? false)
                return ValueResult<IEnumerable<ITranslation>>.Success(translations);
            else
                return ValueResult<IEnumerable<ITranslation>>.Error(new InvalidOperationException("No translations found."));
        }
        catch (Exception ex)
        {
            return ValueResult<IEnumerable<ITranslation>>.Error(ex);
        }
    }

    /// <inheritdoc />
    public async Task<ValueResult<IEnumerable<ITranslation>>> GetTranslationsAsync(string key)
    {
        try
        {
            IEnumerable<ITranslation>? translations = null;
            
            await ExecuteWithConnection(async connection => 
                translations = await connection.QueryAsync<ITranslation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key", key));
            
            if (translations?.Any() ?? false)
                return ValueResult<IEnumerable<ITranslation>>.Success(translations);
            else
                return ValueResult<IEnumerable<ITranslation>>.Error(new InvalidOperationException("No translations found."));
        }
        catch (Exception ex)
        {
            return ValueResult<IEnumerable<ITranslation>>.Error(ex);
        }
    }

    /// <inheritdoc />
    public async Task<ValueResult<ITranslation>> GetTranslationAsync(string key, string language)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<Result<bool>> AddTranslationAsync(ITranslation translation)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<Result<bool>> DeleteTranslationAsync(ITranslation translation)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<Result<bool>> UpdateTranslationAsync(ITranslation translation)
    {
        throw new System.NotImplementedException();
    }
}