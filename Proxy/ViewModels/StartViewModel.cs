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

    [ObservableProperty]
    private ushort listening = 23456;

    [ObservableProperty]
    private bool isLoading;

    [RelayCommand]
    private async Task StartAsync()
    {
        try
        {
            IsLoading = true;

            var endPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            await proxyService.StartAsync(endPoint, listening);

            WeakReferenceMessenger.Default.Send<NavigationMessage>();
        }
        catch
        {
            // Log or something.
        }
        finally
        {
            IsLoading = false;
        }
    }
}