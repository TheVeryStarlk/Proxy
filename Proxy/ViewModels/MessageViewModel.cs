using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Proxy.Models;

namespace Proxy.ViewModels;

internal sealed partial class MessageViewModel(Message message) : ObservableObject
{
    [ObservableProperty]
    private int identifier = message.Identifier;

    [ObservableProperty]
    private ReadOnlyMemory<byte> memory = message.Memory;

    [ObservableProperty]
    private string direction = message.Outgoing ? "Outgoing" : "Ingoing";
}