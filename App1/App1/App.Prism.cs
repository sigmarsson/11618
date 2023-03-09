using System;

using Microsoft.UI.Xaml;



#if WINDOWS
using System.Diagnostics;

using Weather.History.Windows;

using Windows.ApplicationModel;
#endif

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.DryIoc;


namespace Weather.History
{
    public sealed partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        
        }



        protected override UIElement CreateShell()
        {
#if WINDOWS
            shell.Loaded += (s, e) =>
            {
                MainXamlRoot = (s as UIElement).XamlRoot;
            };
#endif
            return null;
        }

#if WINDOWS
        protected override void InitializeShell(UIElement shell)
        {

            Window = new Window
            {
                ExtendsContentIntoTitleBar = true
            };

            Window.Activate();
            Window.Content = shell;
			
			Win32Ex.UpdateTitleBar(Window.Content);

        }
#endif

        protected override void OnInitialized()
        {
#if WINDOWS
            var proc = Process.GetCurrentProcess();
            var hwnd = proc.MainWindowHandle;
            var geoService = Container.Resolve<IGeoService>();
            var appService = new AppServiceHost(hwnd, geoService);

            appService.Start();
#endif
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }
    }
}
