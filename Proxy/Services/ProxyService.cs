using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Proxy.Models;

namespace Proxy.Services;

internal sealed class ProxyService : IAsyncDisposable
{
    public event EventHandler<ProxyEventArgs>? OnMessageReceived;

    public async Task StartAsync(IPEndPoint endPoint, ushort listening)
    {
        using var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await socket.ConnectAsync(endPoint);

        using var listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        listener.Bind(new IPEndPoint(endPoint.Address, listening));
        listener.Listen();

        using var client = await listener.AcceptAsync();
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}