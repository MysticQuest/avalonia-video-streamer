using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using MyVideoStreamer.ViewModels;

namespace MyVideoStreamer.Views
{
    public partial class MyVideoView : ReactiveUserControl<MyVideoViewModel>
    {
        public MyVideoView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
