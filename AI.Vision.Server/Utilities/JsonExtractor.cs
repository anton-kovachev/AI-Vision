using System.Text.Json;

namespace AI.Vision.Server.Utilities;

public static class JsonExtractor
{
    public static T? ExtractAndParse<T>(string response, JsonSerializerOptions? options = null)
    {
        var jsonContent = ExtractJsonFromMarkdown(response);
        
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(jsonContent, options);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    private static string ExtractJsonFromMarkdown(string text)
    {
        const string jsonMarker = "```json";
        const string endMarker = "```";

        var startIndex = text.IndexOf(jsonMarker, StringComparison.OrdinalIgnoreCase);
        
        if (startIndex == -1)
        {
            // No markdown code block found, try parsing the entire text as JSON
            return text.Trim();
        }

        // Move past the ```json marker and any newlines
        startIndex += jsonMarker.Length;
        
        // Find the closing marker after the opening one
        var endIndex = text.IndexOf(endMarker, startIndex, StringComparison.Ordinal);
        
        if (endIndex == -1)
        {
            // No closing marker found, return everything after the opening marker
            return text[startIndex..].Trim();
        }

        // Extract the JSON content between the markers
        return text[startIndex..endIndex].Trim();
    }
}