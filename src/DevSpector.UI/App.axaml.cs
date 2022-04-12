using System;
using System.Collections.Generic;
using Ninject;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DevSpector.SDK;
using DevSpector.SDK.Networking;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Authorization;
using DevSpector.Desktop.Service;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.UI.ViewModels;
using DevSpector.Desktop.Service.DependencyInjection;

namespace DevSpector.Desktop.UI
{
    public class App : Application
    {
        private readonly IKernel _kernel;

        private IHostBuilder _hostBuilder;

        public App()
        {
            _kernel = new StandardKernel();
        }

        public override void Initialize() =>
            AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            ConfigureTargetHost();

            EnableApplicationEvents();

            AddServices();

            AddSDK();

            AddAuthorization();

            AddViewModels();

            UseLanguage("ru");

            SetupMainWindow();

            base.OnFrameworkInitializationCompleted();
        }

        private void AddAuthorization()
        {
            _kernel.Bind<IUserSession>().To<UserSession>().InSingletonScope();

            IServerDataProvider defaultProvider = _kernel.Get<IServerDataProvider>();

            _kernel.Bind<IAuthorizationManager>().To<AuthorizationManager>().
                WithConstructorArgument("provider", defaultProvider);
        }

        private void ConfigureTargetHost()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            if (environment == "Production")
                _hostBuilder = new HostBuilder("devspector.herokuapp.com", scheme: "https");
            else if (environment == "Development")
                _hostBuilder = new HostBuilder("dev-devspector.herokuapp.com", scheme: "https");
            else
                _hostBuilder = new HostBuilder(port: 5000);
        }

        private void SetupMainWindow()
        {
            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<AuthorizationView>();
        }

        private void AddViewModels()
        {
            _kernel.Load(new ViewModelsModule());

            SubscribeToEvents();
        }

        private void AddSDK()
        {
            _kernel.Bind<IServerDataProvider>().To<JsonProvider>().
                InSingletonScope().
                    WithConstructorArgument("builder", _hostBuilder);

            var dataProvider = _kernel.Get<IServerDataProvider>();

			_kernel.Bind<ILocationProvider>().To<LocationProvider>().
                WithConstructorArgument("provider", dataProvider);

			_kernel.Bind<IDevicesProvider>().To<DevicesProvider>().
                WithConstructorArgument("provider", dataProvider);

			_kernel.Bind<IDevicesEditor>().To<DevicesEditor>().
                WithConstructorArgument("provider", dataProvider);

            _kernel.Bind<IUsersProvider>().To<UsersProvider>().
                WithConstructorArgument("provider", dataProvider);

            _kernel.Bind<IUsersEditor>().To<UsersEditor>().
                WithConstructorArgument("provider", dataProvider);

            _kernel.Bind<INetworkManager>().To<NetworkManager>().
                WithConstructorArgument("provider", dataProvider);
        }

        private void AddServices()
        {
            _kernel.Bind<IMessagesBroker>().
                To<MessagesBroker>().InSingletonScope();
            _kernel.Bind<IDevicesStorage>().
                To<DevicesStorage>().InSingletonScope();
            _kernel.Bind<IUsersStorage>().
                To<UsersStorage>().InSingletonScope();
        }

        private void UseLanguage(string langCode)
        {
            _kernel.Bind<ILanguageSwitcher>().To<LanguageSwitcher>();

            var languageSwitcher = _kernel.Get<ILanguageSwitcher>();
            languageSwitcher.SetLanguage(langCode);
        }

        private void EnableApplicationEvents()
        {
            _kernel.Bind<IApplicationEvents>().To<ApplicationEvents>().InSingletonScope();
        }

        private void SubscribeToEvents()
        {
            var appEvents = _kernel.Get<IApplicationEvents>();

            //
            // Get views
            //
            var mainView = _kernel.Get<MainView>();
            var authView = _kernel.Get<AuthorizationView>();

            //
            // Get VM's
            //
            // VM stands for View Model
            var authVM = _kernel.Get<IAuthorizationViewModel>();
            var commonInfoVM = _kernel.Get<ICommonInfoViewModel>();
            var locationInfoVM = _kernel.Get<ILocationInfoViewModel>();
            var softwareInfoVM = _kernel.Get<ISoftwareInfoViewModel>();
            var networkInfoVM = _kernel.Get<INetworkInfoViewModel>();
            var devicesListVM = _kernel.Get<IDevicesListViewModel>();
            var usersListVM = _kernel.Get<IUsersListViewModel>();
            var userInfoVM = _kernel.Get<IUserInfoViewModel>();
            var sessionBrokerVM = _kernel.Get<ISessionBrokerViewModel>();
            var messagesBrokerVM = _kernel.Get<IMessagesBrokerViewModel>();
            var freeIPListVM = _kernel.Get<IFreeIPListViewModel>();

            appEvents.UserSelected += userInfoVM.UpdateUserInfo;

            appEvents.SearchExecuted += devicesListVM.LoadItemsFromList;

            appEvents.UserAuthorized += (u) => {
                _kernel.Get<IServerDataProvider>().ChangeAccessToken(u.AccessToken);

                sessionBrokerVM.UpdateLoggedUserInfo(u);

                locationInfoVM.LoadHousingsAsync();
                commonInfoVM.LoadDeviceTypesAsync();
                usersListVM.LoadUserGroupsAsync();
                userInfoVM.LoadUserGroupsAsync();

                devicesListVM.UpdateList();
                usersListVM.UpdateList();

                freeIPListVM.UpdateList();

                authView.Hide();
                mainView.Show();
            };

            //
            // Subscribe VM's UpdateDeviceInfo on Device selection
            //

            var targetVMsAmount = 4;
            var deviceInfoVMs = new List<IDeviceInfoViewModel>(targetVMsAmount);

            deviceInfoVMs.Add(commonInfoVM);
            deviceInfoVMs.Add(locationInfoVM);
            deviceInfoVMs.Add(softwareInfoVM);
            deviceInfoVMs.Add(networkInfoVM);

            foreach (var vm in deviceInfoVMs)
                appEvents.DeviceSelected += vm.UpdateDeviceInfo;

            //

            appEvents.DeviceUpdated += () => {
                devicesListVM.UpdateList();
            };

            appEvents.DeviceDeleted += (d) => {
                freeIPListVM.UpdateList();
            };

            appEvents.IPAddressAdded += (d, ip) => {
                devicesListVM.AddIPToSelectedDevice(ip);
                networkInfoVM.UpdateDeviceInfo(d);
            };

            appEvents.IPAddressDeleted += (d, ip) => {
                devicesListVM.RemoveIPFromSelectedDevice(ip);
                freeIPListVM.UpdateList();
            };

            appEvents.Logout += () => {
                mainView.Hide();
                authView.Show();
                authVM.ClearCredentials();
            };

            appEvents.UserNotified += (message) => {
                messagesBrokerVM.Message = message;
            };
        }
    }
}
