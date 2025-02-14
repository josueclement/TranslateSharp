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
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class DatabaseTranslationRepository : ITranslationRepository
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;

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

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        await using var connection = GetConnection();
        await connection.OpenAsync(); 
        return await connection.QueryAsync<Translation>("SELECT Key, Language, Text FROM Translations");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<Translation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key", key);
    }

    /// <inheritdoc />
    public async Task<Translation?> GetTranslationAsync(string key, string language)
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("@Key", key);
        parameters.Add("@Language", language);
            
        return await connection.QueryFirstOrDefaultAsync<Translation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key AND Language = @Language", parameters);
    }

    /// <inheritdoc />
    public async Task<int> AddTranslationAsync(Translation translation)
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();
        return await connection.ExecuteAsync("INSERT INTO Translations (Key, Language, Text) VALUES (@Key, @Language, @Text)", translation);
    }

    /// <inheritdoc />
    public async Task<int> DeleteTranslationAsync(Translation translation)
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();
        return await connection.ExecuteAsync("DELETE FROM Translations WHERE Key = @Key AND Language = @Language", translation);
    }

    /// <inheritdoc />
    public async Task<int> UpdateTranslationAsync(Translation translation)
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();
        return await connection.ExecuteAsync("UPDATE Translations SET Text = @Text WHERE Key = @Key AND Language = @Language", translation);
    }
}