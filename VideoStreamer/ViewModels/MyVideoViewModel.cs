using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MyVideoStreamer.Services;

namespace MyVideoStreamer.ViewModels
{
    public class MyVideoViewModel : ReactiveObject
    {
        private readonly MyVideoStream _videoStream;

        public ReactiveCommand<Unit, Unit> StartStreamingCommand { get; }

        public MyVideoViewModel()
        {
            _videoStream = new MyVideoStream();
            StartStreamingCommand = ReactiveCommand.CreateFromTask(StartStreamingAsync);
        }

        private async Task StartStreamingAsync()
        {
            await _videoStream.StartStreamingAsync("http://example.com/videostream");
        }

        public PipeReader GetVideoStreamReader() => _videoStream.GetReader();
    }
}