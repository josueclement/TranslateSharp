using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranslateSharp.Abstractions;

/// <summary>
/// Represents a translation repository
/// </summary>
public interface ITranslationRepository
{
    /// <summary>
    /// Asynchronously get all translations
    /// </summary>
    Task<IEnumerable<Translation>> GetAllTranslationsAsync();
    
    /// <summary>
    /// Asynchronously get translations by their key
    /// </summary>
    Task<IEnumerable<Translation>> GetTranslationsAsync(string key);
    
    /// <summary>
    /// Asynchronously get translation by its key and language
    /// </summary>
    Task<Translation> GetTranslationAsync(string key, string language);
    
    /// <summary>
    /// Asynchronously add a translation
    /// </summary>
    Task<int> AddTranslationAsync(Translation translation);
    
    /// <summary>
    /// Asynchronously delete a translation
    /// </summary>
    Task<int> DeleteTranslationAsync(Translation translation);
    
    /// <summary>
    /// Asynchronously update a translation
    /// </summary>
    Task<int> UpdateTranslationAsync(Translation translation);
}