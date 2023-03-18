namespace TrimmerPro.ViewModels;

public class AboutViewModel : BindableBase
{
    public AboutViewModel() => Version = "Version " + Assembly.GetExecutingAssembly().GetName().Version;

    private string _version = string.Empty;
    public string Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
    }
}