using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Proxy.ViewModels;

namespace Proxy.Views;

internal sealed partial class ProxyView : UserControl
{
    public ProxyView()
    {
        DataContext = App.Current.Services.GetRequiredService<ProxyViewModel>();
        InitializeComponent();
    }
}