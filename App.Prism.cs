using System;

using Microsoft.UI.Xaml;


using Weather.History.IoC;
using Weather.History.Mvvm.Views;
using Weather.History.Mvvm.ViewModels;
using Weather.History.Log;
using Weather.History.Prism;
using Weather.History.Abstract;
using Weather.History.AppService;
using Weather.History.Events;
using Weather.History.Entity.Events;

#if WINDOWS
using System.Diagnostics;

using Weather.History.Windows;

using Windows.ApplicationModel;
#endif

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.DryIoc;
using Prism.Events;


namespace Weather.History
{
    public sealed partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HeadlessIocSetup>();
            moduleCatalog.AddModule<HeadIocSetup>();

            var moduleManager = Container.Resolve<IModuleManager>();

            moduleManager.LoadModuleCompleted += OnModuleLoaded;

            moduleManager.Run();
        }

        private void OnModuleLoaded(object sender, LoadModuleCompletedEventArgs e)
        {
            if (e.Error is null)
            {
                Log4.Debug($"Module loaded : {e.ModuleInfo.ModuleName} [{e.ModuleInfo.State}]");
            }
            else
            {
                Log4.Error(e.Error);
            }
        }

        protected override UIElement CreateShell()
        {
           var shell = Container.Resolve<Shell>();

#if WINDOWS
            shell.Loaded += (s, e) =>
            {
                MainXamlRoot = (s as UIElement).XamlRoot;
            };
#endif
            return (UIElement)shell;
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
            var settingsProvider = Container.Resolve<ISettingsProvider>();

            SwitchTheme(settingsProvider.ThemeMode);
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
