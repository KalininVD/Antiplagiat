namespace FileAnalysisService.Application.Services;

public class WordCloudClient(HttpClient httpClient) : IWordCloudClient
{
    private readonly HttpClient _httpClient = httpClient;

    public byte[]? GenerateWordCloud(string content)
    {
        if (content.Length < 1000) // If content is less than 1000 characters, use GET request
        {
            var url = $"/wordcloud?text={Uri.EscapeDataString(content)}&format=png";

            var response = _httpClient.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.Content.ReadAsByteArrayAsync().Result;
        }
        else
        {
            var url = "/wordcloud";
            
            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(new { text = content, format = "png" }),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = _httpClient.PostAsync(url, jsonContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.Content.ReadAsByteArrayAsync().Result;
        }
    }
}