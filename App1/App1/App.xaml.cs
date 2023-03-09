using Microsoft.UI.Xaml;


using System.IO;
using System;

namespace Weather.History
{
    public sealed partial class App
    {
        public App()
        {
#if WINDOWS
            UnhandledException += ExceptionHandler;
#endif
            Environment.SetEnvironmentVariable(MyEnvironment.VARIABLE, AppDataLocation.Current, EnvironmentVariableTarget.Process);

            var logDirectory = Path.Combine(AppDataLocation.Current, "Log");

            Directory.CreateDirectory(logDirectory);

            Log4.Debug($"Current Working Directory : {SetCwd()}");
            Log4.Debug($"Log Directory: {logDirectory}");
            Log4.Info($"Time zone: {TimeZoneInfo.Local.Id}");

            InitializeComponent();
        }

        private string SetCwd()
        {
            try
            {
                var exe = GetType().Assembly.Location;
                var cwd = Path.GetDirectoryName(exe);

                Directory.SetCurrentDirectory(cwd);

                return cwd;
            }
            catch (Exception e)
            {
                Log4.Error(e);

                return null;
            }
        }

#if WINDOWS
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
        }
#endif
        public void ExceptionHandler(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
        }
    }
}