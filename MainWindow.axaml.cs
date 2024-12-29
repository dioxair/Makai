using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Makai.Utils;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;

namespace Makai;

public partial class MainWindow : Window
{
    private Touhou touhou = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        IMsBox<ButtonResult> messageBox = MessageBoxManager
            .GetMessageBoxStandard("Touhou not running",
                "Touhou 12 needs to be running in order to use Makai. Please start Touhou 12 and try again.");
        Process[] th12 = Process.GetProcessesByName("th12");
        if (th12.Length == 0)
        {
            ButtonResult result = await messageBox.ShowAsync();
            if (result == ButtonResult.Ok) Environment.Exit(0);
        }
    }
}