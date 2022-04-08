using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public abstract class ListViewModelBase<TModel> : ViewModelBase, IListViewModel<TModel>
    {
        protected TModel _selectedItem;

        protected bool _areItemsLoaded;

        protected bool _areItemsLoading;

        protected bool _areThereItems;

        protected string _noItemsMessage;

        protected ListViewModelBase()
        {
            Items = new ObservableCollection<TModel>();
            ItemsCache = new List<TModel>();
        }

        public ObservableCollection<TModel> Items { get; }

        public List<TModel> ItemsCache { get; set; }

        public abstract TModel SelectedItem { get; set; }

        public bool AreItemsLoaded
        {
            get => _areItemsLoaded;
            set => this.RaiseAndSetIfChanged(ref _areItemsLoaded, value);
        }

        public bool AreThereItems
        {
            get => _areThereItems;
            set { this.RaiseAndSetIfChanged(ref _areThereItems, value); }
        }

        public string NoItemsMessage
        {
            get => _noItemsMessage;
            set { this.RaiseAndSetIfChanged(ref _noItemsMessage, value); }
        }

        public virtual void LoadItemsFromList(IEnumerable<TModel> items)
        {
            Items.Clear();

            foreach (var item in items)
                Items.Add(item);

            if (Items.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Нет элементов";
            }
            else AreThereItems = true;
        }

        protected virtual void AddToList(TModel item)
        {
            Items.Add(item);

            SelectedItem = item;
        }

        protected virtual void RemoveFromList(TModel item)
        {
            int previousSelectedIndex = Items.IndexOf(item);
            Items.Remove(item);
            ItemsCache.Remove(item);

            if (previousSelectedIndex < 1) {
                SelectedItem = Items.FirstOrDefault();
                return;
            }

            SelectedItem = Items.Skip(previousSelectedIndex - 1).FirstOrDefault();
        }
    }
}
