using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamer.ViewModels
{
    internal class VideoViewModel : ReactiveObject
    {
        private readonly VideoStream _videoStream;

        public ReactiveCommand<Unit, Unit> StartStreamingCommand { get; }

        public VideoViewModel()
        {
            _videoStream = new VideoStream();
            StartStreamingCommand = ReactiveCommand.CreateFromTask(StartStreamingAsync);
        }

        private async Task StartStreamingAsync()
        {
            await _videoStream.StartStreamingAsync("http://example.com/videostream");
        }

        public PipeReader GetVideoStreamReader() => _videoStream.GetReader();
    }
}