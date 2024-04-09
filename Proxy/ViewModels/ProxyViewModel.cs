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

    [ObservableProperty]
    private bool scrollToBottom = true;

    [ObservableProperty]
    private MessageViewModel? selectedMessage;

    private bool isForceStop;

    private readonly ProxyService proxyService;

    public ProxyViewModel(ProxyService proxyService)
    {
        this.proxyService = proxyService;

        WeakReferenceMessenger.Default.Register<NavigationMessage>(
            this,
            (_, _) => Messages.Clear());

        proxyService.OnMessageReceived += (_, eventArgs) =>
        {
            var message = new MessageViewModel(eventArgs.Message);
            Messages.Add(message);

            if (ScrollToBottom)
            {
                SelectedMessage = message;
            }
        };

        proxyService.OnStopped += (_, _) =>
        {
            if (isForceStop)
            {
                return;
            }

            WeakReferenceMessenger.Default.Send<NavigationMessage>();
        };
    }

    [RelayCommand]
    private async Task StopAsync()
    {
        isForceStop = true;

        await proxyService.DisposeAsync();
        WeakReferenceMessenger.Default.Send<NavigationMessage>();
    }

    [RelayCommand]
    private void ClearAll()
    {
        Messages.Clear();
    }
}