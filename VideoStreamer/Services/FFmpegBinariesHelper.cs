using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;

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
                    System.Diagnostics.Debug.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}");
                    Console.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}");
                    ffmpeg.RootPath = ffmpegDirectory;
                    return;
                }
                current = Directory.GetParent(current)?.FullName;
            }
        }

        //public static void RegisterFFmpegBinariesNew()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        //    {
        //        var current = Environment.CurrentDirectory;
        //        var probe = Path.Combine("FFmpeg", "bin", Environment.Is64BitProcess ? "x64" : "x86");

        //        while (current != null)
        //        {
        //            var ffmpegBinaryPath = Path.Combine(current, probe);

        //            if (Directory.Exists(ffmpegBinaryPath))
        //            {
        //                Console.WriteLine($"FFmpeg binaries found in: {ffmpegBinaryPath}");
        //                FFmpeg.AutoGen.Bindings.DynamicallyLoaded.DynamicallyLoadedBindings.LibrariesPath = ffmpegBinaryPath;
        //                return;
        //            }

        //            current = Directory.GetParent(current)?.FullName;
        //        }
        //    }
        //    else
        //        throw new NotSupportedException();
        //}
    }
}
