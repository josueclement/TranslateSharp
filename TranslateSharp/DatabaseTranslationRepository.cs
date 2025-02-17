using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

// CREATE TABLE Translations (
//     Key TEXT NOT NULL,
//     Language TEXT NOT NULL,
//     Text TEXT NOT NULL,
//     PRIMARY KEY (Key, Language)
// );

/// <summary>
/// Database translation repository implementation
/// </summary>
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseTranslationRepository : ITranslationRepository
{
    private readonly Func<DbConnection> _connectionFactory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DatabaseTranslationRepository(Func<DbConnection> connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false); 
            return await connection
                .QueryAsync<Translation>("SELECT Key, Language, Text FROM Translations")
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false);
            return await connection
                .QueryAsync<Translation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key", key)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<Translation?> GetTranslationAsync(string key, string language)
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false);

            var parameters = new DynamicParameters();
            parameters.Add("@Key", key);
            parameters.Add("@Language", language);

            return await connection
                .QueryFirstOrDefaultAsync<Translation>("SELECT Key, Language, Text FROM Translations WHERE Key = @Key AND Language = @Language", parameters)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<int> AddTranslationAsync(Translation translation)
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false);
            return await connection
                .ExecuteAsync("INSERT INTO Translations (Key, Language, Text) VALUES (@Key, @Language, @Text)", translation)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<int> DeleteTranslationAsync(Translation translation)
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false);
            return await connection
                .ExecuteAsync("DELETE FROM Translations WHERE Key = @Key AND Language = @Language", translation)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task<int> UpdateTranslationAsync(Translation translation)
    {
        var connection = _connectionFactory();
        await using (connection.ConfigureAwait(false))
        {
            await connection.OpenAsync().ConfigureAwait(false);
            return await connection
                .ExecuteAsync("UPDATE Translations SET Text = @Text WHERE Key = @Key AND Language = @Language", translation)
                .ConfigureAwait(false);
        }
    }
}