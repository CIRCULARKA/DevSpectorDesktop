using Ninject.Modules;
using DevSpector.SDK;

namespace DevSpector.Desktop.Service.DependencyInjection
{
	public class SdkModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IRawDataProvider>().To<JsonProvider>();
			Bind<IDevicesProvider>().To<DevicesProvider>();
			Bind<IUsersProvider>().To<UsersProvider>();

			Bind<IUserSession>().To<UserSession>().InSingletonScope();
		}
	}
}
