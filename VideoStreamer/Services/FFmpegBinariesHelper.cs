using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;

namespace MyVideoStreamer.Services
{
    public static class FFmpegBinariesHelper
    {
        public static void RegisterFFmpegBinaries()
        {
            var current = Environment.CurrentDirectory;
            var architecture = RuntimeInformation.ProcessArchitecture.ToString();
            var osFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ffmpeg-6.0-full_build" : $"ffmpeg-6.0-{architecture}-static";
            var binaryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ffmpeg.exe" : "ffmpeg";

            while (current != null)
            {
                var ffmpegDirectoryBase = Path.Combine(current, "Res", osFolder);
                var ffmpegDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Path.Combine(ffmpegDirectoryBase, "bin") : ffmpegDirectoryBase;
                var ffmpegBinaryPath = Path.Combine(ffmpegDirectory, binaryName);

                if (Directory.Exists(ffmpegDirectory) && File.Exists(ffmpegBinaryPath))
                {
                    Console.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}");
                    RegisterFFmpegBinariesPath(ffmpegDirectory);
                    return;
                }
                current = Directory.GetParent(current)?.FullName;
            }
        }

        private static void RegisterFFmpegBinariesPath(string path)
        {
            var current = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            var newPath = $"{path}{Path.PathSeparator}{current}";
            Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Process);
        }
    }
}
