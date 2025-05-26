using FileAnalysisService.Application.DTOs;

namespace FileAnalysisService.Application.Services;

public class FileStoringClient(HttpClient httpClient) : IFileStoringClient
{
    private readonly HttpClient _httpClient = httpClient;

    public FileDto? GetFileById(Guid fileId)
    {
        var response = _httpClient.GetAsync($"/files/{fileId}").Result;

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        string? fileName = null;
        if (response.Content.Headers.ContentDisposition != null)
        {
            fileName = response.Content.Headers.ContentDisposition.FileName?.Trim('\"');
        }
        else if (response.Content.Headers.TryGetValues("Content-Disposition", out var values))
        {
            var disposition = values.FirstOrDefault();
            if (disposition != null)
            {
                var fileNamePart = disposition.Split(';').FirstOrDefault(x => x.Trim().StartsWith("filename="));
                if (fileNamePart != null)
                {
                    fileName = fileNamePart.Split('=')[1].Trim('\"');
                }
            }
        }

        if (string.IsNullOrEmpty(fileName))
        {
            return null;
        }

        var fileContent = response.Content.ReadAsStringAsync().Result;

        return new FileDto(fileId, fileName, fileContent);
    }
}