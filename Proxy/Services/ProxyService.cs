using System;
using System.Net;
using System.Threading.Tasks;
using Proxy.Models;

namespace Proxy.Services;

internal sealed class ProxyService : IAsyncDisposable
{
    public event EventHandler<ProxyEventArgs>? OnMessageReceived;

    public Task StartAsync(IPEndPoint endPoint)
    {
        // var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        // await socket.ConnectAsync(endPoint);

        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}