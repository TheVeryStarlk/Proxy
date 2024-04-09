using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Proxy.Services;
using Proxy.ViewModels;
using Proxy.Views;

namespace Proxy;

internal sealed class App : Application
{
    public new static App Current => (App) Application.Current!;

    public IServiceProvider Services { get; } = new ServiceCollection()
        .AddTransient<ShellView>()
        .AddTransient<ShellViewModel>()
        .AddTransient<StartView>()
        .AddTransient<StartViewModel>()
        .AddTransient<ProxyView>()
        .AddSingleton<ProxyViewModel>()
        .AddTransient<MessageView>()
        .AddTransient<MessageViewModel>()
        .AddSingleton<ProxyService>()
        .BuildServiceProvider();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = Services.GetRequiredService<ShellView>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}