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
    
    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
        => await _repository.GetAllTranslationsAsync();

    public async Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
        => await _repository.GetTranslationsAsync(key);

    public async Task<Translation?> GetTranslationAsync(string key, string language)
        => await _repository.GetTranslationAsync(key, language);

    public async Task<bool> AddTranslationAsync(Translation translation)
    {
        var result = await _repository.AddTranslationAsync(translation);
        return result > 0;
    }

    public async Task<bool> DeleteTranslationAsync(Translation translation)
    {
        var result = await _repository.DeleteTranslationAsync(translation);
        return result > 0;
    }

    public async Task<bool> UpdateTranslationAsync(Translation translation)
    {
        var result = await _repository.UpdateTranslationAsync(translation);
        return result > 0;
    }
}