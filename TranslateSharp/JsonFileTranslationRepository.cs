using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TranslateSharp.Abstractions;

namespace TranslateSharp;

/// <summary>
/// Json file repository implementation
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class JsonFileTranslationRepository : ITranslationRepository
{
    private readonly string _filePath;

    // ReSharper disable once ConvertToPrimaryConstructor
    public JsonFileTranslationRepository(string filePath)
    {
        _filePath = filePath;
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        await using (fs.ConfigureAwait(false))
        {
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                var json = await sr.ReadToEndAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Translation>>(json) ?? Enumerable.Empty<Translation>();
            }
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Translation>> GetTranslationsAsync(string key)
    {
        var translations = await GetAllTranslationsAsync().ConfigureAwait(false);
        return translations.Where(t => t.Key == key);
    }

    /// <inheritdoc />
    public async Task<Translation?> GetTranslationAsync(string key, string language)
    {
        var translations = await GetAllTranslationsAsync().ConfigureAwait(false);
        return translations.FirstOrDefault(t => t.Key == key && t.Language == language);
    }

    /// <inheritdoc />
    public async Task<int> AddTranslationAsync(Translation translation)
    {
        var translations = new List<Translation>(await GetAllTranslationsAsync().ConfigureAwait(false)) { translation };

        var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        await using (fs.ConfigureAwait(false))
        {
            await using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(translations);
                await sw.WriteAsync(json).ConfigureAwait(false);
            }
        }

        return 1;
    }

    /// <inheritdoc />
    public async Task<int> DeleteTranslationAsync(Translation translation)
    {
        var translations = new List<Translation>(await GetAllTranslationsAsync().ConfigureAwait(false));
        var removed = translations.RemoveAll(x => x.Key == translation.Key && x.Language == translation.Language);
        
        var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        await using (fs.ConfigureAwait(false))
        {
            await using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(translations);
                await sw.WriteAsync(json).ConfigureAwait(false);
            }
        }

        return removed; 
    }

    /// <inheritdoc />
    public async Task<int> UpdateTranslationAsync(Translation translation)
    {
        var translations = new List<Translation>(await GetAllTranslationsAsync().ConfigureAwait(false));
        var removed = translations.RemoveAll(x => x.Key == translation.Key && x.Language == translation.Language); 
        translations.Add(translation);
        
        var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        await using (fs.ConfigureAwait(false))
        {
            await using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(translations);
                await sw.WriteAsync(json).ConfigureAwait(false);
            }
        }

        return removed; 
    }
}