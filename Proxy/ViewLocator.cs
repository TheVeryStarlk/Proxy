using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Proxy;

internal sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return null;
        }

        var name = data
            .GetType().FullName!
            .Replace("ViewModel", "View", StringComparison.Ordinal);

        var type = Type.GetType(name);

        return type is not null
            ? (Control) Activator.CreateInstance(type)!
            : new TextBlock()
            {
                Text = $"Could not find: \"{name}\"."
            };
    }

    public bool Match(object? data)
    {
        return data is INotifyPropertyChanged;
    }
}