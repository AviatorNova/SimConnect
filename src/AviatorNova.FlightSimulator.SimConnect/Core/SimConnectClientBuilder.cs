using AviatorNova.FlightSimulator.SimConnect.Threading;
using Microsoft.UI.Dispatching;

namespace AviatorNova.FlightSimulator.SimConnect;

/// <summary>
/// Builder for creating and configuring a <see cref="SimConnectClient"/>.
/// </summary>
public sealed class SimConnectClientBuilder
{
    private string _name = "SimConnect";
    private DispatchStyle _dispatchStyle = DispatchStyle.Polling;
    /// <summary>The dispatcher used to marshal SimConnect callbacks.</summary>
    public required IDispatcher _dispatcher;
    private bool _autoReconnect = true;

    /// <summary>Sets the application name sent to SimConnect.</summary>
    public SimConnectClientBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
        return this;
    }

    /// <summary>Sets the dispatch style used to process SimConnect messages.</summary>
    public SimConnectClientBuilder UseDispatchStyle(DispatchStyle style)
    {
        _dispatchStyle = style;
        return this;
    }

    /// <summary>Configures the builder to use a <see cref="DispatcherQueue"/> for dispatching callbacks.</summary>
    public SimConnectClientBuilder WithDispatcherQueue(DispatcherQueue queue)
    {
        _dispatcher = new DispatcherQueueAdapter(queue);
        return this;
    }

    /// <summary>Enables or disables automatic reconnection on connection loss.</summary>
    public SimConnectClientBuilder WithAutoReconnect(bool enabled = true)
    {
        _autoReconnect = enabled;
        return this;
    }

    /// <summary>Builds and initializes the <see cref="SimConnectClient"/>.</summary>
    public async Task<SimConnectClient> BuildAsync(CancellationToken cancellationToken = default)
    {
        if (_dispatcher is null)
            throw new InvalidOperationException(ExceptionMessages.Dispatcher_Null);

        // This style satisfies 'required' properties cleanly
        var client = new SimConnectClient(
            name: _name,
            dispatchStyle: _dispatchStyle,
            dispatcher: _dispatcher,
            autoReconnect: _autoReconnect)
        {
            Name = _name,
            Dispatcher = _dispatcher
        };

        await client.InitializeAsync(cancellationToken);
        return client;
    }
}