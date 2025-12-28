using System.Net.Http.Headers;
using System.Text.Json;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;

internal static class OpenAiEndpoint
{
    internal static async Task<OpenAiModelStatus> GetStatusAsync(
        string endpoint,
        string? apiKey,
        string modelId)
    {
        var status = new OpenAiModelStatus();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri(endpoint.TrimEnd('/') + "/")
        };

        if (apiKey is not null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        // 1️⃣ Check if endpoint + API key are valid
        try
        {
            var response = await httpClient.GetAsync("v1/models");
            status.IsUp = response.IsSuccessStatusCode;

            if (!status.IsUp)
                return status;
        }
        catch
        {
            status.IsUp = false;
            return status;
        }

        // 2️⃣ Check if model exists
        try
        {
            var modelsJson = await httpClient.GetStringAsync("v1/models");
            var models = JsonSerializer.Deserialize<OpenAiModelsResponse>(modelsJson, options);

            status.IsAvailable = models?.Data?.Any(m => m.Id == modelId) is true;

            // OpenAI models are always "running" if available
            status.IsRunning = status.IsAvailable;
        }
        catch
        {
            status.IsAvailable = false;
            status.IsRunning = false;
        }

        return status;
    }
}

public class OpenAiModelStatus
{
    public bool IsUp { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsRunning { get; set; }
}

public class OpenAiModelsResponse
{
    public List<OpenAiModelInfo>? Data { get; set; }
}

public class OpenAiModelInfo
{
    public string? Id { get; set; }
}

