using System.Windows;
using Prism.Ioc;
using TrimmerPro.Views;
using Window = System.Windows.Window;

namespace TrimmerPro;

public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        
    }

    protected override Window CreateShell() => Container.Resolve<MainWindow>();
}