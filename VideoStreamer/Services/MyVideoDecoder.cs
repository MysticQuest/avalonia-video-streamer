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
        private readonly AVFormatContext* _pFormatContext;
        private AVCodecContext* _pCodecContext;
        private readonly AVFrame* _pFrame;
        private readonly AVPacket* _pPacket;

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
            AVFormatContext* pFormatContext = null;
            AVCodecContext* pCodecContext = null;
            AVFrame* pFrame = null;
            AVPacket* pPacket = null;

            try
            {
                int ret = ffmpeg.avformat_open_input(&pFormatContext, "your_video_url_here", null, null);
                if (ret < 0) throw new ApplicationException($"Could not open input: {GetErrorMessage(ret)}");

                ret = ffmpeg.avformat_find_stream_info(pFormatContext, null);
                if (ret < 0) throw new ApplicationException($"Could not find stream info: {GetErrorMessage(ret)}");

                AVStream* videoStream = null;
                for (var i = 0; i < pFormatContext->nb_streams; i++)
                {
                    if (pFormatContext->streams[i]->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                    {
                        videoStream = pFormatContext->streams[i];
                        break;
                    }
                }

                if (videoStream == null) throw new ApplicationException("Could not find video stream");

                var codecId = videoStream->codecpar->codec_id;
                var pCodec = ffmpeg.avcodec_find_decoder(codecId);
                if (pCodec == null) throw new ApplicationException("Unsupported codec");

                pCodecContext = ffmpeg.avcodec_alloc_context3(pCodec);
                if (pCodecContext == null) throw new ApplicationException("Could not allocate codec context");

                ret = ffmpeg.avcodec_open2(pCodecContext, pCodec, null);
                if (ret < 0) throw new ApplicationException($"Could not open codec: {GetErrorMessage(ret)}");

            }
            finally
            {
                ffmpeg.avformat_close_input(&pFormatContext);
                if (pCodecContext != null) ffmpeg.avcodec_close(pCodecContext);
                ffmpeg.av_frame_free(&pFrame);
                ffmpeg.av_packet_free(&pPacket);
            }
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
