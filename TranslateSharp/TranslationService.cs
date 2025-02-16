using System.Collections.Generic;
using System.Threading.Tasks;
using TranslateSharp.Abstractions;
using ZiggyCreatures.Caching.Fusion;

namespace TranslateSharp;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _repository;
    private readonly FusionCache _cache = new(new FusionCacheOptions());

    // ReSharper disable once ConvertToPrimaryConstructor
    public TranslationService(ITranslationRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        const string cacheKey = "translations:_all_";

        return await _cache.GetOrSetAsync<IEnumerable<Translation>>(cacheKey, 
            async _ => await _repository.GetAllTranslationsAsync().ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
    {
        var cacheKey = $"translations:{key}" + key;
        
        return await _cache.GetOrSetAsync<IEnumerable<Translation>>(cacheKey,
            async _ => await _repository.GetTranslationsAsync(key).ConfigureAwait(false)).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<bool> AddTranslationAsync(Translation translation)
    {
        var result = await _repository.AddTranslationAsync(translation).ConfigureAwait(false);
        await _cache.ClearAsync().ConfigureAwait(false);
        return result > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTranslationAsync(Translation translation)
    {
        var result = await _repository.DeleteTranslationAsync(translation).ConfigureAwait(false);
        await _cache.ClearAsync().ConfigureAwait(false);
        return result > 0;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateTranslationAsync(Translation translation)
    {
        var result = await _repository.UpdateTranslationAsync(translation).ConfigureAwait(false);
        await _cache.ClearAsync().ConfigureAwait(false);
        return result > 0;
    }
}