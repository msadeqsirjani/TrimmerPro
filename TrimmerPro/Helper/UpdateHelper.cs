namespace TrimmerPro.Helper;

public static class UpdateHelper
{
    private const string GithubApi = "https://api.github.com/repos/{0}/{1}/releases/latest";

    private static SystemVersionInfo GetAsVersionInfo(string version)
    {
        var nums = GetVersionNumbers(version).Split('.').Select(int.Parse).ToList();

        return nums.Count switch
        {
            <= 1 => new SystemVersionInfo(nums[0], 0, 0),
            <= 2 => new SystemVersionInfo(nums[0], nums[1], 0),
            <= 3 => new SystemVersionInfo(nums[0], nums[1], nums[2]),
            _ => new SystemVersionInfo(nums[0], nums[1], nums[2])
        };
    }

    private static string GetVersionNumbers(string version)
    {
        const string allowedChars = "01234567890.";
        return new string(version.Where(c => allowedChars.Contains(c)).ToArray());
    }

    public static ReleaseInfo CheckUpdate(string username, string repository, Version? currentVersion = null)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        if (string.IsNullOrEmpty(repository))
            throw new ArgumentNullException(nameof(repository));

        ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;

        var url = string.Format(GithubApi, username, repository);
        using var request = new HttpClient();
        var response = request.GetAsync(url).Result;
        var reader = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<ReleaseInfo>(reader) ?? new ReleaseInfo();

        if (currentVersion == null)
        {
            currentVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0);
        }

        var newVersionInfo = GetAsVersionInfo(result.TagName);
        var major = currentVersion.Major == -1 ? 0 : currentVersion.Major;
        var minor = currentVersion.Minor == -1 ? 0 : currentVersion.Minor;
        var build = currentVersion.Build == -1 ? 0 : currentVersion.Build;

        var currentVersionInfo = new SystemVersionInfo(major, minor, build);

        ArgumentNullException.ThrowIfNull(result);

        return new ReleaseInfo
        {
            Changelog = result.Changelog,
            CreatedAt = Convert.ToDateTime(result.CreatedAt),
            Assets = result.Assets,
            IsPreRelease = result.IsPreRelease,
            PublishedAt = Convert.ToDateTime(result.PublishedAt),
            TagName = result.TagName,
            ApiUrl = result.ApiUrl,
            ReleaseUrl = result.ReleaseUrl,
            IsExistNewVersion = newVersionInfo > currentVersionInfo
        };
    }

    public static async Task<ReleaseInfo> CheckUpdateAsync(string username, string repository,
        Version? currentVersion = null)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException(nameof(username));

        if (string.IsNullOrEmpty(repository))
            throw new ArgumentNullException(nameof(repository));

        ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", username);
        var url = string.Format(GithubApi, username, repository);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ReleaseInfo>(responseBody) ?? new ReleaseInfo();

        if (currentVersion == null)
        {
            currentVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(1, 0, 0);
        }

        var newVersionInfo = GetAsVersionInfo(result.TagName);
        var major = currentVersion.Major == -1 ? 0 : currentVersion.Major;
        var minor = currentVersion.Minor == -1 ? 0 : currentVersion.Minor;
        var build = currentVersion.Build == -1 ? 0 : currentVersion.Build;

        var currentVersionInfo = new SystemVersionInfo(major, minor, build);

        ArgumentNullException.ThrowIfNull(result);

        return new ReleaseInfo
        {
            Changelog = result.Changelog,
            CreatedAt = Convert.ToDateTime(result.CreatedAt),
            Assets = result.Assets,
            IsPreRelease = result.IsPreRelease,
            PublishedAt = Convert.ToDateTime(result.PublishedAt),
            TagName = result.TagName,
            ApiUrl = result.ApiUrl,
            ReleaseUrl = result.ReleaseUrl,
            IsExistNewVersion = newVersionInfo > currentVersionInfo
        };
    }
}