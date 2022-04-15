using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class PasswordView : UserControl
    {
        public PasswordView() {}

        public PasswordView(IPasswordViewModel vm)
        {
            InitializeComponent();

            this.DataContext = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
