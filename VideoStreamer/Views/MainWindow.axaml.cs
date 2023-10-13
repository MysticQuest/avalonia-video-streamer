using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MyVideoStreamer.ViewModels;

namespace MyVideoStreamer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MyVideoViewModel();
            AvaloniaXamlLoader.Load(this);
        }
    }
}
