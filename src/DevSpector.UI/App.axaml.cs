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
using DevSpector.Desktop.UI.Validators;
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

            AddValidation();

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

        private void AddValidation()
        {
            _kernel.Bind<ITextValidator>().To<EnglishTextValidator>().
                WithConstructorArgument("validationErrorMessage", value: null);
        }

        private void ConfigureTargetHost()
        {
            string environment = Environment.GetEnvironmentVariable("DEVSPECTOR_ENV");

            string host = Environment.GetEnvironmentVariable("DEVSPECTOR_HOST");
            string scheme = "http";
            int port;

            if (environment == "Development")
            {
                host = "dev-devspector.herokuapp.com";
                scheme = "https";
                port = 443;
            }
            else
            {
                host = Environment.GetEnvironmentVariable("DEVSPECTOR_HOST");
                if (host == null)
                    throw new InvalidOperationException("Задайте имя хоста через переменную \"DEVSPECTOR_HOST\"");

                scheme = "http";

                int.TryParse(
                    Environment.GetEnvironmentVariable("DEVSPECTOR_PORT"),
                    out port
                );
                if (port < 0) port = 0;
            }

            _hostBuilder = new HostBuilder(host, port, scheme);
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
            _kernel.Bind<IUserRights>().
                To<UserRights>().InSingletonScope();
        }

        private void UseLanguage(string langCode)
        {
            _kernel.Bind<ILanguageSwitcher>().To<LanguageSwitcher>();

            var languageSwitcher = _kernel.Get<ILanguageSwitcher>();
            languageSwitcher.SetLanguage(langCode);
        }

        private void EnableApplicationEvents()
        {
            _kernel.Bind<ApplicationEvents>().ToSelf().InSingletonScope();
        }

        private void SubscribeToEvents()
        {
            var appEvents = _kernel.Get<ApplicationEvents>();

            //
            // Get services
            //
            var userRights = _kernel.Get<UserRights>();

            //
            // Get views
            //
            var mainView = _kernel.Get<MainView>();
            var authView = _kernel.Get<AuthorizationView>();

            //
            // Get VM's
            //
            // VM stands for View Model
            var viewModels = new List<object>();

            var authVM = _kernel.Get<IAuthorizationViewModel>();
            viewModels.Add(authVM);
            var commonInfoVM = _kernel.Get<ICommonInfoViewModel>();
            viewModels.Add(commonInfoVM);
            var locationInfoVM = _kernel.Get<ILocationInfoViewModel>();
            viewModels.Add(locationInfoVM);
            var softwareInfoVM = _kernel.Get<ISoftwareInfoViewModel>();
            viewModels.Add(softwareInfoVM);
            var networkInfoVM = _kernel.Get<INetworkInfoViewModel>();
            viewModels.Add(networkInfoVM);
            var devicesListVM = _kernel.Get<IDevicesListViewModel>();
            viewModels.Add(devicesListVM);
            var usersListVM = _kernel.Get<IUsersListViewModel>();
            viewModels.Add(usersListVM);
            var userInfoVM = _kernel.Get<IUserInfoViewModel>();
            viewModels.Add(userInfoVM);
            var sessionBrokerVM = _kernel.Get<ISessionBrokerViewModel>();
            viewModels.Add(sessionBrokerVM);
            var messagesBrokerVM = _kernel.Get<IMessagesBrokerViewModel>();
            viewModels.Add(messagesBrokerVM);
            var freeIPListVM = _kernel.Get<IFreeIPListViewModel>();
            viewModels.Add(freeIPListVM);
            var accessTokenVM = _kernel.Get<IAccessKeyViewModel>();
            viewModels.Add(accessTokenVM);
            var passwordVM = _kernel.Get<IPasswordViewModel>();
            viewModels.Add(passwordVM);
            var mainMenuVM = _kernel.Get<IMainMenuViewModel>();
            viewModels.Add(mainMenuVM);
            var devicesMainVM = _kernel.Get<IDevicesMainViewModel>();
            viewModels.Add(devicesMainVM);
            var usersMainVM = _kernel.Get<IUsersMainViewModel>();
            viewModels.Add(usersMainVM);
            var mainVM = _kernel.Get<IMainViewModel>();
            viewModels.Add(mainVM);
            var settingsVM = _kernel.Get<ISettingsViewModel>();
            viewModels.Add(settingsVM);

            foreach (var vm in viewModels)
                appEvents.UserAuthorized += (vm as ViewModelBase).UpdateUserRights;

            appEvents.UserSelected += (u) => {
                userInfoVM.UpdateUserInfo(u);
                userInfoVM.UpdateInputsAccessibility();
            };

            appEvents.UserUpdated += (o) => {
                usersListVM.UpdateListAsync(o);
            };

            appEvents.DeviceSearched += devicesListVM.LoadItemsFromList;
            appEvents.UserSearched += usersListVM.LoadItemsFromList;

            appEvents.UserAuthorized += (u) => {
                _kernel.Get<IServerDataProvider>().ChangeAccessToken(u.AccessToken);

                sessionBrokerVM.UpdateLoggedUserInfo(u);
                accessTokenVM.DisplayUserAccessKey(u);
                accessTokenVM.EraisePasswordInput();
                passwordVM.EraisePasswordInputs();
                messagesBrokerVM.ClearMessages();

                locationInfoVM.LoadHousingsAsync();
                commonInfoVM.LoadDeviceTypesAsync();
                usersListVM.LoadUserGroupsAsync();
                userInfoVM.LoadUserGroupsAsync();
                userInfoVM.UpdateUserInfo(null);

                devicesListVM.UpdateListAsync();
                usersListVM.UpdateListAsync();

                freeIPListVM.UpdateListAsync();

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

            appEvents.DeviceSelected += (d) => {
                networkInfoVM.HideFreeIPList();
                commonInfoVM.UpdateInputsAccessibility();
                locationInfoVM.UpdateInputsAccessibility();
            };

            //

            appEvents.DeviceUpdated += (o) => {
                devicesListVM.UpdateListAsync(o);
            };

            appEvents.DeviceDeleted += (d) => {
                freeIPListVM.UpdateListAsync();
            };

            appEvents.IPAddressAdded += (d, ip) => {
                devicesListVM.AddIPToSelectedDevice(ip);
                networkInfoVM.UpdateDeviceInfo(d);
            };

            appEvents.IPAddressDeleted += (d, ip) => {
                devicesListVM.RemoveIPFromSelectedDevice(ip);
                freeIPListVM.UpdateListAsync();
            };

            appEvents.IPRangeUpdated += () => {
                devicesListVM.UpdateListAsync();
                freeIPListVM.UpdateListAsync();
            };

            appEvents.Logout += () => {
                mainVM.SelectDefaultView();
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
