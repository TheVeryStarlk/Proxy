using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;

namespace Proxy.ViewModels;

internal sealed partial class StartViewModel : ObservableObject
{
    [RelayCommand]
    private Task StartAsync()
    {
        WeakReferenceMessenger.Default.Send<NavigationMessage>();
        return Task.CompletedTask;
    }
}