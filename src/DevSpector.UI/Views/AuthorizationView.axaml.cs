using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class AuthorizationView : RootWindow
    {
        public AuthorizationView() { }

        public AuthorizationView(IAuthorizationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
