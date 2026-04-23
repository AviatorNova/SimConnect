using System.Runtime.InteropServices;

namespace AviatorNova.FlightSimulator.SimConnect.Native;

[StructLayout(LayoutKind.Sequential)]
internal struct SIMCONNECT_RECV
{
    public uint dwSize;
    public uint dwVersion;
    public uint dwID;           // SIMCONNECT_RECV_ID enum
}