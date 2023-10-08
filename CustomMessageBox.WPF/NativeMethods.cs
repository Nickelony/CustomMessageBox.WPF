using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CustomMessageBox.WPF;

internal static class NativeMethods
{
	private const int GWL_EXSTYLE = -20;
	private const int SWP_FRAMECHANGED = 0x0020;
	private const int SWP_NOMOVE = 0x0002;
	private const int SWP_NOSIZE = 0x0001;
	private const int SWP_NOZORDER = 0x0004;
	private const int WS_EX_DLGMODALFRAME = 0x0001;

	[DllImport("user32.dll")]
	private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

	[DllImport("kernel32.dll")]
	private static extern IntPtr LoadLibrary(string dllToLoad);

	[DllImport("kernel32.dll")]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

	[DllImport("kernel32.dll")]
	private static extern bool FreeLibrary(IntPtr hModule);

	[DllImport("user32.dll")]
	private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

	[DllImport("user32.dll")]
	internal static extern int DestroyIcon(IntPtr hIcon);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int LoadIconWithScaleDown(IntPtr hinst, int pszName, int cx, int cy, out IntPtr phico);

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

	internal static IntPtr LoadSystemIcon(int icon)
		=> LoadIcon(IntPtr.Zero, new IntPtr(icon));

	internal static void LoadModernSystemIcon(int icon, int width, int height, out IntPtr phico)
	{
		IntPtr pDll = LoadLibrary("comctl32.dll");
		IntPtr pAddressOfFunctionToCall = GetProcAddress(pDll, "LoadIconWithScaleDown");

		var loadIcon = (LoadIconWithScaleDown)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(LoadIconWithScaleDown));
		loadIcon(IntPtr.Zero, icon, width, height, out phico);

		FreeLibrary(pDll);
	}

	internal static bool IsComCtl32Version6OrGreater()
	{
		IntPtr pDll = LoadLibrary("comctl32.dll");
		bool is6OrGreater = pDll != IntPtr.Zero && GetProcAddress(pDll, "LoadIconWithScaleDown") != IntPtr.Zero;

		FreeLibrary(pDll);
		return is6OrGreater;
	}
}
