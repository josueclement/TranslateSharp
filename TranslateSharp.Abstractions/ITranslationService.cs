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
    Task<IEnumerable<ITranslation>> GetAllTranslationsAsync();
    
    /// <summary>
    /// Asynchronously get translations by their key
    /// </summary>
    Task<IEnumerable<ITranslation>> GetTranslationsAsync(string key);
    
    /// <summary>
    /// Asynchronously get translation by its key and language
    /// </summary>
    Task<ITranslation> GetTranslationAsync(string key, string language);
    
    /// <summary>
    /// Asynchronously add a translation
    /// </summary>
    Task<int> AddTranslationAsync(ITranslation translation);
    
    /// <summary>
    /// Asynchronously delete a translation
    /// </summary>
    Task<int> DeleteTranslationAsync(ITranslation translation);
    
    /// <summary>
    /// Asynchronously update a translation
    /// </summary>
    Task<int> UpdateTranslationAsync(ITranslation translation);
}