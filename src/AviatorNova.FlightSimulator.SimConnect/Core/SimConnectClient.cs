using AviatorNova.FlightSimulator.SimConnect.Threading;
using System.Threading.Channels;

namespace AviatorNova.FlightSimulator.SimConnect;

/// <summary>
/// Main SimConnect client for AviatorNova.
/// </summary>
public sealed class SimConnectClient : IAsyncDisposable
{
    /// <summary>Gets the name of this client.</summary>
    public string Name { get; }

    /// <summary>Gets the dispatch style used by this client.</summary>
    public DispatchStyle DispatchStyle { get; }

    /// <summary>Gets the dispatcher used for marshaling events.</summary>
    public IDispatcher Dispatcher { get; }

    /// <summary>Gets a value indicating whether the client should automatically reconnect.</summary>
    public bool AutoReconnect { get; }

    // Events (will be marshaled to UI thread automatically)
    /// <summary>Occurs when the client connects to SimConnect.</summary>
    public event EventHandler? Connected;

    /// <summary>Occurs when an exception is encountered during SimConnect operations.</summary>
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
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        AutoReconnect = autoReconnect;

        // Satisfy non-nullable public properties (fixes CS8618)
        Dispatcher = dispatcher;
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

    private async Task RunCallbackPumpAsync(CancellationToken cancellationToken)
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

    /// <summary>Disposes the client and stops the receive pump.</summary>
    public async ValueTask DisposeAsync()
    {
        _cts?.Cancel();
        if (_receivePumpTask != null)
        {
            try
            {
                await _receivePumpTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected when disposal cancels the receive pump.
            }
        }
        _cts?.Dispose();
    }


}