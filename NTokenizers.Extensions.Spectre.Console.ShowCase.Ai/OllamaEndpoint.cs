using System.Text.Json;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;
internal static class OllamaEndpoint
{
    internal static async Task<OllamaModelStatus> GetStatusAsync(string endpoint, string modelId)
    {
        var status = new OllamaModelStatus();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        using (var httpClient = new HttpClient { BaseAddress = new Uri(endpoint) })
        {
            // Check if the endpoint is up
            try
            {
                var response = await httpClient.GetAsync("");
                status.IsUp = response.IsSuccessStatusCode;
            }
            catch
            {
                status.IsUp = false;
                return status;
            }

            // Check if the model is available
            try
            {
                var tagsResponse = await httpClient.GetStringAsync("api/tags");
                var tags = JsonSerializer.Deserialize<TagsResponse>(tagsResponse, options);
                status.IsAvailable = tags?.Models?.Any(m => m.Model == modelId || m.Model == $"{modelId}:latest") is true;
            }
            catch
            {
                status.IsAvailable = false;
            }

            // Check if the model is running
            try
            {
                var psResponse = await httpClient.GetStringAsync("api/ps");
                var ps = JsonSerializer.Deserialize<PsResponse>(psResponse, options);
                status.IsRunning = ps?.Models?.Any(m => m.Model == modelId || m.Model == $"{modelId}:latest") is true;
            }
            catch
            {
                status.IsRunning = false;
            }
        }

        return status;
    }
}

public class OllamaModelStatus
{
    public bool IsUp { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsRunning { get; set; }
}

public class TagsResponse
{
    public List<ModelInfo>? Models { get; set; }
}

public class PsResponse
{
    public List<ModelInfo>? Models { get; set; }
}

public class ModelInfo
{
    public string? Model { get; set; }
}