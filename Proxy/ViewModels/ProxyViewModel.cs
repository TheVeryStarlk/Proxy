using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;
using Proxy.Models;
using Proxy.Services;

namespace Proxy.ViewModels;

internal sealed partial class ProxyViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Message> messages = [];

    public ProxyViewModel(ProxyService proxyService)
    {
        WeakReferenceMessenger.Default.Register<NavigationMessage>(
            this,
            (_, _) => Messages.Clear());

        proxyService.OnMessageReceived += (_, eventArgs) =>
        {
            Messages.Add(eventArgs.Message);
        };
    }
}