using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CustomMessageBox.WPF;

internal static class NativeMethods
{
	internal const int SWP_NOSIZE = 0x0001;
	internal const int SWP_NOMOVE = 0x0002;
	internal const int SWP_NOZORDER = 0x0004;
	internal const int SWP_FRAMECHANGED = 0x0020;
	internal const int GWL_EXSTYLE = -20;
	internal const int WS_EX_DLGMODALFRAME = 0x0001;

	[DllImport("user32.dll", SetLastError = true)]
	internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

	/// <summary>
	/// Hides the window's title bar icon.
	/// </summary>
	internal static void HideIcon(this Window window)
	{
		// Get this window's handle
		IntPtr hWnd = new WindowInteropHelper(window).Handle;

		// Change the extended window style to not show a window icon
		int extendedStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
		SetWindowLong(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

		// Update the window's non-client area to reflect the changes
		SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
	}
}
