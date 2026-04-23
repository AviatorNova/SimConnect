using AviatorNova.FlightSimulator.SimConnect.Threading;
using Microsoft.UI.Dispatching;

namespace AviatorNova.FlightSimulator.SimConnect;

public sealed class SimConnectClientBuilder
{
    private string _name = "AviatorNova";
    private DispatchStyle _dispatchStyle = DispatchStyle.Polling;
    private IDispatcher? _dispatcher;
    private bool _autoReconnect = true;

    public SimConnectClientBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SimConnectClientBuilder UseDispatchStyle(DispatchStyle style)
    {
        _dispatchStyle = style;
        return this;
    }

    public SimConnectClientBuilder WithDispatcherQueue(DispatcherQueue queue)
    {
        _dispatcher = new DispatcherQueueAdapter(queue);
        return this;
    }

    public SimConnectClientBuilder WithAutoReconnect(bool enabled = true)
    {
        _autoReconnect = enabled;
        return this;
    }

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