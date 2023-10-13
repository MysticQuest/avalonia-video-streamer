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
            BuildAvaloniaApp().Start(AppMain, args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<MyVideoStreamerApp>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();

        private static void AppMain(Application app, string[] args)
        {
            FFmpegBinariesHelper.RegisterFFmpegBinaries(); 

            if (app.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var mainWindow = new MainWindow();
                mainWindow.InitializeAsync().GetAwaiter().GetResult();
                desktopLifetime.MainWindow = mainWindow;
            }
        }
    }
}
