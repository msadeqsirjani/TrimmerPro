namespace TrimmerPro.ViewModels;

public class UpdateViewModel : BindableBase
{
    public UpdateViewModel()
    {
        _version = string.Empty;
        _createdAt = string.Empty;
        _publishedAt = string.Empty;
        _currentVersion = string.Empty;
        _changeLog = string.Empty;

        CheckForUpdateCommand = new DelegateCommand(CheckForUpdate);
    }

    private string _version;

    public string Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
    }

    private string _createdAt;

    public string CreatedAt
    {
        get => _createdAt;
        set => SetProperty(ref _createdAt, value);
    }

    private string _publishedAt;

    public string PublishedAt
    {
        get => _publishedAt;
        set => SetProperty(ref _publishedAt, value);
    }

    private string _currentVersion;

    public string CurrentVersion
    {
        get => _currentVersion;
        set => SetProperty(ref _currentVersion, value);
    }

    private string _changeLog;

    public string ChangeLog
    {
        get => _changeLog;
        set => SetProperty(ref _changeLog, value);
    }

    public DelegateCommand CheckForUpdateCommand { get; }

    private void CheckForUpdate()
    {
        try
        {
            var releaseInfo = UpdateHelper.CheckUpdate("msadeqsirjani", "TrimmerPro");
            CreatedAt = releaseInfo.CreatedAt.ToString(CultureInfo.InvariantCulture);
            PublishedAt = releaseInfo.PublishedAt.ToString(CultureInfo.InvariantCulture);
            CurrentVersion = (Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0)).ToString();
            Version = releaseInfo.TagName.Replace("v", "");
            ChangeLog = releaseInfo.Changelog;
            
            if (releaseInfo.IsExistNewVersion)
            {
                Growl.Success("New Version Found!");
            }
            else
            {
                Growl.Info("You are using Latest Version");
            }
        }
        catch (Exception)
        {
            Growl.Error("Can not find any version");
        }
    }
}