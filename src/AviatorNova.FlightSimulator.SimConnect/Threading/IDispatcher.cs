using Microsoft.UI.Dispatching;

namespace AviatorNova.FlightSimulator.SimConnect.Threading;

/// <summary>
/// Thin abstraction over DispatcherQueue for safe UI marshaling.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// True if the current thread is the UI thread.
    /// </summary>
    bool HasThreadAccess { get; }

    /// <summary>
    /// Post an action to run on the UI thread.
    /// </summary>
    void Post(Action action);

    /// <summary>
    /// Post an action with explicit priority.
    /// </summary>
    void Post(DispatcherQueuePriority priority, Action action);
}