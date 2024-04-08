using CommunityToolkit.Mvvm.ComponentModel;

namespace Proxy.ViewModels;

internal sealed class ShellViewModel : ObservableObject
{
    public string Greeting => "Welcome to Avalonia!";
}