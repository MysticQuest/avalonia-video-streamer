using MyVideoStreamer.Services;
using ReactiveUI;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace MyVideoStreamer.ViewModels
{
    internal class MyVideoViewModel : ReactiveObject
    {
        private readonly MyVideoStream _videoStream;

        public MyVideoViewModel()
        {
            _videoStream = new MyVideoStream();
        }

        public async Task StartStreamingAsync()
        {
            await _videoStream.StartStreamingAsync("http://example.com/videostream");
        }

        public PipeReader GetVideoStreamReader() => _videoStream.GetReader();
    }
}
