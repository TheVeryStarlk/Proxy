using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Proxy.Models;

namespace Proxy.Services;

internal sealed class ProxyService : IAsyncDisposable
{
    public event EventHandler<ProxyEventArgs>? OnMessageReceived;

    public event EventHandler? OnStopped;

    private Socket? avalonia;
    private Socket? listener;
    private Socket? minecraft;
    private bool running;

    public async Task StartAsync(IPEndPoint endPoint, ushort listening)
    {
        if (running)
        {
            throw new InvalidOperationException("Already connected.");
        }

        running = true;

        avalonia = CreateSocket(endPoint);

        await avalonia.ConnectAsync(endPoint);

        listener = CreateSocket(endPoint);

        listener.Bind(new IPEndPoint(endPoint.Address, listening));
        listener.Listen();

        minecraft = await listener.AcceptAsync();

        _ = Task.Run(async () =>
        {
            await using var destination = new NetworkStream(avalonia);
            await using var source = new NetworkStream(minecraft);

            await Task.WhenAny(
                ForwardAsync(destination, source, false),
                ForwardAsync(source, destination, true));
        });
    }

    public ValueTask DisposeAsync()
    {
        if (running)
        {
            avalonia?.Dispose();
            listener?.Dispose();
            minecraft?.Dispose();

            running = false;

            OnStopped?.Invoke(this, EventArgs.Empty);
        }

        return ValueTask.CompletedTask;
    }

    private static Socket CreateSocket(EndPoint endPoint)
    {
        return new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    private static int ReadVariableInteger(Stream stream)
    {
        var numRead = 0;
        var result = 0;
        byte read;

        do
        {
            read = (byte) stream.ReadByte();
            var value = read & 0b01111111;
            result |= value << 7 * numRead;

            numRead++;

            if (numRead > 5)
            {
                throw new InvalidOperationException("Variable integer is too big.");
            }
        } while ((read & 0b10000000) != 0);

        return result;
    }

    private async Task ForwardAsync(NetworkStream source, NetworkStream destination, bool outgoing)
    {
        while (true)
        {
            try
            {
                var buffer = new byte[16_777_216];
                buffer = buffer[..await source.ReadAsync(buffer)];
                await destination.WriteAsync(buffer);

                using var header = new MemoryStream(buffer);

                _ = ReadVariableInteger(header);
                var identifier = ReadVariableInteger(header);

                OnMessageReceived?.Invoke(
                    this,
                    new ProxyEventArgs(new Message(
                        identifier,
                        buffer.AsMemory()[(int) header.Position..],
                        outgoing)));
            }
            catch
            {
                break;
            }
        }

        await DisposeAsync();
    }
}