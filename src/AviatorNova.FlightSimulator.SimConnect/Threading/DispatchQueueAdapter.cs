using Microsoft.UI.Dispatching;

namespace AviatorNova.FlightSimulator.SimConnect.Threading;

internal sealed class DispatcherQueueAdapter : IDispatcher
{
    private readonly DispatcherQueue? _queue;

    public DispatcherQueueAdapter(DispatcherQueue? queue = null)
    {
        _queue = queue ?? DispatcherQueue.GetForCurrentThread();
    }

    public bool HasThreadAccess => _queue?.HasThreadAccess ?? false;

    public void Post(Action action)
    {
        if (action is null) return;

        if (HasThreadAccess)
            action();
        else
            _queue?.TryEnqueue(DispatcherQueuePriority.Normal, () => action());
    }

    public void Post(DispatcherQueuePriority priority, Action action)
    {
        if (action is null) return;

        if (HasThreadAccess)
            action();
        else
            _queue?.TryEnqueue(priority, () => action());
    }
}