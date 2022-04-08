using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IListViewModel<TModel>
    {
        ObservableCollection<TModel> Items { get; }

        public List<TModel> ItemsCache { get; set; }

        public abstract TModel SelectedItem { get; set; }

        public bool AreItemsLoaded { get; set; }

        public bool AreThereItems { get; set; }

        public string NoItemsMessage { get; set; }

        void LoadItemsFromList(IEnumerable<TModel> items);
    }
}
