using System.Collections.Generic;
using System.Threading.Tasks;
using CsharpExtensions;

namespace TranslateSharp.Abstractions;

/// <summary>
/// Represents a translation service
/// </summary>
public interface ITranslationService
{
    /// <summary>
    /// Asynchronously get all translations
    /// </summary>
    Task<ValueResult<IEnumerable<ITranslation>>> GetAllTranslationsAsync();
    
    /// <summary>
    /// Asynchronously get translations by their key
    /// </summary>
    Task<ValueResult<IEnumerable<ITranslation>>> GetTranslationsAsync(string key);
    
    /// <summary>
    /// Asynchronously get translation by its key and language
    /// </summary>
    Task<ValueResult<ITranslation>> GetTranslationAsync(string key, string language);
    
    /// <summary>
    /// Asynchronously add a translation
    /// </summary>
    Task<Result<bool>> AddTranslationAsync(ITranslation translation);
    
    /// <summary>
    /// Asynchronously delete a translation
    /// </summary>
    Task<Result<bool>> DeleteTranslationAsync(ITranslation translation);
    
    /// <summary>
    /// Asynchronously update a translation
    /// </summary>
    Task<Result<bool>> UpdateTranslationAsync(ITranslation translation);
}