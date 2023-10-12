using System;
using System.IO.Pipelines;
using System.Net.Http;
using System.Threading.Tasks;

public class VideoStream
{
    private readonly HttpClient _httpClient;
    private readonly Pipe _pipe;

    public VideoStream()
    {
        _httpClient = new HttpClient();
        _pipe = new Pipe();
    }

    public PipeReader GetReader() => _pipe.Reader;
}
