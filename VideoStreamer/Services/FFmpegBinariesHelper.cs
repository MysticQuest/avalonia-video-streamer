using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.AutoGen;
using FFmpeg.AutoGen.Bindings.DynamicallyLinked;
using FFmpeg.AutoGen.Bindings.DynamicallyLoaded;

namespace MyVideoStreamer.Services
{
    public static class FFmpegBinariesHelper
    {
        //public static void RegisterFFmpegBinaries()
        //{
        //    var current = Environment.CurrentDirectory;
        //    var architecture = RuntimeInformation.ProcessArchitecture.ToString();
        //    var osFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ffmpeg-6.0-full_build" : $"ffmpeg-6.0-{architecture}-static";
        //    var binaryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "ffmpeg.exe" : "ffmpeg";

        //    while (current != null)
        //    {
        //        var ffmpegDirectoryBase = Path.Combine(current, "Res", osFolder);
        //        var ffmpegDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Path.Combine(ffmpegDirectoryBase, "bin") : ffmpegDirectoryBase;
        //        var ffmpegBinaryPath = Path.Combine(ffmpegDirectory, binaryName);


        //        Console.WriteLine($"Checking directory: {ffmpegDirectory}");
        //        Console.WriteLine($"Checking binary path: {ffmpegBinaryPath}");
        //        System.Diagnostics.Debug.WriteLine($"\nChecking directory: {ffmpegDirectory}");
        //        System.Diagnostics.Debug.WriteLine($"Checking binary path: {ffmpegBinaryPath}");

        //        if (Directory.Exists(ffmpegDirectory))
        //        {
        //            Console.WriteLine($"Directory exists: {ffmpegDirectory}");
        //            System.Diagnostics.Debug.WriteLine($"Directory exists: {ffmpegDirectory}");
        //        }
        //        if (File.Exists(ffmpegBinaryPath))
        //        {
        //            Console.WriteLine($"Binary exists: {ffmpegBinaryPath}");
        //            System.Diagnostics.Debug.WriteLine($"Binary exists: {ffmpegBinaryPath}");
        //        }

        //        if (Directory.Exists(ffmpegDirectory) && File.Exists(ffmpegBinaryPath))
        //        {
        //            System.Diagnostics.Debug.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}\n");
        //            Console.WriteLine($"FFmpeg binaries found in: {ffmpegDirectory}");
        //            FFmpeg.AutoGen.Bindings.DynamicallyLoaded.DynamicallyLoadedBindings.LibrariesPath = ffmpegBinaryPath;
        //            ffmpeg.RootPath = ffmpegDirectory;
        //            return;
        //        }
        //        current = Directory.GetParent(current)?.FullName;
        //    }
        //}

        public static void RegisterFFmpegBinaries()
        {
            try
            {
                Debug.WriteLine("\nRegistering FFmpeg binaries...");

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Debug.WriteLine("OS Platform: Windows");

                    var current = Environment.CurrentDirectory;
                    var probe = Path.Combine("FFmpeg", "bin", Environment.Is64BitProcess ? "x64" : "x86");

                    Debug.WriteLine($"Current directory: {current}");
                    Debug.WriteLine($"Probe path: {probe}");

                    while (current != null)
                    {
                        var ffmpegBinaryPath = Path.Combine(current, probe);

                        Debug.WriteLine($"Checking FFmpeg binaries at: {ffmpegBinaryPath}");

                        if (Directory.Exists(ffmpegBinaryPath))
                        {
                            Console.WriteLine($"FFmpeg binaries found in: {ffmpegBinaryPath}");
                            Debug.WriteLine($"FFmpeg binaries found in: {ffmpegBinaryPath}");

                            FFmpeg.AutoGen.Bindings.DynamicallyLoaded.DynamicallyLoadedBindings.LibrariesPath = ffmpegBinaryPath;
                            return;
                        }

                        current = Directory.GetParent(current)?.FullName;
                    }
                }
                else
                {
                    Debug.WriteLine("OS Platform: Not Windows");
                    throw new NotSupportedException();
                }
            }
            catch (Exception ex)
            {
                // Exception Handling and Debugging
                Debug.WriteLine($"An error occurred: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
