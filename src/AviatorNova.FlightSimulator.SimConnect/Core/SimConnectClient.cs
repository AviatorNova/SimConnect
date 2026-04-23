using AviatorNova.FlightSimulator.SimConnect.Threading;
using System.Threading.Channels;

namespace AviatorNova.FlightSimulator.SimConnect;

/// <summary>
/// Main SimConnect client for AviatorNova.
/// </summary>
public sealed class SimConnectClient : IAsyncDisposable
{
    public string Name { get; }
    public DispatchStyle DispatchStyle { get; }
    public IDispatcher Dispatcher { get; }
    public bool AutoReconnect { get; }

    // Events (will be marshaled to UI thread automatically)
    public event EventHandler? Connected;
    public event EventHandler<Exception>? ExceptionOccurred;

    private readonly Channel<object> _commandChannel = Channel.CreateUnbounded<object>();
    private readonly IDispatcher _dispatcher;
    private CancellationTokenSource? _cts;
    private Task? _receivePumpTask;

    internal SimConnectClient(string name, DispatchStyle dispatchStyle,
                             IDispatcher dispatcher, bool autoReconnect)
    {
        Name = name;
        DispatchStyle = dispatchStyle;
        _dispatcher = dispatcher;
        AutoReconnect = autoReconnect;
    }

    internal async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _receivePumpTask = DispatchStyle == DispatchStyle.Polling
            ? RunPollingPumpAsync(_cts.Token)
            : RunCallbackPumpAsync(_cts.Token);

        // Notify UI that we're initializing
        _dispatcher.Post(() => Connected?.Invoke(this, EventArgs.Empty));
    }

    private async Task RunPollingPumpAsync(CancellationToken ct)
    {
        // Placeholder for now - we'll implement real SimConnect_GetNextDispatch later
        try
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(16, ct); // ~60 Hz placeholder
                // Real pump logic will go here in next step
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _dispatcher.Post(() => ExceptionOccurred?.Invoke(this, ex));
        }
    }

    private async Task RunCallbackPumpAsync(CancellationToken ct)
    {
        // Placeholder for CallDispatch style
        try
        {
            await Task.CompletedTask;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _dispatcher.Post(() => ExceptionOccurred?.Invoke(this, ex));
        }
    }

    public async ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        if (_receivePumpTask != null)
        {
            await _receivePumpTask.ConfigureAwait(false);
        }
        _cts?.Dispose();
    }
}