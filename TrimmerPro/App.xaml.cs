namespace TrimmerPro;

public partial class App
{
    protected override void Initialize()
    {
        base.Initialize();

        GlobalData.Init();
        if (GlobalData.Config.Skin != SkinType.Default)
        {
            UpdateSkin(GlobalData.Config.Skin);
        }

        Container.Resolve<IRegionManager>().RequestNavigate("ContentRegion", "MainShortener");
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<MainShortenerView>();
        containerRegistry.RegisterForNavigation<MainContentView>();
        containerRegistry.RegisterForNavigation<LeftMainContentView>();
        containerRegistry.RegisterForNavigation<AboutView>();
        containerRegistry.RegisterForNavigation<SettingView>();
        containerRegistry.RegisterForNavigation<UpdateView>();
    }

    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    public void UpdateSkin(SkinType skin)
    {
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/HandyControl;component/Themes/Skin{skin}.xaml")
        });

        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
        });
    }
}