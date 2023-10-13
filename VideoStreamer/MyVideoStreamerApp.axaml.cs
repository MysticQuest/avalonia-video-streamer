using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MyVideoStreamer.ViewModels;
using MyVideoStreamer.Views;

namespace MyVideoStreamer
{
    public partial class MyVideoStreamerApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MyVideoViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MyVideoView
                {
                    DataContext = new MyVideoViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}


