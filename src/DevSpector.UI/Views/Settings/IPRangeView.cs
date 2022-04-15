using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class IPRangeView : UserControl
    {
        public IPRangeView() {}

        public IPRangeView(IIPRangeViewModel vm)
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
