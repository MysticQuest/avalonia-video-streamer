using System;
using System.IO.Pipelines;
using System.Net.Http;
using System.Threading.Tasks;


namespace MyVideoStreamer.Services
{
    public class MyVideoStream
    {
        private readonly HttpClient _httpClient;
        private readonly Pipe _pipe;

        public MyVideoStream()
        {
            _httpClient = new HttpClient();
            _pipe = new Pipe();
        }

        public async Task StartStreamingAsync(string url)
        {
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var writer = _pipe.Writer;
            var buffer = writer.GetMemory(4096);

            int read;
            while ((read = await stream.ReadAsync(buffer)) != 0)
            {
                writer.Advance(read);
                var result = await writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
                buffer = writer.GetMemory(4096);
            }

            writer.Complete();
        }

        public PipeReader GetReader() => _pipe.Reader;
    }

}
