using System.Collections.Generic;
using Ninject;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DevSpector.SDK;
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

        private string _targetHost;

        public App()
        {
            _kernel = new StandardKernel();
        }

        public override void Initialize() =>
            AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            SetTargetHost("devspector.herokuapp");

            EnableApplicationEvents();

            AddAuthorization();

            AddSDK();

            AddViewModels();

            AddLanguage("ru");

            SetupMainWindow();

            base.OnFrameworkInitializationCompleted();
        }

        private void SetTargetHost(string hostname) =>
            _targetHost = hostname;

        private void SetupMainWindow()
        {
            var desktop = ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            desktop.MainWindow = _kernel.Get<AuthorizationView>();
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

            //
            // Subscribe VMs UpdateDeviceInfo on appliance selection
            //

            var targetVMsAmount = 4;
            var deviceInfoVMs = new List<IDeviceInfoViewModel>(targetVMsAmount);

            deviceInfoVMs.Add(commonInfoVM);
            deviceInfoVMs.Add(locationInfoVM);
            deviceInfoVMs.Add(softwareInfoVM);
            deviceInfoVMs.Add(networkInfoVM);

            foreach (var vm in deviceInfoVMs)
                appEvents.ApplianceSelected += vm.UpdateDeviceInfo;

            //
            // Update current user info on user change
            //
            appEvents.UserSelected += userInfoVM.UpdateUserInfo;

            //
            // Subscribe appliances list update on search
            //

            appEvents.SearchExecuted += devicesListVM.LoadItemsFromList;

            //
            // Subscribe on authorization
            //

            appEvents.AuthorizationCompleted += mainView.Show;
            appEvents.AuthorizationCompleted += authView.Hide;
            appEvents.AuthorizationCompleted += devicesListVM.InitializeList;
            appEvents.AuthorizationCompleted += usersListVM.InitializeList;

            appEvents.UserAuthorized += sessionBrokerVM.UpdateLoggedUserInfo;

            //
            // Subscribe on logout
            //
            appEvents.Logout += mainView.Hide;
            appEvents.Logout += authView.Show;
            appEvents.Logout += authVM.ClearCredentials;
        }

        private void AddViewModels()
        {
            _kernel.Load(new ViewModelsModule());

            SubscribeToEvents();
        }

        private void AddAuthorization()
        {
            _kernel.Bind<IAuthorizationManager>().To<AuthorizationManager>().
                WithConstructorArgument("hostname", _targetHost);
            _kernel.Bind<IUserSession>().To<UserSession>();
        }

        private void AddSDK()
        {
			_kernel.Bind<IRawDataProvider>().To<JsonProvider>().
                WithConstructorArgument("hostname", _targetHost);

            var rawDataProvider = _kernel.Get<IRawDataProvider>();

			_kernel.Bind<IDevicesProvider>().To<DevicesProvider>().
                WithConstructorArgument(
                    "provider",
                    rawDataProvider
                );
			_kernel.Bind<IUsersProvider>().To<UsersProvider>().
                WithConstructorArgument("provider", rawDataProvider);
        }

        private void AddLanguage(string langCode)
        {
            _kernel.Bind<ILanguageSwitcher>().To<LanguageSwitcher>();

            var languageSwitcher = _kernel.Get<ILanguageSwitcher>();
            languageSwitcher.SetLanguage(langCode);
        }

        private void EnableApplicationEvents() =>
            _kernel.Bind<IApplicationEvents>().To<ApplicationEvents>().InSingletonScope();
    }
}
