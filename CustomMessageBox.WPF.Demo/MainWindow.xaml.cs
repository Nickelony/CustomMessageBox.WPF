using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomMessageBox.WPF.Demo;

public partial class MainWindow : Window
{
	public MainWindow()
		=> InitializeComponent();

	private void Button_Traditional_OK(object? sender, RoutedEventArgs e)
	{
		CMessageBox.UsePathIconsByDefault = false;

		CMessageBox.Show(
			"This is a traditional message box with an \"OK\" button.",
			"Traditional 1");
	}

	private void Button_Traditional_YesNoCancel(object? sender, RoutedEventArgs e)
	{
		CMessageBox.Show(
			"This is a traditional message box with \"YesNoCancel\" buttons.",
			"Traditional 2",
			CMessageBoxButtons.YesNoCancel,
			CMessageBoxIcon.Question,
			CMessageBoxDefaultButton.Button1);
	}

	private void Button_Traditional_BottomCenter(object? sender, RoutedEventArgs e)
	{
		var messageBox = new CMessageBox(
			"This is a traditional message box with \"YesNo\" buttons.\n" +
			"The buttons are aligned to the center.",
			"Traditional 3", CMessageBoxIcon.Information)
		{
			HorizontalButtonsPanelAlignment = HorizontalAlignment.Center
		};

		messageBox.Show(CMessageBoxButtons.YesNo, CMessageBoxDefaultButton.Button1);
	}

	private void Button_Traditional_BottomRight(object? sender, RoutedEventArgs e)
	{
		CMessageBox.Show(
			"This is a traditional message box with \"YesNo\" buttons.\n" +
			"The buttons are aligned to the right.",
			"Traditional 4",
			CMessageBoxButtons.YesNo,
			CMessageBoxIcon.Error,
			CMessageBoxDefaultButton.Button1);
	}

	private void Button_Traditional_BottomLeft(object? sender, RoutedEventArgs e)
	{
		var messageBox = new CMessageBox(
			"This is a traditional message box with \"YesNo\" buttons.\n" +
			"The buttons are aligned to the left.",
			"Traditional 5", CMessageBoxIcon.Warning)
		{
			HorizontalButtonsPanelAlignment = HorizontalAlignment.Left
		};

		messageBox.Show(CMessageBoxButtons.YesNo, CMessageBoxDefaultButton.Button1);
	}

	private void Button_Traditional_CustomImage(object? sender, RoutedEventArgs e)
	{
		var bitmap = new BitmapImage(new Uri("pack://application:,,,/CustomMessageBox.WPF.Demo;component/Assets/Icon.png"));

		var messageBox = new CMessageBox(
			"This is a traditional message box with \"YesNo\" buttons.\n" +
			"The message contains a custom icon with its size set to 64 x 64.",
			"Traditional 6", bitmap)
		{
			MaxIconWidth = 64,
			MaxIconHeight = 64,
			SoundOverride = SystemSounds.Asterisk
		};

		messageBox.Show(CMessageBoxButtons.YesNo, CMessageBoxDefaultButton.Button1);
	}

	private void Button_Custom1(object? sender, RoutedEventArgs e)
	{
		CMessageBox.UsePathIconsByDefault = true;

		var textBlock = new TextBlock
		{
			Text = "This is a custom message box with \"YesNoCancel\" buttons.\n" +
				   "The buttons are displayed vertically and to the left.",
			TextAlignment = TextAlignment.Center
		};

		var messageBox = new CMessageBox(textBlock, "Custom 1", CMessageBoxIcon.Error)
		{
			DialogContentOrientation = Orientation.Horizontal,
			MessagePanelOrientation = Orientation.Vertical,
			ButtonsPanelOrientation = Orientation.Vertical,
			MaxIconWidth = 192,
			MaxIconHeight = 192,
			MessagePanelPadding = new Thickness(20, 20, 0, 20),
			ButtonsPanelBackground = Brushes.Transparent,
		};

		messageBox.Show(
			new CMessageBoxButton<CMessageBoxResult>(CMessageBox.YesText, CMessageBoxResult.Yes, CSpecialButtonRole.IsDefault),
			new CMessageBoxButton<CMessageBoxResult>(CMessageBox.NoText, CMessageBoxResult.No),
			new CMessageBoxButton<CMessageBoxResult>(CMessageBox.CancelText, CMessageBoxResult.Cancel, CSpecialButtonRole.IsCancel)
		);
	}

	private void Button_Custom2(object? sender, RoutedEventArgs e)
	{
		var textBlock = new TextBlock
		{
			Text = "This is a custom message box with custom buttons.\n" +
				   "The icon is displayed above the text.",
			TextAlignment = TextAlignment.Center
		};

		var messageBox = new CMessageBox(textBlock, "Custom 2", CMessageBoxIcon.Information)
		{
			MessagePanelOrientation = Orientation.Vertical,
			HorizontalButtonsPanelAlignment = HorizontalAlignment.Center,
			MinButtonWidth = 125
		};

		messageBox.Show(
			new CMessageBoxButton<CMessageBoxResult>("Yes, Confirm", CMessageBoxResult.Yes, CSpecialButtonRole.IsDefault),
			new CMessageBoxButton<CMessageBoxResult>("No, Cancel", CMessageBoxResult.Cancel, CSpecialButtonRole.IsCancel)
		);
	}

	private void Button_Custom3(object? sender, RoutedEventArgs e)
	{
		var textBlock = new TextBlock
		{
			Text = "This is a custom message box with custom buttons.\n\n" +
				   "The buttons are displayed vertically and on the bottom.\n" +
				   "The icon is displayed above the text.",
			TextAlignment = TextAlignment.Center
		};

		var messageBox = new CMessageBox(textBlock, "Custom 3", CMessageBoxIcon.Warning)
		{
			MessagePanelOrientation = Orientation.Vertical,
			HorizontalButtonsPanelAlignment = HorizontalAlignment.Center,
			ButtonsPanelOrientation = Orientation.Vertical
		};

		messageBox.Show(
			new CMessageBoxButton<CustomMessageBoxResult>("Accept", CustomMessageBoxResult.Accept, CSpecialButtonRole.None),
			new CMessageBoxButton<CustomMessageBoxResult>("Decline", CustomMessageBoxResult.Decline)
		);
	}

	private void Button_Custom4(object? sender, RoutedEventArgs e)
	{
		var bitmap = new BitmapImage(new Uri("pack://application:,,,/CustomMessageBox.WPF.Demo;component/Assets/Icon.png"));

		var messageBox = new CMessageBox(
			"This is a custom message box with custom buttons.\n" +
			"The icon is custom and is displayed above the text.",
			"Custom 4", bitmap)
		{
			ShowTitleBarIcon = true,
			Padding = new Thickness(24),
			DialogContentOrientation = Orientation.Horizontal,
			MessagePanelOrientation = Orientation.Vertical,
			ButtonsPanelOrientation = Orientation.Vertical,
			MinButtonWidth = 90,
			MinButtonHeight = 32,
			ButtonSpacing = 12,
			MaxIconWidth = 256,
			MaxIconHeight = 256
		};

		string[] languages = new[] { "English", "Polish", "German", "Spanish", "Italian", "French", "Chinese" };
		string[] flagUris = new[] { "gb.png", "pl.png", "de.png", "es.png", "it.png", "fr.png", "cn.png" };

		var buttons = new List<CMessageBoxButton<int>>();
		var iconMargin = new Thickness(6, 0, 6, 0);

		for (int i = 0; i < languages.Length; i++)
		{
			var content = new StackPanel { Orientation = Orientation.Horizontal };
			var flagIcon = new BitmapImage(new Uri($"pack://application:,,,/CustomMessageBox.WPF.Demo;component/Assets/{flagUris[i]}"));
			content.Children.Add(new Image { Source = flagIcon, Margin = iconMargin });
			content.Children.Add(new TextBlock { Text = languages[i] });

			buttons.Add(new CMessageBoxButton<int>(content, i + 1, styleKey: "LanguageOptionStyle"));
		}

		int result = messageBox.Show(buttons.ToArray());

		if (result >= 1)
		{
			var content = new StackPanel { Orientation = Orientation.Horizontal };
			content.Children.Add(new TextBlock { Text = $"You have selected " });
			content.Children.Add(new TextBlock
			{
				Text = languages[result - 1],
				FontWeight = FontWeights.Bold,
				Foreground = Brushes.Red
			});

			CMessageBox.Show(content);
		}
		else
			CMessageBox.Show("You have not selected any language.", icon: CMessageBoxIcon.Information);
	}

	private void CheckBox_Checked(object sender, RoutedEventArgs e)
	{
		Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
		{
			Source = new Uri("DarkStyles.xaml", UriKind.Relative)
		});

		if (TryFindResource("Brush_Background_Alternative") is SolidColorBrush brush)
			CMessageBox.DefaultButtonsPanelBackground = brush;
	}

	private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
	{
		Application.Current.Resources.MergedDictionaries.RemoveAt(0);
		CMessageBox.DefaultButtonsPanelBackground = SystemColors.ControlBrush;
	}
}
