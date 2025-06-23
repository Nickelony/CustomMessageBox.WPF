using System.Linq;
using System.Windows;

namespace CustomMessageBox.WPF;

internal static class WindowHelper
{
	internal static Window? FindViableOwner()
	{
		Window? activeWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window.IsActive);

		if (activeWindow is not null)
			return activeWindow;

		bool isMainWindowLoaded = Application.Current.MainWindow?.IsLoaded ?? false;
		return isMainWindowLoaded ? Application.Current.MainWindow : null;
	}
}
