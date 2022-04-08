using Ninject.Modules;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.Service.DependencyInjection
{
    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            BindViewModels();
            BindViews();
        }

        private void BindViewModels()
        {
            Bind<IMainViewModel>().To<MainViewModel>().InSingletonScope();
            Bind<IMainMenuViewModel>().To<MainMenuViewModel>().InSingletonScope();

            Bind<IDevicesMainViewModel>().To<DevicesMainViewModel>().InSingletonScope();

            Bind<IDevicesListViewModel>().To<DevicesListViewModel>().InSingletonScope();
            Bind<IUsersMainViewModel>().To<UsersMainViewModel>().InSingletonScope();
            Bind<IUsersListViewModel>().To<UsersListViewModel>().InSingletonScope();

            Bind<ICommonInfoViewModel>().To<CommonInfoViewModel>().InSingletonScope();
            Bind<INetworkInfoViewModel>().To<NetworkInfoViewModel>().InSingletonScope();
            Bind<ISoftwareInfoViewModel>().To<SoftwareInfoViewModel>().InSingletonScope();
            Bind<ILocationInfoViewModel>().To<LocationInfoViewModel>().InSingletonScope();
            Bind<IUserInfoViewModel>().To<UserInfoViewModel>().InSingletonScope();

            Bind<ISearchViewModel>().To<SearchViewModel>();

            Bind<IAuthorizationViewModel>().To<AuthorizationViewModel>().InSingletonScope();

            Bind<ISessionBrokerViewModel>().To<SessionBrokerViewModel>().InSingletonScope();
            Bind<IMessagesBrokerViewModel>().To<MessagesBrokerViewModel>().InSingletonScope();

            Bind<ISettingsViewModel>().To<SettingsViewModel>().InSingletonScope();

            Bind<IFreeIPListViewModel>().To<FreeIPListViewModel>().InSingletonScope();
        }

        private void BindViews()
        {
            Bind<MainView>().ToSelf().InSingletonScope();

            Bind<AuthorizationView>().ToSelf().InSingletonScope();

            Bind<DevicesMainView>().ToSelf().InSingletonScope();
            Bind<UsersMainView>().ToSelf().InSingletonScope();

            Bind<DevicesListView>().ToSelf();
            Bind<UsersListView>().ToSelf();

            Bind<CommonInfoView>().ToSelf();
            Bind<SoftwareInfoView>().ToSelf();
            Bind<LocationInfoView>().ToSelf();
            Bind<NetworkInfoView>().ToSelf();
            Bind<UserInfoView>().ToSelf();

            Bind<SearchView>().ToSelf();

            Bind<SessionBrokerView>().ToSelf();
            Bind<MessagesBrokerView>().ToSelf();

            Bind<SettingsView>().ToSelf();

            Bind<FreeIPListView>().ToSelf();
        }
    }
}
