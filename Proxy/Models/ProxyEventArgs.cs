using System;

namespace Proxy.Models;

internal sealed class ProxyEventArgs(Message message) : EventArgs
{
    public Message Message => message;
}