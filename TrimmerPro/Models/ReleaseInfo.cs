namespace TrimmerPro.Models;

public class ReleaseInfo
{
    [JsonPropertyName("url")] public string ApiUrl { get; set; } = string.Empty;
    [JsonPropertyName("html_url")] public string ReleaseUrl { get; set; } = string.Empty;
    [JsonPropertyName("tag_name")] public string TagName { get; set; } = string.Empty;
    [JsonPropertyName("prerelease")] public bool IsPreRelease { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("published_at")] public DateTime PublishedAt { get; set; }
    [JsonPropertyName("assets")] public List<Asset> Assets { get; set; } = new();
    [JsonPropertyName("body")] public string Changelog { get; set; } = string.Empty;
    public bool IsExistNewVersion { get; set; }
}