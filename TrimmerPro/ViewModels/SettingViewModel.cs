namespace TrimmerPro.ViewModels;

public class SettingViewModel : BindableBase
{
    public SettingViewModel()
    {
        IsShowNotifyIconCommand = new DelegateCommand<object>(IsShowNotifyIcon);
        
        InitSetting();
    }
    
    private bool _getIsCheckedShowNotifyIcon;
    public bool GetIsCheckedShowNotifyIcon
    {
        get => _getIsCheckedShowNotifyIcon;
        set => SetProperty(ref _getIsCheckedShowNotifyIcon, value);
    }
    
    public DelegateCommand<object> IsShowNotifyIconCommand { get; }

    private void InitSetting() => GetIsCheckedShowNotifyIcon = GlobalData.Config.IsShowNotifyIcon;

    private static void IsShowNotifyIcon(object isChecked)
    {
        if ((bool) isChecked == GlobalData.Config.IsShowNotifyIcon)
        {
            return;
        }
        
        GlobalData.Config.IsShowNotifyIcon = (bool) isChecked;
        GlobalData.Save();
    }
}