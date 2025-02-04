namespace TranslateSharp.Abstractions;

/// <summary>
/// Represents a translation
/// </summary>
public interface ITranslation
{
    /// <summary>
    /// Translation key
    /// </summary>
    string Key { get; }
    
    /// <summary>
    /// Language
    /// </summary>
    string Language { get; }
    
    /// <summary>
    /// Translated text
    /// </summary>
    string Text { get; }
}