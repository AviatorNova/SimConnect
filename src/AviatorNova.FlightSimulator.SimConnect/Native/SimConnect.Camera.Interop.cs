using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace AviatorNova.FlightSimulator.SimConnect
{
    /// <summary>
    /// Modern P/Invoke declarations for the Microsoft Flight Simulator 2024
    /// SimConnect Camera API using <see cref="LibraryImportAttribute"/> (source-generated).
    /// All types are blittable for maximum performance and AOT compatibility.
    /// </summary>
    public static partial class SimConnectCameraInterop
    {
        private const string LibraryName = "SimConnect.dll";

        #region Modern Add-on Camera API (Recommended)

        /// <summary>
        /// Requests ownership of the dedicated add-on camera slot.
        /// Only one client can own the add-on camera at any given time.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraAcquire")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraAcquire(nint hSimConnect);

        /// <summary>
        /// Releases ownership of the add-on camera back to the simulator.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraRelease")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraRelease(nint hSimConnect);

        /// <summary>
        /// Sets position, orientation, FOV and other properties of the acquired add-on camera.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraSet")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraSet(nint hSimConnect, nint pCameraData, uint dwMask);

        /// <summary>
        /// Retrieves the current settings of the add-on camera.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraGet")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraGet(nint hSimConnect, nint pCameraData);

        /// <summary>
        /// Gets the current availability and ownership status of the add-on camera.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraGetStatus")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraGetStatus(nint hSimConnect, out SIMCONNECT_CAMERA_AVAILABILITY pStatus);

        #endregion

        #region Legacy Camera Functions (Useful for initial Player One prototyping)

        /// <summary>
        /// Legacy 6DOF method — fast and reliable for early Chase/Tower testing.
        /// Sets relative offset and orientation from the current view.
        /// </summary>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraSetRelative6DOF")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraSetRelative6DOF(
            nint hSimConnect,
            float fDeltaX,
            float fDeltaY,
            float fDeltaZ,
            float fPitch,
            float fBank,
            float fHeading);

        #endregion

        #region Camera Flag Control

        /// <summary>
        /// Enables a specific camera feature or behavior.
        /// </summary>
        /// <param name="hSimConnect">SimConnect handle.</param>
        /// <param name="eFlag">The camera flag to enable.</param>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraEnableFlag")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraEnableFlag(nint hSimConnect, SIMCONNECT_CAMERA_FLAG eFlag);

        /// <summary>
        /// Disables a specific camera feature or behavior.
        /// </summary>
        /// <param name="hSimConnect">SimConnect handle.</param>
        /// <param name="eFlag">The camera flag to disable.</param>
        [LibraryImport(LibraryName, EntryPoint = "SimConnect_CameraDisableFlag")]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
        public static partial int CameraDisableFlag(nint hSimConnect, SIMCONNECT_CAMERA_FLAG eFlag);

        #endregion
    }

    /// <summary>
    /// Availability and ownership state of the add-on camera slot.
    /// </summary>
    public enum SIMCONNECT_CAMERA_AVAILABILITY : int
    {
        /// <summary>Camera slot is currently free and available to be acquired.</summary>
        SIMCONNECT_CAMERA_NOT_ACQUIRED = 0,

        /// <summary>This client has successfully acquired the camera.</summary>
        SIMCONNECT_CAMERA_ACQUIRED = 1,

        /// <summary>Another add-on or WASM module currently owns the camera.</summary>
        SIMCONNECT_CAMERA_ACQUIRED_BY_OTHER = 2,

        /// <summary>User or simulator has disabled the add-on camera feature.</summary>
        SIMCONNECT_CAMERA_USER_DISABLED = 3
    }

    /// <summary>
    /// Flags for enabling/disabling specific camera behaviors and features.
    /// </summary>
    [Flags]
    public enum SIMCONNECT_CAMERA_FLAG : uint
    {
        /// <summary>No flags enabled.</summary>
        None = 0,

        // TODO: Populate with actual SDK flag values (collision, terrain following, damping, etc.)
        // as they become available in the official SDK documentation.
    }
}