using System;

namespace AviatorNova.FlightSimulator.SimConnect.Threading;

/// <summary>Specifies the dispatch style used by the SimConnect client.</summary>
public enum DispatchStyle
{
    /// <summary>Messages are polled at a regular interval.</summary>
    Polling,
    /// <summary>Messages are received via event-driven callbacks.</summary>
    EventDriven
}
