using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MyVideoStreamer.Views;
using MyVideoStreamer.Services;
using MyVideoStreamer;

namespace MyVideoStreamer.Desktop
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static AppBuilder BuildAvaloniaApp()
          => AppBuilder.Configure<MyVideoStreamerApp>()
              .UsePlatformDetect()
              .WithInterFont()
              .LogToTrace()
              .UseReactiveUI();

        private static void AppMain(Application app, string[] args)
        {
            try
            {
                FFmpegBinariesHelper.RegisterFFmpegBinaries();

                if (app.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.InitializeAsync().GetAwaiter().GetResult();
                    desktopLifetime.MainWindow = mainWindow;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
