using System;
using System.IO;
using Avalonia;
using Avalonia.ReactiveUI;

namespace DevSpector.Desktop.UI
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				BuildAvaloniaApp().
					StartWithClassicDesktopLifetime(args);
			}
			catch (Exception e)
			{
				Log(e.Message);
			}
		}

		public static AppBuilder BuildAvaloniaApp() =>
			AppBuilder.Configure<App>().
				UsePlatformDetect().
				LogToTrace().
				UseReactiveUI();

		/// <summary>
		/// Logs message into <c>logs/<cur-date>.txt</c> file into root of the app
		/// </summary>
		private static void Log(string message)
		{
			DateTime now = DateTime.Now;
			var logsDir = "logs";
			Directory.CreateDirectory(logsDir);
			var targetPath = Path.Combine(logsDir, now.ToString("dd-MM-yyyy") + ".txt");
			File.AppendAllText(
				targetPath,
				$"[{now.ToString("HH:MM:ss")}]\n{message}\n\n"
			);
		}
	}
}
