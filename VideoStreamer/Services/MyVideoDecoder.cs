using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamer.Services
{
    internal class MyVideoDecoder
    {
        public MyVideoDecoder()
        {
            FFmpegBinariesHelper.RegisterFFmpegBinaries();
        }

        public void DecodeVideo(PipeReader reader)
        {
            // todo
        }
    }
}
