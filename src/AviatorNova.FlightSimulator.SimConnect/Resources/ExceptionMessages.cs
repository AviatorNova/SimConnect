namespace AviatorNova.FlightSimulator.SimConnect
{
    /// <summary>
    /// Strongly-typed exception messages for AviatorNova.SimConnect.
    /// Centralized location for all user-facing and logging exception strings.
    /// </summary>
    public static class ExceptionMessages
    {
        /// <summary>The dispatcher must be provided and cannot be null.</summary>
        public const string Dispatcher_Null = "The dispatcher must be provided and cannot be null.";

        /// <summary>Failed to connect to Microsoft Flight Simulator 2024.</summary>
        public const string SimConnect_ConnectionFailed = "Failed to connect to Microsoft Flight Simulator 2024.";

        /// <summary>Could not acquire the add-on camera slot. Another tool may already own it.</summary>
        public const string SimConnect_CameraAcquireFailed = "Could not acquire the add-on camera slot. Another tool may already own it.";

        /// <summary>This client does not currently own the add-on camera.</summary>
        public const string SimConnect_CameraNotOwned = "This client does not currently own the add-on camera.";

        /// <summary>Unsupported camera mode: {0}</summary>
        public const string CameraMode_Unsupported = "Unsupported camera mode: {0}";

        /// <summary>Operation is not valid during shutdown.</summary>
        public const string InvalidOperation_DuringShutdown = "Operation is not valid during shutdown.";
    }
}