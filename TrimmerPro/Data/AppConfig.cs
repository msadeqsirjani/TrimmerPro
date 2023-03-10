namespace TrimmerPro.Data;

public class AppConfig
{
    public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

    public bool IsShowNotifyIcon { get; set; }
    public bool IsFirstRun { get; set; } = true;
    public int SelectedIndex { get; set; }

    public SkinType Skin { get; set; } = SkinType.Default;
}