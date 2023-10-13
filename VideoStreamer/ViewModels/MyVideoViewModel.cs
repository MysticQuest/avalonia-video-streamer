using MyVideoStreamer.Services;
using ReactiveUI;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace MyVideoStreamer.ViewModels
{
    internal class MyVideoViewModel : ReactiveObject
    {
        private readonly MyVideoStream _videoStream;
        private WriteableBitmap _currentFrame;
        public WriteableBitmap CurrentFrame
        {
            get => _currentFrame;
            set => this.RaiseAndSetIfChanged(ref _currentFrame, value);
        }

        public MyVideoViewModel()
        {
            _videoStream = new MyVideoStream();
        }

        public void UpdateFrame(WriteableBitmap frame)
        {
            CurrentFrame = frame;
        }

        public async Task StartStreamingAsync()
        {
            await _videoStream.StartStreamingAsync("http://example.com/videostream");
        }

        public PipeReader GetVideoStreamReader() => _videoStream.GetReader();
    }
}
