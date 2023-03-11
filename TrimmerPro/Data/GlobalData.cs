namespace TrimmerPro.Data;

public static class GlobalData
{
    public static AppConfig Config { get; private set; } = null!;

    public static void Init()
    {
        if (File.Exists(AppConfig.SavePath))
        {
            try
            {
                var json = File.ReadAllText(AppConfig.SavePath);
                Config = string.IsNullOrWhiteSpace(json)
                    ? new AppConfig()
                    : JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch
            {
                Config = new AppConfig();
            }
        }
        else
        {
            Config = new AppConfig();
        }
    }

    public static void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(Config);
            File.WriteAllText(AppConfig.SavePath, json);
        }
        catch (UnauthorizedAccessException)
        {
            MessageBox.Error("You don't have administrator access, please run app as administrator",
                "Administrator Access Error");
        }
    }
}