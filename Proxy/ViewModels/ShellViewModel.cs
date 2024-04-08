using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Proxy.ViewModels;

internal sealed partial class ShellViewModel(
    StartViewModel startViewModel,
    ProxyViewModel proxyViewModel) : ObservableObject
{
    [ObservableProperty]
    private INotifyPropertyChanged current = startViewModel;
}