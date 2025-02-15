using System.Collections.Generic;
using System.Threading.Tasks;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

public class JsonFileTranslationRepository : ITranslationRepository
{
    private readonly string _filePath;

    // ReSharper disable once ConvertToPrimaryConstructor
    public JsonFileTranslationRepository(string filePath)
    {
        _filePath = filePath;
    }
    
    public Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
    {
        throw new System.NotImplementedException();
    }

    public Task<Translation?> GetTranslationAsync(string key, string language)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> AddTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> DeleteTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }

    public Task<int> UpdateTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }
}