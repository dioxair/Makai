using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Makai.Utils;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using Avalonia.Threading;

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

        th12[0].EnableRaisingEvents = true;
        th12[0].Exited += IfTouhouExited;
    }

    private void ApplyButton_OnClick(object? sender, RoutedEventArgs e)
    {
        touhou.Invulnerability = (bool)InvulnerabilityCheckBox.IsChecked;
        touhou.Autobomb = (bool)AutobombCheckBox.IsChecked;
        touhou.AutocollectItems = (bool)AutocollectItemsCheckBox.IsChecked;

        if ((bool)GrazeCheckBox.IsChecked)
            touhou.DisableGraze();
        else
            touhou.EnableGraze();

        /* TODO: it looks like when changing a ufo slot already filled, the slot number will
         * go up by one when the player gains another ufo. this needs to be fixed.
         */
        UpdateUFOSlot(UFOSlot1.SelectedIndex, value => touhou.UFOSlot1 = value);
        UpdateUFOSlot(UFOSlot2.SelectedIndex, value => touhou.UFOSlot2 = value);
        UpdateUFOSlot(UFOSlot3.SelectedIndex, value => touhou.UFOSlot3 = value);
    }

    private void UpdateUFOSlot(int selectedIndex, Action<int> setUFOSlot)
    {
        if (Enum.IsDefined(typeof(Touhou.UFOColor), selectedIndex))
        {
            setUFOSlot(selectedIndex);
        }
    }

    private async void IfTouhouExited(object? sender, EventArgs e)
    {
        await Dispatcher.UIThread.Invoke(async () =>
        {
            IMsBox<ButtonResult> messageBox = MessageBoxManager
                .GetMessageBoxStandard("Touhou exited",
                    "It looks like you closed Touhou 12.\n" +
                    "Touhou 12 needs to be running in order to use Makai. Please restart Touhou 12 and run Makai again.");
            ButtonResult result = await messageBox.ShowAsync();
            if (result == ButtonResult.Ok) Environment.Exit(0);
        });
    }
}