using System.Linq;
using System.Windows;

namespace CustomMessageBox.WPF;

internal static class WindowHelper
{
	internal static Window? FindViableOwner()
	{
		Window? activeWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window.IsActive);
		return activeWindow ?? Application.Current.MainWindow;
	}
}
