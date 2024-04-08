using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Proxy.ViewModels;

namespace Proxy.Views;

internal sealed partial class ShellView : Window
{
    public ShellView()
    {
        DataContext = App.Current.Services.GetRequiredService<ShellViewModel>();
        InitializeComponent();
    }
}