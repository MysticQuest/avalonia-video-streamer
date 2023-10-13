using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;

namespace VideoStreamer.Desktop
{
    internal static class FFmpegBinariesHelper
    {
        public static void RegisterFFmpegBinaries()
        {
            var current = Environment.CurrentDirectory;
            var probe = Path.Combine(current, "Res", RuntimeInformation.ProcessArchitecture.ToString());
            while (current != null)
            {
                var ffmpegDirectory = Path.Combine(probe, "ffmpeg");
                if (Directory.Exists(ffmpegDirectory))
                {
                    Console.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}");
                    RegisterFFmpegBinaries(ffmpegDirectory);
                    return;
                }
                current = Directory.GetParent(current)?.FullName;
            }
        }

        private static void RegisterFFmpegBinaries(string path)
        {
            var current = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            var newPath = $"{path}{Path.PathSeparator}{current}";
            Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Process);
        }
    }
}
