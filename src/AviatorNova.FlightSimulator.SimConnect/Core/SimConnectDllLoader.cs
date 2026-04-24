using System.Reflection;
using System.Runtime.InteropServices;

namespace AviatorNova.FlightSimulator.SimConnect.Native;

internal static class SimConnectDllLoader
{
    private static readonly string[] SearchPaths =
    [
        AppDomain.CurrentDomain.BaseDirectory,           // 1. App folder (most common)
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",

        // Common MSFS 2024 SDK locations
        // MSFS 2024
        @"C:\MSFS 2024 SDK\SimConnect SDK\lib",
        @"D:\MSFS 2024 SDK\SimConnect SDK\lib",
        @"E:\MSFS 2024 SDK\SimConnect SDK\lib",
        @"F:\MSFS 2024 SDK\SimConnect SDK\lib",
        @"G:\MSFS 2024 SDK\SimConnect SDK\lib",

        // MSFS 2020 legacy
        @"C:\MSFS SDK\SimConnect SDK\lib",
        @"D:\MSFS SDK\SimConnect SDK\lib",
        @"E:\MSFS SDK\SimConnect SDK\lib",
        @"F:\MSFS SDK\SimConnect SDK\lib",
        @"G:\MSFS SDK\SimConnect SDK\lib",
        
        // Add more if users report common locations
    ];

    private static nint _loadedHandle = nint.Zero;

    /// <summary>
    /// Tries to load SimConnect.dll and returns the handle.
    /// Throws a helpful exception with instructions if it fails.
    /// </summary>
    public static nint Load()
    {
        if (_loadedHandle != nint.Zero)
            return _loadedHandle;

        // Try direct load first (if it's already in PATH or app folder)
        if (NativeLibrary.TryLoad("SimConnect.dll", out _loadedHandle))
            return _loadedHandle;

        // Search in known paths
        foreach (var path in SearchPaths)
        {
            var fullPath = Path.Combine(path, "SimConnect.dll");
            if (File.Exists(fullPath) && NativeLibrary.TryLoad(fullPath, out _loadedHandle))
                return _loadedHandle;
        }

        // If we get here, loading failed
        throw new DllNotFoundException(
            """
            Could not find SimConnect.dll.

            Please ensure one of the following:
            1. You have installed the MSFS 2024 SDK (Developer Mode → Help → SDK Installer)
            2. Copy SimConnect.dll from:
               C:\MSFS 2024 SDK\SimConnect SDK\lib\
               into the same folder as AviatorNova.exe

            The library searched common locations but could not locate it.
            """);
    }

    public static void Unload()
    {
        if (_loadedHandle != nint.Zero)
        {
            NativeLibrary.Free(_loadedHandle);
            _loadedHandle = nint.Zero;
        }
    }
}