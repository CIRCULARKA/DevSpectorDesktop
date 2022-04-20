using System.Reactive;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISearchViewModel
    {
        string SearchQuery { get; set; }

        ReactiveCommand<Unit, Unit> SearchCommand { get; }
    }
}
