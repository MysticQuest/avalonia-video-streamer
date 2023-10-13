﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using MyVideoStreamer.ViewModels;

namespace MyVideoStreamer.Views
{
    internal partial class MyVideoView : ReactiveUserControl<MyVideoViewModel>
    {
        public MyVideoView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
