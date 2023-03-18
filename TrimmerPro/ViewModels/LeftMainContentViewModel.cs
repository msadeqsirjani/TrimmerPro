namespace TrimmerPro.ViewModels;

public class LeftMainContentViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private const string RegionName = "ContentRegion";

    public LeftMainContentViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;

        SwitchItemCommand = new DelegateCommand<SelectionChangedEventArgs>(Switch);
        UrlShortenerCommand = new DelegateCommand(UrlShortener);
    }

    public DelegateCommand<SelectionChangedEventArgs> SwitchItemCommand { get; }
    public DelegateCommand UrlShortenerCommand { get; }

    private void UrlShortener()
    {
        var activeView = _regionManager.Regions[RegionName].ActiveViews.FirstOrDefault();

        if (activeView != null)
        {
            var isMainShortenerView = activeView.GetType() == typeof(MainShortenerView);

            if (isMainShortenerView)
            {
                _regionManager.Regions[RegionName].RemoveAll();
            }
            else
            {
                _regionManager.RequestNavigate(RegionName, "MainShortenerView");
            }
        }
        else
        {
            _regionManager.RequestNavigate(RegionName, "MainShortenerView");
        }
    }

    private void Switch(SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
        {
            return;
        }

        if (e.AddedItems[0] is ListBoxItem {Tag: { }} item)
        {
            _regionManager.RequestNavigate(RegionName, $"{item.Tag}View");
        }
    }
}