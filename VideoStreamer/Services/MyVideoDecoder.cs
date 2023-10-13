using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoStreamer.Services
{
    internal unsafe class MyVideoDecoder
    {
        private AVFormatContext* _pFormatContext;
        private AVCodecContext* _pCodecContext;
        private AVFrame* _pFrame;
        private AVPacket* _pPacket;
        private int _videoStreamIndex = -1;

        public MyVideoDecoder()
        {
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
            _pFormatContext = ffmpeg.avformat_alloc_context();
            _pCodecContext = null;
            _pFrame = ffmpeg.av_frame_alloc();
            _pPacket = ffmpeg.av_packet_alloc();
        }

        public void DecodeVideo(PipeReader reader)
        {
            InitializeDecoder("your_video_url_here");
            DecodeAndRenderFrames();
            Cleanup();
        }

        private void InitializeDecoder(string url)
        {
            fixed (AVFormatContext** ppFormatContext = &_pFormatContext)
            {
                int ret = ffmpeg.avformat_open_input(ppFormatContext, url, null, null);
                if (ret < 0) throw new ApplicationException($"Could not open input: {GetErrorMessage(ret)}");

                ret = ffmpeg.avformat_find_stream_info(*ppFormatContext, null);
                if (ret < 0) throw new ApplicationException($"Could not find stream info: {GetErrorMessage(ret)}");
            }

            AVStream* videoStream = FindVideoStream(_pFormatContext);
            if (videoStream == null) throw new ApplicationException("Could not find video stream");

            _videoStreamIndex = videoStream->index;  // Store the video stream index

            _pCodecContext = AllocateCodecContext(videoStream);
        }

        private AVStream* FindVideoStream(AVFormatContext* pFormatContext)
        {
            for (var i = 0; i < pFormatContext->nb_streams; i++)
            {
                if (pFormatContext->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    return pFormatContext->streams[i];
                }
            }
            return null;
        }

        private AVCodecContext* AllocateCodecContext(AVStream* videoStream)
        {
            var codecId = videoStream->codecpar->codec_id;
            var pCodec = ffmpeg.avcodec_find_decoder(codecId);
            if (pCodec == null) throw new ApplicationException("Unsupported codec");

            var pCodecContext = ffmpeg.avcodec_alloc_context3(pCodec);
            if (pCodecContext == null) throw new ApplicationException("Could not allocate codec context");

            int ret = ffmpeg.avcodec_open2(pCodecContext, pCodec, null);
            if (ret < 0) throw new ApplicationException($"Could not open codec: {GetErrorMessage(ret)}");

            return pCodecContext;
        }

        private void DecodeAndRenderFrames()
        {
            int ret;
            while ((ret = ffmpeg.av_read_frame(_pFormatContext, _pPacket)) >= 0)
            {
                if (_pPacket->stream_index == _videoStreamIndex)
                {
                    ret = ffmpeg.avcodec_send_packet(_pCodecContext, _pPacket);
                    if (ret < 0)
                    {
                        Console.WriteLine($"Error sending packet to decoder: {GetErrorMessage(ret)}");
                        break;
                    }

                    while ((ret = ffmpeg.avcodec_receive_frame(_pCodecContext, _pFrame)) >= 0)
                    {
                        RenderFrame(_pFrame);
                    }

                    if (ret != ffmpeg.AVERROR(ffmpeg.EAGAIN))
                    {
                        Console.WriteLine($"Error receiving frame from decoder: {GetErrorMessage(ret)}");
                        break;
                    }
                }

                ffmpeg.av_packet_unref(_pPacket);
            }

            if (ret != ffmpeg.AVERROR_EOF)
            {
                Console.WriteLine($"Error reading frame: {GetErrorMessage(ret)}");
            }
        }

        private void RenderFrame(AVFrame* pFrame)
        {
            // Your rendering logic here
        }


        private void Cleanup()
        {
            AVFormatContext* tempFormatContext = _pFormatContext;
            ffmpeg.avformat_close_input(&tempFormatContext);
            if (_pCodecContext != null) ffmpeg.avcodec_close(_pCodecContext);

            AVFrame* tempFrame = _pFrame;
            AVPacket* tempPacket = _pPacket;

            ffmpeg.av_frame_free(&tempFrame);
            ffmpeg.av_packet_free(&tempPacket);

            _pFormatContext = tempFormatContext;
            _pFrame = tempFrame;
            _pPacket = tempPacket;
        }


        private string GetErrorMessage(int error)
        {
            byte[] buffer = new byte[1024];
            fixed (byte* pBuffer = buffer)
            {
                ffmpeg.av_strerror(error, pBuffer, (ulong)buffer.Length);
                string errorMessage = Encoding.ASCII.GetString(buffer);
                return errorMessage.TrimEnd('\0');
            }
        }
    }
}
