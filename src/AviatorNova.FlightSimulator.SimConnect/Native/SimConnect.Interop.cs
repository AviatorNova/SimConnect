using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AviatorNova.FlightSimulator.SimConnect.Native;

/// <summary>
/// Modern, high-performance P/Invoke layer for SimConnect.dll using [LibraryImport] and nint.
/// </summary>
internal static partial class SimConnectInterop
{
    private const string DllName = "SimConnect.dll";

    #region Connection

    [LibraryImport(DllName, EntryPoint = "SimConnect_Open")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    internal static partial int Open(
        out nint hSimConnect,                                   // blittable handle
        [MarshalAs(UnmanagedType.LPWStr)] string szName,
        nint hWnd = default,
        uint UserEventWinMessage = 0,
        nint hEventHandle = default,
        uint ConfigIndex = 0);

    [LibraryImport(DllName, EntryPoint = "SimConnect_Close")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    internal static partial int Close(nint hSimConnect);

    #endregion

    #region Message Pump

    [LibraryImport(DllName, EntryPoint = "SimConnect_GetNextDispatch")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    internal static partial int GetNextDispatch(
        nint hSimConnect,
        out nint ppData,           // Pointer to SIMCONNECT_RECV*
        out uint pcbData);

    [LibraryImport(DllName, EntryPoint = "SimConnect_CallDispatch")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    internal static partial int CallDispatch(
        nint hSimConnect,
        nint pfcnDispatch,
        nint pContext);

    #endregion

    #region Utility

    [LibraryImport(DllName, EntryPoint = "SimConnect_GetLastSentPacketID")]
    internal static partial int GetLastSentPacketID(nint hSimConnect, out uint dwSendID);

    #endregion
}