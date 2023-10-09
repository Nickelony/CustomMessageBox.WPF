using System.Windows;

namespace CustomMessageBox.WPF.Demo;

public partial class DarkStyles : ResourceDictionary
{
	private void CloseWindow_Event(object sender, RoutedEventArgs e)
	{
		if (e.Source != null)
			try { CloseWindow(Window.GetWindow((FrameworkElement)e.Source)); }
			catch { }
	}

	public static void CloseWindow(Window window)
		=> window.Close();
}
