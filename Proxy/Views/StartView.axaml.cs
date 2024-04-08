using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Proxy.ViewModels;

namespace Proxy.Views;

internal sealed partial class StartView : UserControl
{
    public StartView()
    {
        DataContext = App.Current.Services.GetRequiredService<StartViewModel>();
        InitializeComponent();
    }
}