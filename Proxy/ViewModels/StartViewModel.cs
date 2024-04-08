using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;

namespace Proxy.ViewModels;

internal sealed partial class StartViewModel : ObservableObject
{
    [RelayCommand]
    private void Click()
    {
        WeakReferenceMessenger.Default.Send<NavigationMessage>();
    }
}