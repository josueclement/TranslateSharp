using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranslateSharp.Abstractions;

/// <summary>
/// Represents a translation service
/// </summary>
public interface ITranslationService
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
    /// Asynchronously add a translation
    /// </summary>
    Task<bool> AddTranslationAsync(Translation translation);
    
    /// <summary>
    /// Asynchronously delete a translation
    /// </summary>
    Task<bool> DeleteTranslationAsync(Translation translation);
    
    /// <summary>
    /// Asynchronously update a translation
    /// </summary>
    Task<bool> UpdateTranslationAsync(Translation translation);
}