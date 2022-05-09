using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace DevSpector.Desktop.UI.Views
{
    public class RootWindow : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            var app = Application.Current.
                ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

            app.Shutdown();
        }
    }
}
