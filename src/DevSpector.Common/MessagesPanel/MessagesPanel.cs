using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace DevSpector.Desktop.UI.Views.Shared
{
    public partial class MessagesPanel : TemplatedControl
    {
        public static readonly StyledProperty<List<string>> MessagesProperty =
            AvaloniaProperty.Register<MessagesPanel, List<string>>(nameof(Messages), new List<string>());

        private List<string> _messages = new List<string>();

        public MessagesPanel()
        {

        }

        public List<string> Messages
        {
            get => this.GetValue(MessagesProperty);
            set => this.SetValue(MessagesProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
        }
    }
}
