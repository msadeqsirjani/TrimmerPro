namespace TrimmerPro.Models;

public class Asset
{
    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string Url { get; set; } = string.Empty;
}