using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;
using Proxy.Services;

namespace Proxy.ViewModels;

internal sealed partial class ProxyViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MessageViewModel> messages = [];

    private readonly ProxyService proxyService;

    public ProxyViewModel(ProxyService proxyService)
    {
        this.proxyService = proxyService;

        WeakReferenceMessenger.Default.Register<NavigationMessage>(
            this,
            (_, _) => Messages.Clear());

        proxyService.OnMessageReceived += (_, eventArgs) =>
        {
            Messages.Add(new MessageViewModel(eventArgs.Message));
        };
    }

    [RelayCommand]
    private async Task StopAsync()
    {
        await proxyService.DisposeAsync();
        WeakReferenceMessenger.Default.Send<NavigationMessage>();
    }
}