using AviatorNova.FlightSimulator.SimConnect.Threading;
using Microsoft.UI.Dispatching;

namespace AviatorNova.FlightSimulator.SimConnect;

/// <summary>Builder for creating and configuring a <see cref="SimConnectClient"/>.</summary>
public sealed class SimConnectClientBuilder
{
    private string _name = "AviatorNova";
    private DispatchStyle _dispatchStyle = DispatchStyle.Polling;
    private IDispatcher? _dispatcher;
    private bool _autoReconnect = true;

    /// <summary>Sets the name of the client.</summary>
    public SimConnectClientBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    /// <summary>Sets the dispatch style used by the client.</summary>
    public SimConnectClientBuilder UseDispatchStyle(DispatchStyle style)
    {
        _dispatchStyle = style;
        return this;
    }

    /// <summary>Sets the dispatcher queue used for marshaling events.</summary>
    public SimConnectClientBuilder WithDispatcherQueue(DispatcherQueue queue)
    {
        _dispatcher = new DispatcherQueueAdapter(queue);
        return this;
    }

    /// <summary>Enables or disables automatic reconnection.</summary>
    public SimConnectClientBuilder WithAutoReconnect(bool enabled = true)
    {
        _autoReconnect = enabled;
        return this;
    }

    /// <summary>Builds and initializes the <see cref="SimConnectClient"/>.</summary>
    public async Task<SimConnectClient> BuildAsync(CancellationToken cancellationToken = default)
    {
        var client = new SimConnectClient(
            _name,
            _dispatchStyle,
            _dispatcher ?? new DispatcherQueueAdapter(),
            _autoReconnect);

        await client.InitializeAsync(cancellationToken);
        return client;
    }
}