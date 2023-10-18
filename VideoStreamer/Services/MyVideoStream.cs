using Avalonia;
using System;
using System.Diagnostics;
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
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            _pipe = new Pipe();
        }

        public async Task StartStreamingAsync(string url)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var writer = _pipe.Writer;

                while (true)
                {
                    var memory = writer.GetMemory(4096);
                    var read = await stream.ReadAsync(memory).ConfigureAwait(false);

                    if (read == 0)
                    {
                        break; // Stream ended
                    }

                    writer.Advance(read);

                    var flushResult = await writer.FlushAsync().ConfigureAwait(false);

                    if (flushResult.IsCompleted)
                    {
                        break;
                    }
                }

                writer.Complete();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"HTTP Error: {e.Message}");
                _pipe.Writer.Complete(e);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
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
