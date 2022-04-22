using System.Threading.Tasks;

namespace DevSpector.Desktop.UI.ViewModels
{

    public interface IInitializableListViewModel<TModel>
    {
        Task UpdateListAsync(object keyToSelectBy = null);
    }
}
