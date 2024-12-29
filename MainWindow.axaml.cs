using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Makai.Utils;
using Memory;

namespace Makai;

public partial class MainWindow : Window
{
private Touhou touhou = new();

public MainWindow()
{
InitializeComponent();
}
}