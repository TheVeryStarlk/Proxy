using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Proxy.Models;

namespace Proxy.Services;

internal sealed class ProxyService : IAsyncDisposable
{
    public event EventHandler<ProxyEventArgs>? OnMessageReceived;

    private Socket? avalonia;
    private Socket? listener;
    private Socket? minecraft;

    public async Task StartAsync(IPEndPoint endPoint, ushort listening)
    {
        if (avalonia is not null && listener is not null && minecraft is not null)
        {
            throw new InvalidOperationException("Already connected.");
        }

        avalonia = CreateSocket(endPoint);

        await avalonia.ConnectAsync(endPoint);

        listener = CreateSocket(endPoint);

        listener.Bind(new IPEndPoint(endPoint.Address, listening));
        listener.Listen();

        minecraft = await listener.AcceptAsync();

        _ = Task.Run(async () =>
        {
            try
            {
                await using var destination = new NetworkStream(avalonia);
                await using var source = new NetworkStream(minecraft);

                await Task.WhenAny(
                    ForwardAsync(destination, source),
                    ForwardAsync(source, destination));
            }
            catch
            {
                // Definitely should log.
            }
        });
    }

    public ValueTask DisposeAsync()
    {
        if (avalonia is not null && listener is not null && minecraft is not null)
        {
            avalonia.Dispose();
            listener.Dispose();
            minecraft.Dispose();

            avalonia = null;
            listener = null;
            minecraft = null;
        }

        return ValueTask.CompletedTask;
    }

    private static Socket CreateSocket(EndPoint endPoint)
    {
        return new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    private async Task ForwardAsync(NetworkStream source, NetworkStream destination)
    {
        while (true)
        {
            try
            {
                var buffer = new byte[16_777_216];
                buffer = buffer[..await source.ReadAsync(buffer)];
                await destination.WriteAsync(buffer);

                OnMessageReceived?.Invoke(
                    this,
                    new ProxyEventArgs(new Message(buffer[0], buffer)));
            }
            catch
            {
                break;
            }
        }
    }
}