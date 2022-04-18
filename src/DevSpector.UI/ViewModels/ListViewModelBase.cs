using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public abstract class ListViewModelBase<TModel> : ViewModelBase, IListViewModel<TModel>
    {
        protected TModel _selectedItem;

        protected bool _areItemsLoaded;

        protected bool _areItemsLoading;

        protected bool _areThereItems;

        protected string _noItemsMessage;

        protected ListViewModelBase(
            IUserRights userRights
        ) : base(userRights)
        {
            ItemsToDisplay = new ObservableCollection<TModel>();
            ItemsCache = new List<TModel>();
        }

        public ObservableCollection<TModel> ItemsToDisplay { get; }

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
            ItemsToDisplay.Clear();

            foreach (var item in items)
                ItemsToDisplay.Add(item);

            if (ItemsToDisplay.Count == 0) {
                AreThereItems = false;
                NoItemsMessage = "Нет элементов";
            }
            else AreThereItems = true;
        }

        protected virtual void AddToList(TModel item)
        {
            ItemsToDisplay.Add(item);

            SelectedItem = item;
        }

        protected virtual void RemoveFromList(TModel item)
        {
            int previousSelectedIndex = ItemsToDisplay.IndexOf(item);
            ItemsToDisplay.Remove(item);
            ItemsCache.Remove(item);

            if (ItemsCache.Count == 0)
            {
                AreThereItems = false;
                return;
            }

            if (previousSelectedIndex < 1) {
                SelectedItem = ItemsToDisplay.FirstOrDefault();
                return;
            }

            SelectedItem = ItemsToDisplay.Skip(previousSelectedIndex - 1).FirstOrDefault();
        }
    }
}
