using System;
using System.IO.Pipelines;
using System.Net.Http;
using System.Threading.Tasks;


namespace MyVideoStreamer.Services
{
    internal class MyVideoStream : IDisposable
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
            HttpResponseMessage response = null;

            try
            {
                response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();

                var writer = _pipe.Writer;

                int read;
                var buffer = writer.GetMemory(4096);
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
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HTTP Error: {e.Message}");
                _pipe.Writer.Complete(e);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                _pipe.Writer.Complete(e);
            }
            finally
            {
                response?.Dispose();
            }
        }

        public PipeReader GetReader() => _pipe.Reader;

        public void Dispose()
        {
            _httpClient.Dispose();
            _pipe.Reader.Complete();
            _pipe.Writer.Complete();
        }
    }
}
