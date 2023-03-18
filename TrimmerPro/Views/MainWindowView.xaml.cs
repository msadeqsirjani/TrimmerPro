namespace TrimmerPro.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (GlobalData.Config.IsShowNotifyIcon)
        {
            if (GlobalData.Config.IsFirstRun)
            {
                var result = MessageBox.Show(new MessageBoxInfo
                {
                    Message =
                        "Application will be hidden instead of closed and you can access it from SystemTray, do you want?",
                    Caption = "Url Shortener",
                    Button = MessageBoxButton.YesNo,
                    IconBrushKey = ResourceToken.AccentBrush,
                    IconKey = ResourceToken.InfoGeometry
                });
                
                if (result == MessageBoxResult.Yes)
                {
                    Hide();
                    e.Cancel = true;
                    GlobalData.Config.IsFirstRun = false;
                    GlobalData.Save();
                }
                else
                {
                    base.OnClosing(e);
                }
            }
            else
            {
                Hide();
                e.Cancel = true;
            }
        }
        else
        {
            base.OnClosing(e);
        }
    }

    private void ConfigButton_OnClick(object sender, RoutedEventArgs e)
    {
        PopupConfig.IsOpen = true;
    }

    private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not Button {Tag: SkinType tag})
        {
            return;
        }

        PopupConfig.IsOpen = false;

        if (tag.Equals(GlobalData.Config.Skin))
        {
            return;
        }

        GlobalData.Config.Skin = tag;
        GlobalData.Save();
        ((App) Application.Current).UpdateSkin(tag);
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e) => Environment.Exit(0);
}