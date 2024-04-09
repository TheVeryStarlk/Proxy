using System;

namespace Proxy.Models;

internal sealed record Message(int Identifier, ReadOnlyMemory<byte> Memory, bool Outgoing);