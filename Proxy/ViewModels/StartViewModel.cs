using System.Net;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;
using Proxy.Services;

namespace Proxy.ViewModels;

internal sealed partial class StartViewModel(ProxyService proxyService) : ObservableObject
{
    [ObservableProperty]
    private string address = "127.0.0.1";

    [ObservableProperty]
    private ushort port = 25565;

    [RelayCommand]
    private async Task StartAsync()
    {
        try
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            await proxyService.StartAsync(endPoint);

            WeakReferenceMessenger.Default.Send<NavigationMessage>();
        }
        catch
        {
            // Log or something.
        }
    }
}