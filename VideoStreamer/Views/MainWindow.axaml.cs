﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MyVideoStreamer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
