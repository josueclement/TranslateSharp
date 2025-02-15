using System.Collections.Generic;
using System.Threading.Tasks;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _repository;

    // ReSharper disable once ConvertToPrimaryConstructor
    public TranslationService(ITranslationRepository repository)
    {
        _repository = repository;
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

    public Task<bool> AddTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> DeleteTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> UpdateTranslationAsync(Translation translation)
    {
        throw new System.NotImplementedException();
    }
}