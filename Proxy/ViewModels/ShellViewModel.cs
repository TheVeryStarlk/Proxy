using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Proxy.Messages;

namespace Proxy.ViewModels;

internal sealed partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    private INotifyPropertyChanged current;

    public ShellViewModel(StartViewModel startViewModel, ProxyViewModel proxyViewModel)
    {
        current = startViewModel;

        WeakReferenceMessenger.Default.Register<NavigationMessage>(
            this,
            (_, _) => Current = Current is StartViewModel
                ? proxyViewModel
                : startViewModel);
    }
}