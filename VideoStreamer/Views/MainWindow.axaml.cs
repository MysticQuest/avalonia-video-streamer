using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MyVideoStreamer.ViewModels;
using System.Threading.Tasks;

namespace MyVideoStreamer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async Task InitializeAsync()
        {
            await InitializeStreaming();
        }

        private async Task InitializeStreaming()
        {
            var viewModel = new MyVideoViewModel();
            await viewModel.StartStreamingAsync();
        }
    }
}
