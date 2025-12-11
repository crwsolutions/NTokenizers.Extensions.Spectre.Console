using NTokenizers.Yaml;
using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Extensions.Spectre.Console.Writers;
using System.Text;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console;

/// <summary>
/// Provides extension methods for writing YAML content to the console with syntax highlighting.
/// This class enables style-rich console output for YAML data using the Spectre.Console library
/// and NTokenizers for tokenization.
/// </summary>
public static class AnsiConsoleYamlExtensions
{
    /// <summary>
    /// Writes YAML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing YAML content.</param>
    /// <param name="yamlStyles">The styles to use for YAML token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The YAML content as a string.</returns>
    public static async Task<string> WriteYamlAsync(this IAnsiConsole ansiConsole, Stream stream, YamlStyles? yamlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var yamlWriter = new YamlWriter(ansiConsole, yamlStyles ?? YamlStyles.Default);
        if (encoding is null)
        {
            // Call overload without encoding to preserve BOM detection
            return await YamlTokenizer.Create().ParseAsync(
                stream,
                ct,
                yamlWriter.WriteToken
            );
        }
        else
        {
            // Call overload with encoding and cancellation token
            return await YamlTokenizer.Create().ParseAsync(
                stream,
                encoding,
                ct,
                yamlWriter.WriteToken
            );
        }
    }

    /// <summary>
    /// Writes YAML content from a stream to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="stream">The stream containing YAML content.</param>
    /// <param name="yamlStyles">The styles to use for YAML token coloring.</param>
    /// <param name="encoding">The character encoding to use. If null, encoding will be detected from the stream's byte order mark (BOM).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The YAML content as a string.</returns>
    public static string WriteYaml(this IAnsiConsole ansiConsole, Stream stream, YamlStyles? yamlStyles = null, Encoding? encoding = null, CancellationToken ct = default)
    {
        var t = Task.Run(() => WriteYamlAsync(ansiConsole, stream, yamlStyles, encoding, ct), ct);
        return t.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Writes YAML content from a string to the console with default styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The YAML string to write.</param>
    public static void WriteYaml(this IAnsiConsole ansiConsole, string value) =>
        WriteYaml(ansiConsole, value, YamlStyles.Default);

    /// <summary>
    /// Writes YAML content from a string to the console with custom styling.
    /// </summary>
    /// <param name="ansiConsole">The console to write to.</param>
    /// <param name="value">The YAML string to write.</param>
    /// <param name="yamlStyles">The styles to use for YAML token coloring.</param>
    public static void WriteYaml(this IAnsiConsole ansiConsole, string value, YamlStyles yamlStyles)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(value));
        var t = Task.Run(() => WriteYamlAsync(ansiConsole, stream, yamlStyles, Encoding.UTF8, default));
        t.GetAwaiter().GetResult();
    }
}
