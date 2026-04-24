using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AviatorNova.FlightSimulator.SimConnect
{
    /// <summary>
    /// Modern P/Invoke declarations for the core SimConnect Connection API
    /// using <see cref="LibraryImportAttribute"/> (source-generated, .NET 7+ best practices).
    /// All types are blittable for maximum performance and AOT compatibility.
    /// </summary>
    public static partial class SimConnectConnectionInterop
    {
        private const string LibraryName = "SimConnect.dll";

        #region Core Connection Functions

        /// <summary>
        /// Opens a connection to Microsoft Flight Simulator 2024.
        /// </summary>
        /// <param name="phSimConnect">Receives the SimConnect handle on success.</param>
        /// <param name="szAppName">Name of your application (appears in SimConnect Inspector).</param>
        /// <param name="hWnd">Window handle for message pump (use your WinUI Window HWND).</param>
        /// <param name="uMsg">Custom Windows message ID (commonly 0x0402).</param>
        /// <param name="hEvent">Event handle (usually Zero for callback style).</param>
        /// <param name="dwConfigIndex">Configuration index (usually SIMCONNECT_OPEN_CONFIGINDEX_LOCAL).</param>
        /// <returns>HRESULT. S_OK on success.</returns>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_Open")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int Open(
            out nint phSimConnect,
            [MarshalAs(UnmanagedType.LPStr)] string szAppName,
            nint hWnd,
            uint uMsg,
            nint hEvent,
            uint dwConfigIndex);

        /// <summary>
        /// Closes an existing SimConnect connection.
        /// </summary>
        /// <param name="hSimConnect">SimConnect handle returned from Open.</param>
        /// <returns>HRESULT.</returns>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_Close")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int Close(nint hSimConnect);

        /// <summary>
        /// Dispatches all pending messages from SimConnect. Call this regularly (usually in your message loop).
        /// </summary>
        /// <param name="hSimConnect">SimConnect handle.</param>
        /// <param name="pDispatchProc">Pointer to your dispatch callback function.</param>
        /// <param name="pContext">User context pointer (usually Zero or your service instance).</param>
        /// <returns>HRESULT.</returns>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CallDispatch")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CallDispatch(nint hSimConnect, nint pDispatchProc, nint pContext);

        #endregion

        #region Utility & Status Functions

        /// <summary>
        /// Returns the ID of the last packet sent to SimConnect (useful for debugging).
        /// </summary>
        /// <param name="hSimConnect">SimConnect handle.</param>
        /// <param name="dwPacketID">Receives the ID of the last packet sent to the simulator.</param>
        /// <returns>HRESULT.</returns>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_GetLastSentPacketID")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int GetLastSentPacketID(nint hSimConnect, out uint dwPacketID);

        #endregion
    }

    /// <summary>
    /// Configuration index values for SimConnect_Open.
    /// </summary>
    public enum SimConnectOpenConfigIndex : uint
    {
        /// <summary>Use default local connection (most common).</summary>
        SIMCONNECT_OPEN_CONFIGINDEX_LOCAL = 0,

        /// <summary>Reserved / future use.</summary>
        SIMCONNECT_OPEN_CONFIGINDEX_REMOTE = 1
    }

    /// <summary>
    /// Common HRESULT return codes from SimConnect functions.
    /// </summary>
    public enum SimConnectResult : int
    {
        /// <summary>Operation completed successfully.</summary>
        S_OK = 0,

        /// <summary>Unspecified failure. Check SimConnect logs or call <see cref="SimConnectConnectionInterop.GetLastSentPacketID"/> for diagnostics.</summary>
        E_FAIL = unchecked((int)0x80004005),
        // Add more as needed from SDK
    }
}