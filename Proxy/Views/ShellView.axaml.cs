using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Proxy.Services;
using Proxy.ViewModels;

namespace Proxy.Views;

internal sealed partial class ShellView : Window
{
    public ShellView()
    {
        DataContext = App.Current.Services.GetRequiredService<ShellViewModel>();
        InitializeComponent();
    }


    protected override async void OnClosing(WindowClosingEventArgs eventArgs)
    {
        await App.Current.Services
            .GetRequiredService<ProxyService>()
            .DisposeAsync();

        base.OnClosing(eventArgs);
    }
}