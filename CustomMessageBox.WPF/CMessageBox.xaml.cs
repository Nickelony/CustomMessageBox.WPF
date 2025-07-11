﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomMessageBox.WPF;

/// <summary>
/// A custom message box for WPF applications.
/// </summary>
public partial class CMessageBox : Window, INotifyPropertyChanged
{
	/// <summary>
	/// The string key that is used to style the <see cref="CMessageBox" /> window.
	/// </summary>
	public const string WINDOW_STYLE_KEY = "CMessageBoxWindow";

	/// <summary>
	/// The string key that is used to style the generated buttons inside the <see cref="CMessageBox" />.
	/// </summary>
	public const string BUTTON_STYLE_KEY = "CMessageBoxButton";

	#region Default (static) values

	// Using Fluent Icons for Avalonia: http://avaloniaui.github.io/icons.html

	/// <summary>
	/// Default path icon geometry for the "Question" message box.
	/// </summary>
	public static Geometry QuestionPathIconGeometry { get; set; } = Geometry.Parse("M24 4C35.0457 4 44 12.9543 44 24C44 35.0457 35.0457 44 24 44C12.9543 44 4 35.0457 4 24C4 12.9543 12.9543 4 24 4ZM24 6.5C14.335 6.5 6.5 14.335 6.5 24C6.5 33.665 14.335 41.5 24 41.5C33.665 41.5 41.5 33.665 41.5 24C41.5 14.335 33.665 6.5 24 6.5ZM24.25 32C25.0784 32 25.75 32.6716 25.75 33.5C25.75 34.3284 25.0784 35 24.25 35C23.4216 35 22.75 34.3284 22.75 33.5C22.75 32.6716 23.4216 32 24.25 32ZM24.25 13C27.6147 13 30.5 15.8821 30.5 19.2488C30.502 21.3691 29.7314 22.7192 27.8216 24.7772L26.8066 25.8638C25.7842 27.0028 25.3794 27.7252 25.3409 28.5793L25.3379 28.7411L25.3323 28.8689L25.3143 28.9932C25.2018 29.5636 24.7009 29.9957 24.0968 30.0001C23.4065 30.0049 22.8428 29.4493 22.8379 28.7589C22.8251 26.9703 23.5147 25.7467 25.1461 23.9739L26.1734 22.8762C27.5312 21.3837 28.0012 20.503 28 19.25C28 17.2634 26.2346 15.5 24.25 15.5C22.3307 15.5 20.6142 17.1536 20.5055 19.0587L20.4935 19.3778C20.4295 20.0081 19.8972 20.5 19.25 20.5C18.5596 20.5 18 19.9404 18 19.25C18 15.8846 20.8864 13 24.25 13Z");

	/// <summary>
	/// Default path icon geometry for the "Error" message box.
	/// </summary>
	public static Geometry ErrorPathIconGeometry { get; set; } = Geometry.Parse("M12,2 C17.523,2 22,6.478 22,12 C22,17.522 17.523,22 12,22 C6.477,22 2,17.522 2,12 C2,6.478 6.477,2 12,2 Z M12,3.667 C7.405,3.667 3.667,7.405 3.667,12 C3.667,16.595 7.405,20.333 12,20.333 C16.595,20.333 20.333,16.595 20.333,12 C20.333,7.405 16.595,3.667 12,3.667 Z M11.9986626,14.5022358 C12.5502088,14.5022358 12.9973253,14.9493523 12.9973253,15.5008984 C12.9973253,16.0524446 12.5502088,16.4995611 11.9986626,16.4995611 C11.4471165,16.4995611 11,16.0524446 11,15.5008984 C11,14.9493523 11.4471165,14.5022358 11.9986626,14.5022358 Z M11.9944624,7 C12.3741581,6.99969679 12.6881788,7.28159963 12.7381342,7.64763535 L12.745062,7.7494004 L12.7486629,12.2509944 C12.7489937,12.6652079 12.4134759,13.0012627 11.9992625,13.0015945 C11.6195668,13.0018977 11.3055461,12.7199949 11.2555909,12.3539592 L11.2486629,12.2521941 L11.245062,7.7506001 C11.2447312,7.33638667 11.580249,7.00033178 11.9944624,7 Z");

	/// <summary>
	/// Default path icon geometry for the "Warning" message box.
	/// </summary>
	public static Geometry WarningPathIconGeometry { get; set; } = Geometry.Parse("M10.9093922,2.78216375 C11.9491636,2.20625071 13.2471955,2.54089334 13.8850247,3.52240345 L13.9678229,3.66023048 L21.7267791,17.6684928 C21.9115773,18.0021332 22.0085303,18.3772743 22.0085303,18.7586748 C22.0085303,19.9495388 21.0833687,20.9243197 19.9125791,21.003484 L19.7585303,21.0086748 L4.24277801,21.0086748 C3.86146742,21.0086748 3.48641186,20.9117674 3.15282824,20.7270522 C2.11298886,20.1512618 1.7079483,18.8734454 2.20150311,17.8120352 L2.27440063,17.668725 L10.0311968,3.66046274 C10.2357246,3.291099 10.5400526,2.98673515 10.9093922,2.78216375 Z M20.4146132,18.3952808 L12.6556571,4.3870185 C12.4549601,4.02467391 11.9985248,3.89363262 11.6361802,4.09432959 C11.5438453,4.14547244 11.4637001,4.21532637 11.4006367,4.29899869 L11.3434484,4.38709592 L3.58665221,18.3953582 C3.385998,18.7577265 3.51709315,19.2141464 3.87946142,19.4148006 C3.96285732,19.4609794 4.05402922,19.4906942 4.14802472,19.5026655 L4.24277801,19.5086748 L19.7585303,19.5086748 C20.1727439,19.5086748 20.5085303,19.1728883 20.5085303,18.7586748 C20.5085303,18.6633247 20.4903516,18.5691482 20.455275,18.4811011 L20.4146132,18.3952808 L12.6556571,4.3870185 L20.4146132,18.3952808 Z M12.0004478,16.0017852 C12.5519939,16.0017852 12.9991104,16.4489016 12.9991104,17.0004478 C12.9991104,17.5519939 12.5519939,17.9991104 12.0004478,17.9991104 C11.4489016,17.9991104 11.0017852,17.5519939 11.0017852,17.0004478 C11.0017852,16.4489016 11.4489016,16.0017852 12.0004478,16.0017852 Z M11.9962476,8.49954934 C12.3759432,8.49924613 12.689964,8.78114897 12.7399193,9.14718469 L12.7468472,9.24894974 L12.750448,13.7505438 C12.7507788,14.1647572 12.4152611,14.5008121 12.0010476,14.5011439 C11.621352,14.5014471 11.3073312,14.2195442 11.257376,13.8535085 L11.250448,13.7517435 L11.2468472,9.25014944 C11.2465164,8.83593601 11.5820341,8.49988112 11.9962476,8.49954934 Z");

	/// <summary>
	/// Default path icon geometry for the "Information" message box.
	/// </summary>
	public static Geometry InformationPathIconGeometry { get; set; } = Geometry.Parse("M14,2 C20.6274,2 26,7.37258 26,14 C26,20.6274 20.6274,26 14,26 C7.37258,26 2,20.6274 2,14 C2,7.37258 7.37258,2 14,2 Z M14,3.5 C8.20101,3.5 3.5,8.20101 3.5,14 C3.5,19.799 8.20101,24.5 14,24.5 C19.799,24.5 24.5,19.799 24.5,14 C24.5,8.20101 19.799,3.5 14,3.5 Z M14,11 C14.3796833,11 14.6934889,11.2821653 14.7431531,11.6482323 L14.75,11.75 L14.75,19.25 C14.75,19.6642 14.4142,20 14,20 C13.6203167,20 13.3065111,19.7178347 13.2568469,19.3517677 L13.25,19.25 L13.25,11.75 C13.25,11.3358 13.5858,11 14,11 Z M14,7 C14.5523,7 15,7.44772 15,8 C15,8.55228 14.5523,9 14,9 C13.4477,9 13,8.55228 13,8 C13,7.44772 13.4477,7 14,7 Z");

	/// <summary>
	/// Default path icon brush for the "Question" message box.
	/// </summary>
	public static Brush QuestionPathIconBrush { get; set; } = new SolidColorBrush(Color.FromRgb(64, 128, 255));

	/// <summary>
	/// Default path icon brush for the "Error" message box.
	/// </summary>
	public static Brush ErrorPathIconBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 64, 64));

	/// <summary>
	/// Default path icon brush for the "Warning" message box.
	/// </summary>
	public static Brush WarningPathIconBrush { get; set; } = new SolidColorBrush(Color.FromRgb(255, 128, 64));

	/// <summary>
	/// Default path icon brush for the "Information" message box.
	/// </summary>
	public static Brush InformationPathIconBrush { get; set; } = new SolidColorBrush(Color.FromRgb(64, 128, 255));

	/// <summary>
	/// Overrides the "Question" message icon with a custom one.
	/// </summary>
	public static ImageSource? QuestionIconOverride { get; set; }

	/// <summary>
	/// Overrides the "Error" message icon with a custom one.
	/// </summary>
	public static ImageSource? ErrorIconOverride { get; set; }

	/// <summary>
	/// Overrides the "Warning" message icon with a custom one.
	/// </summary>
	public static ImageSource? WarningIconOverride { get; set; }

	/// <summary>
	/// Overrides the "Information" message icon with a custom one.
	/// </summary>
	public static ImageSource? InformationIconOverride { get; set; }

	/// <summary>
	/// Default text for the "OK" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string OKText { get; set; } = "OK";

	/// <summary>
	/// Default text for the "Yes" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string YesText { get; set; } = "Yes";

	/// <summary>
	/// Default text for the "No" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string NoText { get; set; } = "No";

	/// <summary>
	/// Default text for the "Abort" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string AbortText { get; set; } = "Abort";

	/// <summary>
	/// Default text for the "Retry" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string RetryText { get; set; } = "Retry";

	/// <summary>
	/// Default text for the "Ignore" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string IgnoreText { get; set; } = "Ignore";

	/// <summary>
	/// Default text for the "Cancel" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string CancelText { get; set; } = "Cancel";

	/// <summary>
	/// Default text for the "Try Again" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string TryAgainText { get; set; } = "Try Again";

	/// <summary>
	/// Default text for the "Continue" button.
	/// <para>You can use this property for translation purposes.</para>
	/// </summary>
	public static string ContinueText { get; set; } = "Continue";

	/// <summary>
	/// Determines whether pop-up sounds should be played by default.
	/// </summary>
	public static bool PlaySoundsByDefault { get; set; } = true;

	/// <summary>
	/// Default padding of the Message Panel (Icon + Message).
	/// </summary>
	public static Thickness DefaultMessagePanelPadding { get; set; } = new(20);

	/// <summary>
	/// Default padding of the Buttons Panel.
	/// </summary>
	public static Thickness DefaultButtonsPanelPadding { get; set; } = new(12);

	/// <summary>
	/// Default background brush of the Buttons Panel.
	/// </summary>
	public static Brush DefaultButtonsPanelBackground { get; set; } = SystemColors.ControlBrush;

	/// <summary>
	/// Default padding of the <see cref="CMessageBox" /> window.
	/// </summary>
	public static Thickness DefaultPadding { get; set; } = new(0);

	/// <summary>
	/// Default window startup location for the <see cref="CMessageBox" />.
	/// </summary>
	public static WindowStartupLocation DefaultWindowStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;

	/// <summary>
	/// Determines whether the <see cref="CMessageBox" /> should be shown in the taskbar by default.
	/// </summary>
	public static bool ShowInTaskbarByDefault { get; set; } = false;

	/// <summary>
	/// Determines whether the <see cref="CMessageBox" /> should always be on top of other windows by default.
	/// </summary>
	public static bool TopmostByDefault { get; set; } = false;

	/// <summary>
	/// Determines whether the icon in the title bar should always be shown by default. The icon is inherited from the owner window.
	/// </summary>
	public static bool ShowTitleBarIconByDefault { get; set; } = false;

	/// <summary>
	/// Determines whether <c>&lt;Path&gt;</c> icons should always be used by default instead of the default system (or custom overridden) ones.
	/// </summary>
	public static bool UsePathIconsByDefault { get; set; } = false;

	/// <summary>
	/// Default maximum width of the icon which is shown inside the Message Panel.
	/// </summary>
	public static double DefaultMaxIconWidth { get; set; } = 32;

	/// <summary>
	/// Default maximum height of the icon which is shown inside the Message Panel.
	/// </summary>
	public static double DefaultMaxIconHeight { get; set; } = 32;

	/// <summary>
	/// Default minimum width of the generated buttons.
	/// </summary>
	public static double DefaultMinButtonWidth { get; set; } = 75;

	/// <summary>
	/// Default minimum height of the generated buttons.
	/// </summary>
	public static double DefaultMinButtonHeight { get; set; } = 23;

	/// <summary>
	/// Default spacing between the Icon and the Message inside the Message Panel.
	/// </summary>
	public static double DefaultIconToMessageSpacing { get; set; } = 12;

	/// <summary>
	/// Default spacing between each generated button.
	/// </summary>
	public static double DefaultButtonSpacing { get; set; } = 6;

	/// <summary>
	/// Default main orientation of the dialog. Change this if you want the buttons to be either at the <b>Bottom</b> or to the <b>Right</b> (<i>Left</i> in RTL).
	/// </summary>
	public static Orientation DefaultDialogContentOrientation { get; set; } = Orientation.Vertical;

	/// <summary>
	/// Default orientation of the Message Panel (Icon + Message). Change this if you want the Icon to be either above the Message (Vertical) or next to it (Horizontal).
	/// </summary>
	public static Orientation DefaultMessagePanelOrientation { get; set; } = Orientation.Horizontal;

	/// <summary>
	/// Default orientation of the Buttons Panel.
	/// </summary>
	public static Orientation DefaultButtonsPanelOrientation { get; set; } = Orientation.Horizontal;

	/// <summary>
	/// Default horizontal alignment of the Message Panel (Icon + Message).
	/// </summary>
	public static HorizontalAlignment DefaultHorizontalMessagePanelAlignment { get; set; } = HorizontalAlignment.Center;

	/// <summary>
	/// Default vertical alignment of the Message Panel (Icon + Message).
	/// </summary>
	public static VerticalAlignment DefaultVerticalMessagePanelAlignment { get; set; } = VerticalAlignment.Center;

	/// <summary>
	/// Default horizontal alignment of the Buttons Panel.
	/// </summary>
	public static HorizontalAlignment DefaultHorizontalButtonsPanelAlignment { get; set; } = HorizontalAlignment.Right;

	/// <summary>
	/// Default vertical alignment of the Buttons Panel.
	/// </summary>
	public static VerticalAlignment DefaultVerticalButtonsPanelAlignment { get; set; } = VerticalAlignment.Center;

	/// <summary>
	/// Default horizontal alignment of the Icon inside the Message Panel.
	/// </summary>
	public static HorizontalAlignment DefaultHorizontalIconAlignment { get; set; } = HorizontalAlignment.Center;

	/// <summary>
	/// Default vertical alignment of the Icon inside the Message Panel.
	/// </summary>
	public static VerticalAlignment DefaultVerticalIconAlignment { get; set; } = VerticalAlignment.Center;

	/// <summary>
	/// Default horizontal alignment of the Message Content inside the Message Panel.
	/// </summary>
	public static HorizontalAlignment DefaultHorizontalMessageAlignment { get; set; } = HorizontalAlignment.Center;

	/// <summary>
	/// Default vertical alignment of the Message Content inside the Message Panel.
	/// </summary>
	public static VerticalAlignment DefaultVerticalMessageAlignment { get; set; } = VerticalAlignment.Center;

	/// <summary>
	/// Default style for the <see cref="CMessageBox" /> window.
	/// </summary>
	public static Style? WindowStyleOverride { get; set; }

	/// <summary>
	/// Default style for the generated buttons inside the <see cref="CMessageBox" />.
	/// </summary>
	public static Style? ButtonStyleOverride { get; set; }

	#endregion Default (static) values

	#region Private property fields

	/// <summary>
	/// Cached icon to be used when the message box is loaded.
	/// </summary>
	private readonly CMessageBoxIcon? _cachedIcon;

	private Thickness _messagePanelPadding = DefaultMessagePanelPadding;
	private Thickness _buttonsPanelPadding = DefaultButtonsPanelPadding;
	private Brush _buttonsPanelBackground = DefaultButtonsPanelBackground;

	private double _maxIconWidth = DefaultMaxIconWidth;
	private double _maxIconHeight = DefaultMaxIconHeight;

	private double _minButtonWidth = DefaultMinButtonWidth;
	private double _minButtonHeight = DefaultMinButtonHeight;

	private double _iconToMessageSpacing = DefaultIconToMessageSpacing;
	private double _buttonSpacing = DefaultButtonSpacing;

	private Orientation _dialogContentOrientation = DefaultDialogContentOrientation;
	private Orientation _messagePanelOrientation = DefaultMessagePanelOrientation;
	private Orientation _buttonsPanelOrientation = DefaultButtonsPanelOrientation;

	private HorizontalAlignment _horizontalMessagePanelAlignment = DefaultHorizontalMessagePanelAlignment;
	private VerticalAlignment _verticalMessagePanelAlignment = DefaultVerticalMessagePanelAlignment;

	private HorizontalAlignment _horizontalButtonsPanelAlignment = DefaultHorizontalButtonsPanelAlignment;
	private VerticalAlignment _verticalButtonsPanelAlignment = DefaultVerticalButtonsPanelAlignment;

	private HorizontalAlignment _horizontalIconAlignment = DefaultHorizontalIconAlignment;
	private VerticalAlignment _verticalIconAlignment = DefaultVerticalIconAlignment;

	private HorizontalAlignment _horizontalMessageAlignment = DefaultHorizontalMessageAlignment;
	private VerticalAlignment _verticalMessageAlignment = DefaultVerticalMessageAlignment;

	#endregion Private property fields

	#region Public properties

	/// <inheritdoc />
	public event PropertyChangedEventHandler? PropertyChanged;

	private void RaisePropertyChanged(string propertyName)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	/// <summary>
	/// Determines whether the pop-up sound should be played.
	/// </summary>
	public bool PlaySound { get; set; } = PlaySoundsByDefault;

	/// <summary>
	/// Overrides the sound which should be played when the <see cref="CMessageBox" /> is shown.
	/// </summary>
	public SystemSound? SoundOverride { get; set; }

	/// <summary>
	/// Determines whether the icon in the title bar should be shown. The icon is inherited from the owner window.
	/// </summary>
	public bool ShowTitleBarIcon { get; set; } = ShowTitleBarIconByDefault;

	/// <summary>
	/// Determines whether <c>&lt;Path&gt;</c> icons should be used instead of the default system ones.
	/// </summary>
	public bool UsePathIcons { get; set; } = UsePathIconsByDefault;

	/// <summary>
	/// Padding of the Message Panel (Icon + Message).
	/// </summary>
	public Thickness MessagePanelPadding
	{
		get => _messagePanelPadding;
		set
		{
			_messagePanelPadding = value;
			RaisePropertyChanged(nameof(MessagePanelPadding));
		}
	}

	/// <summary>
	/// Padding of the Buttons Panel.
	/// </summary>
	public Thickness ButtonsPanelPadding
	{
		get => _buttonsPanelPadding;
		set
		{
			_buttonsPanelPadding = value;
			RaisePropertyChanged(nameof(ButtonsPanelPadding));
		}
	}

	/// <summary>
	/// Background brush of the Buttons Panel.
	/// </summary>
	public Brush ButtonsPanelBackground
	{
		get => _buttonsPanelBackground;
		set
		{
			_buttonsPanelBackground = value;
			RaisePropertyChanged(nameof(ButtonsPanelBackground));
		}
	}

	/// <summary>
	/// Maximum width of the icon which is shown inside the Message Panel.
	/// </summary>
	public double MaxIconWidth
	{
		get => _maxIconWidth;
		set
		{
			_maxIconWidth = value;
			RaisePropertyChanged(nameof(MaxIconWidth));
		}
	}

	/// <summary>
	/// Maximum height of the icon which is shown inside the Message Panel.
	/// </summary>
	public double MaxIconHeight
	{
		get => _maxIconHeight;
		set
		{
			_maxIconHeight = value;
			RaisePropertyChanged(nameof(MaxIconHeight));
		}
	}

	/// <summary>
	/// Minimum width of the generated buttons.
	/// </summary>
	public double MinButtonWidth
	{
		get => _minButtonWidth;
		set
		{
			_minButtonWidth = value;
			RaisePropertyChanged(nameof(MinButtonWidth));
		}
	}

	/// <summary>
	/// Minimum height of the generated buttons.
	/// </summary>
	public double MinButtonHeight
	{
		get => _minButtonHeight;
		set
		{
			_minButtonHeight = value;
			RaisePropertyChanged(nameof(MinButtonHeight));
		}
	}

	/// <summary>
	/// The spacing between the Icon and the Message inside the Message Panel.
	/// </summary>
	public double IconToMessageSpacing
	{
		get => _iconToMessageSpacing;
		set
		{
			_iconToMessageSpacing = value;
			RaisePropertyChanged(nameof(IconToMessageSpacing));
		}
	}

	/// <summary>
	/// The spacing between each generated button.
	/// </summary>
	public double ButtonSpacing
	{
		get => _buttonSpacing;
		set
		{
			_buttonSpacing = value;
			RaisePropertyChanged(nameof(ButtonSpacing));
		}
	}

	/// <summary>
	/// The main orientation of the dialog. Change this if you want the buttons to be either at the <b>Bottom</b> or to the <b>Right</b> (<i>Left</i> in RTL).
	/// </summary>
	public Orientation DialogContentOrientation
	{
		get => _dialogContentOrientation;
		set
		{
			_dialogContentOrientation = value;
			RaisePropertyChanged(nameof(DialogContentOrientation));
		}
	}

	/// <summary>
	/// The orientation of the Message Panel (Icon + Message). Change this if you want the Icon to be either above the Message (Vertical) or next to it (Horizontal).
	/// </summary>
	public Orientation MessagePanelOrientation
	{
		get => _messagePanelOrientation;
		set
		{
			_messagePanelOrientation = value;
			RaisePropertyChanged(nameof(MessagePanelOrientation));
		}
	}

	/// <summary>
	/// The orientation of the Buttons Panel.
	/// </summary>
	public Orientation ButtonsPanelOrientation
	{
		get => _buttonsPanelOrientation;
		set
		{
			_buttonsPanelOrientation = value;
			RaisePropertyChanged(nameof(ButtonsPanelOrientation));
		}
	}

	/// <summary>
	/// Horizontal alignment of the Message Panel (Icon + Message).
	/// </summary>
	public HorizontalAlignment HorizontalMessagePanelAlignment
	{
		get => _horizontalMessagePanelAlignment;
		set
		{
			_horizontalMessagePanelAlignment = value;
			RaisePropertyChanged(nameof(HorizontalMessagePanelAlignment));
		}
	}

	/// <summary>
	/// Vertical alignment of the Message Panel (Icon + Message).
	/// </summary>
	public VerticalAlignment VerticalMessagePanelAlignment
	{
		get => _verticalMessagePanelAlignment;
		set
		{
			_verticalMessagePanelAlignment = value;
			RaisePropertyChanged(nameof(VerticalMessagePanelAlignment));
		}
	}

	/// <summary>
	/// Horizontal alignment of the Buttons Panel.
	/// </summary>
	public HorizontalAlignment HorizontalButtonsPanelAlignment
	{
		get => _horizontalButtonsPanelAlignment;
		set
		{
			_horizontalButtonsPanelAlignment = value;
			RaisePropertyChanged(nameof(HorizontalButtonsPanelAlignment));
		}
	}

	/// <summary>
	/// Vertical alignment of the Buttons Panel.
	/// </summary>
	public VerticalAlignment VerticalButtonsPanelAlignment
	{
		get => _verticalButtonsPanelAlignment;
		set
		{
			_verticalButtonsPanelAlignment = value;
			RaisePropertyChanged(nameof(VerticalButtonsPanelAlignment));
		}
	}

	/// <summary>
	/// Horizontal alignment of the Icon inside the Message Panel.
	/// </summary>
	public HorizontalAlignment HorizontalIconAlignment
	{
		get => _horizontalIconAlignment;
		set
		{
			_horizontalIconAlignment = value;
			RaisePropertyChanged(nameof(HorizontalIconAlignment));
		}
	}

	/// <summary>
	/// Vertical alignment of the Icon inside the Message Panel.
	/// </summary>
	public VerticalAlignment VerticalIconAlignment
	{
		get => _verticalIconAlignment;
		set
		{
			_verticalIconAlignment = value;
			RaisePropertyChanged(nameof(VerticalIconAlignment));
		}
	}

	/// <summary>
	/// Horizontal alignment of the Message Content inside the Message Panel.
	/// </summary>
	public HorizontalAlignment HorizontalMessageAlignment
	{
		get => _horizontalMessageAlignment;
		set
		{
			_horizontalMessageAlignment = value;
			RaisePropertyChanged(nameof(HorizontalMessageAlignment));
		}
	}

	/// <summary>
	/// Vertical alignment of the Message Content inside the Message Panel.
	/// </summary>
	public VerticalAlignment VerticalMessageAlignment
	{
		get => _verticalMessageAlignment;
		set
		{
			_verticalMessageAlignment = value;
			RaisePropertyChanged(nameof(VerticalMessageAlignment));
		}
	}

	#endregion Public properties

	#region Construction

	private CMessageBox()
	{
		InitializeComponent();

		if (WindowStyleOverride is not null)
			Style = WindowStyleOverride;

		Padding = DefaultPadding;
		WindowStartupLocation = DefaultWindowStartupLocation;
		ShowInTaskbar = ShowInTaskbarByDefault;
		Topmost = TopmostByDefault;

		Loaded += CMessageBox_Loaded;
		IsVisibleChanged += CMessageBox_IsVisibleChanged;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CMessageBox"/> class with the specified message and caption.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	public CMessageBox(object message, string? caption = null) : this()
	{
		Title = caption ?? string.Empty;

		SetMessageContent(message);
		SetMessageIcon(null);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CMessageBox"/> class with the specified message, caption, and icon.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">Determines the preset icon which should appear inside the Message Panel.</param>
	public CMessageBox(object message, string? caption = null, CMessageBoxIcon icon = CMessageBoxIcon.None) : this(message, caption)
		=> _cachedIcon = icon;

	/// <summary>
	/// Initializes a new instance of the <see cref="CMessageBox"/> class with the specified message, caption, and custom icon.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">The custom icon which should appear inside the Message Panel.</param>
	public CMessageBox(object message, string? caption = null, ImageSource? icon = null) : this(message, caption)
		=> SetMessageIcon(icon);

	#endregion Construction

	#region Show()

	/// <summary>
	/// Shows the message box with the specified buttons.
	/// <para>The owner of the dialog is determined by the currently active Window or the MainWindow.</para>
	/// </summary>
	/// <typeparam name="TResult">The type of result to be returned by the message box.</typeparam>
	/// <param name="buttons">An array of buttons to be displayed in the message box.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public TResult Show<TResult>(params CMessageBoxButton<TResult>[] buttons) where TResult : struct
		=> Show(WindowHelper.FindViableOwner(), buttons);

	/// <summary>
	/// Shows the message box with the specified buttons.
	/// </summary>
	/// <typeparam name="TResult">The type of result to be returned by the message box.</typeparam>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="buttons">An array of buttons to be displayed in the message box.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public TResult Show<TResult>(Window? owner, params CMessageBoxButton<TResult>[] buttons) where TResult : struct
	{
		Icon = owner?.Icon;

		var buttonPanel = (StackPanel)FindName("PART_ButtonsPanel");
		TResult result = default;

		void AddButton(CMessageBoxButton<TResult> messageBoxButton)
		{
			var button = new Button
			{
				MinWidth = MinButtonWidth,
				MinHeight = MinButtonHeight,
				Content = messageBoxButton.Content,
				IsDefault = messageBoxButton.SpecialRole == CSpecialButtonRole.IsDefault,
				IsCancel = messageBoxButton.SpecialRole == CSpecialButtonRole.IsCancel
			};

			if (messageBoxButton.StyleKey is not null && TryFindResource(messageBoxButton.StyleKey) is Style givenStyle)
				button.Style = givenStyle;
			else if (ButtonStyleOverride is not null)
				button.Style = ButtonStyleOverride;
			else if (TryFindResource(BUTTON_STYLE_KEY) is Style overrideStyle)
				button.Style = overrideStyle;

			button.Click += delegate
			{
				result = messageBoxButton.Result;
				Close();
			};

			buttonPanel.Children.Add(button);
		}

		Array.ForEach(buttons, AddButton);

		if (owner is not null)
			Owner = owner;
		else
			WindowStartupLocation = WindowStartupLocation.CenterScreen;

		ShowDialog();
		return result;
	}

	/// <summary>
	/// Shows the message box with predefined buttons.
	/// <para>The owner of the dialog is determined by the currently active Window or the MainWindow.</para>
	/// </summary>
	/// <param name="buttons">The buttons to be displayed in the message box.</param>
	/// <param name="defaultButton">The button that should be the default (focused) button.</param>
	/// <returns>The result of the message box.</returns>
	public CMessageBoxResult Show(CMessageBoxButtons buttons,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(WindowHelper.FindViableOwner(), buttons, defaultButton);

	/// <summary>
	/// Shows the message box with predefined buttons.
	/// </summary>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="buttons">The buttons to be displayed in the message box.</param>
	/// <param name="defaultButton">The button that should be the default (focused) button.</param>
	/// <returns>The result of the message box.</returns>
	public CMessageBoxResult Show(Window? owner, CMessageBoxButtons buttons,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(owner, CreatePresetButtons(buttons, defaultButton));

	#endregion Show()

	#region Static Show()

	/// <summary>
	/// The classic way of showing a message box.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="buttons">An enum of preset buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <param name="icon">Determines the preset icon which should appear inside the Message Panel.</param>
	/// <param name="defaultButton">Determines which <see cref="Button" /> should have its <c>IsDefault</c> value set to <see langword="true" />.</param>
	/// <returns>A <see cref="CMessageBoxResult"/>.</returns>
	public static CMessageBoxResult Show(object message, string? caption = null,
		CMessageBoxButtons buttons = CMessageBoxButtons.OK, CMessageBoxIcon icon = CMessageBoxIcon.None,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(WindowHelper.FindViableOwner(), message, caption, buttons, icon, defaultButton);

	/// <summary>
	/// The classic way of showing a message box.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="buttons">An enum of preset buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <param name="icon">The custom icon which should appear inside the Message Panel.</param>
	/// <param name="defaultButton">Determines which <see cref="Button" /> should have its <c>IsDefault</c> value set to <see langword="true" />.</param>
	/// <returns>A <see cref="CMessageBoxResult"/>.</returns>
	public static CMessageBoxResult Show(object message, string? caption,
		CMessageBoxButtons buttons, ImageSource? icon,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(WindowHelper.FindViableOwner(), message, caption, buttons, icon, defaultButton);

	/// <summary>
	/// Opens a message box with the specified content.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">Determines the preset icon which should appear inside the Message Panel.</param>
	/// <param name="buttons">A collection of custom buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public static TResult Show<TResult>(object message, string? caption = null,
		CMessageBoxIcon icon = CMessageBoxIcon.None, params CMessageBoxButton<TResult>[] buttons) where TResult : struct
		=> Show(WindowHelper.FindViableOwner(), message, caption, icon, buttons);

	/// <summary>
	/// Opens a message box with the specified custom icon and message.
	/// </summary>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">The custom icon which should appear inside the Message Panel.</param>
	/// <param name="buttons">A collection of custom buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public static TResult Show<TResult>(object message, string? caption,
		ImageSource? icon, params CMessageBoxButton<TResult>[] buttons) where TResult : struct
		=> Show(WindowHelper.FindViableOwner(), message, caption, icon, buttons);

	/// <summary>
	/// The classic way of showing a message box.
	/// </summary>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="buttons">An enum of preset buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <param name="icon">Determines the preset icon which should appear inside the Message Panel.</param>
	/// <param name="defaultButton">Determines which <see cref="Button" /> should have its <c>IsDefault</c> value set to <see langword="true" />.</param>
	/// <returns>A <see cref="CMessageBoxResult"/>.</returns>
	public static CMessageBoxResult Show(Window? owner, object message, string? caption = null,
		CMessageBoxButtons buttons = CMessageBoxButtons.OK, CMessageBoxIcon icon = CMessageBoxIcon.None,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(owner, message, caption, icon, CreatePresetButtons(buttons, defaultButton));

	/// <summary>
	/// The classic way of showing a message box.
	/// </summary>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="buttons">An enum of preset buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <param name="icon">The custom icon which should appear inside the Message Panel.</param>
	/// <param name="defaultButton">Determines which <see cref="Button" /> should have its <c>IsDefault</c> value set to <see langword="true" />.</param>
	/// <returns>A <see cref="CMessageBoxResult"/>.</returns>
	public static CMessageBoxResult Show(Window? owner, object message, string? caption,
		CMessageBoxButtons buttons, ImageSource? icon,
		CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
		=> Show(owner, message, caption, icon, CreatePresetButtons(buttons, defaultButton));

	/// <summary>
	/// Opens a message box with the specified content.
	/// </summary>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">Determines the preset icon which should appear inside the Message Panel.</param>
	/// <param name="buttons">A collection of custom buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public static TResult Show<TResult>(Window? owner, object message, string? caption = null,
		CMessageBoxIcon icon = CMessageBoxIcon.None, params CMessageBoxButton<TResult>[] buttons) where TResult : struct
	{
		var box = new CMessageBox(message, caption, icon);
		return box.Show(owner, buttons);
	}

	/// <summary>
	/// Opens a message box with the specified custom icon and message.
	/// </summary>
	/// <param name="owner">The window that owns this message box.</param>
	/// <param name="message">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	/// <param name="caption">The title of the <see cref="CMessageBox" /> dialog.</param>
	/// <param name="icon">The custom icon which should appear inside the Message Panel.</param>
	/// <param name="buttons">A collection of custom buttons which should appear in the <see cref="CMessageBox" />.</param>
	/// <returns>The result associated with the clicked button.</returns>
	public static TResult Show<TResult>(Window? owner, object message, string? caption,
		ImageSource? icon, params CMessageBoxButton<TResult>[] buttons) where TResult : struct
	{
		var box = new CMessageBox(message, caption, icon);
		return box.Show(owner, buttons);
	}

	#endregion Static Show()

	#region Other methods

	private void CMessageBox_Loaded(object sender, RoutedEventArgs e)
	{
		if (_cachedIcon is not null)
			SetMessageIcon(_cachedIcon.Value);
	}

	private void CMessageBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		IsVisibleChanged -= CMessageBox_IsVisibleChanged;

		if (!ShowTitleBarIcon)
			this.HideIcon();

		SizeToContent = SizeToContent.WidthAndHeight;
		// SizeToContent needs to be set later, otherwise custom window styles will be messed up

		if (PlaySound)
			TriggerSound();
	}

	private void TriggerSound()
	{
		if (SoundOverride is not null)
		{
			SoundOverride.Play();
			return;
		}

		switch (_cachedIcon)
		{
			case CMessageBoxIcon.Error: SystemSounds.Hand.Play(); break;
			case CMessageBoxIcon.Question: SystemSounds.Question.Play(); break;
			case CMessageBoxIcon.Warning: SystemSounds.Exclamation.Play(); break;
			case CMessageBoxIcon.Information: SystemSounds.Asterisk.Play(); break;
		}
	}

	/// <summary>
	/// Sets the preset icon which should be shown inside the Message Panel.
	/// </summary>
	/// <param name="icon">The preset icon to display.</param>
	public void SetMessageIcon(CMessageBoxIcon icon)
	{
		if (FindName("PART_IconGrid") is Grid grid)
			grid.Visibility = icon == CMessageBoxIcon.None ? Visibility.Collapsed : Visibility.Visible;

		if (icon == CMessageBoxIcon.None)
			return;

		if (UsePathIcons)
			SetPathIcon(icon);
		else
			SetDefaultSystemIcon(icon);
	}

	private void SetDefaultSystemIcon(CMessageBoxIcon icon)
	{
		if (icon is CMessageBoxIcon.Error && ErrorIconOverride is not null)
			SetMessageIcon(ErrorIconOverride);
		else if (icon is CMessageBoxIcon.Question && QuestionIconOverride is not null)
			SetMessageIcon(QuestionIconOverride);
		else if (icon is CMessageBoxIcon.Warning && WarningIconOverride is not null)
			SetMessageIcon(WarningIconOverride);
		else if (icon is CMessageBoxIcon.Information && InformationIconOverride is not null)
			SetMessageIcon(InformationIconOverride);
		else
		{
			IntPtr iconPtr;

			if (NativeMethods.IsComCtl32Version6OrGreater())
				NativeMethods.LoadModernSystemIcon((int)icon, (int)MaxIconWidth, (int)MaxIconHeight, out iconPtr);
			else
				iconPtr = NativeMethods.LoadSystemIcon((int)icon);

			BitmapSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
				iconPtr,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions());

			NativeMethods.DestroyIcon(iconPtr);
			SetMessageIcon(imageSource);
		}
	}

	private void SetPathIcon(CMessageBoxIcon icon)
	{
		if (FindName("PART_ImageIcon") is Image image)
			image.Visibility = Visibility.Collapsed;

		if (FindName("PART_PathIcon") is Path pathIcon)
		{
			pathIcon.Visibility = Visibility.Visible;

			pathIcon.Data = icon switch
			{
				CMessageBoxIcon.Question => QuestionPathIconGeometry,
				CMessageBoxIcon.Error => ErrorPathIconGeometry,
				CMessageBoxIcon.Warning => WarningPathIconGeometry,
				CMessageBoxIcon.Information => InformationPathIconGeometry,
				_ => Geometry.Parse(string.Empty)
			};

			pathIcon.Fill = icon switch
			{
				CMessageBoxIcon.Question => QuestionPathIconBrush,
				CMessageBoxIcon.Error => ErrorPathIconBrush,
				CMessageBoxIcon.Warning => WarningPathIconBrush,
				CMessageBoxIcon.Information => InformationPathIconBrush,
				_ => Brushes.Transparent
			};
		}
	}

	/// <summary>
	/// Sets a custom icon which should be shown inside the Message Panel.
	/// </summary>
	/// <param name="icon">The custom icon image to display.</param>
	public void SetMessageIcon(ImageSource? icon)
	{
		if (FindName("PART_PathIcon") is Path pathIcon)
			pathIcon.Visibility = Visibility.Collapsed;

		if (FindName("PART_ImageIcon") is Image image)
		{
			image.Visibility = Visibility.Visible;
			image.Source = icon;
		}
	}

	/// <summary>
	/// Sets the custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.
	/// </summary>
	/// <param name="content">Custom content of the <see cref="CMessageBox" />. This doesn't have to be just text, but may also be a custom control.</param>
	public void SetMessageContent(object content)
	{
		if (FindName("PART_ContentPresenter") is ContentPresenter contentPresenter)
			contentPresenter.Content = content;
	}

	private static CMessageBoxButton<CMessageBoxResult>[] CreatePresetButtons(CMessageBoxButtons buttons, CMessageBoxDefaultButton defaultButton = CMessageBoxDefaultButton.None)
	{
		var buttonList = new List<CMessageBoxButton<CMessageBoxResult>>();

		void AddButtonToList(CMessageBoxButton<CMessageBoxResult> messageBoxButton)
		{
			if (buttonList.Count == (int)defaultButton)
				messageBoxButton.SpecialRole = CSpecialButtonRole.IsDefault;

			buttonList.Add(messageBoxButton);
		}

		if (buttons is CMessageBoxButtons.OK or CMessageBoxButtons.OKCancel)
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(OKText, CMessageBoxResult.OK));

		if (buttons is CMessageBoxButtons.YesNo or CMessageBoxButtons.YesNoCancel)
		{
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(YesText, CMessageBoxResult.Yes));
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(NoText, CMessageBoxResult.No));
		}

		if (buttons is CMessageBoxButtons.AbortRetryIgnore)
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(AbortText, CMessageBoxResult.Abort));

		if (buttons is CMessageBoxButtons.AbortRetryIgnore or CMessageBoxButtons.RetryCancel)
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(RetryText, CMessageBoxResult.Retry));

		if (buttons is CMessageBoxButtons.AbortRetryIgnore)
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(IgnoreText, CMessageBoxResult.Ignore));

		if (buttons is CMessageBoxButtons.OKCancel or
			CMessageBoxButtons.YesNoCancel or
			CMessageBoxButtons.RetryCancel or
			CMessageBoxButtons.CancelTryContinue)
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(CancelText, CMessageBoxResult.Cancel, CSpecialButtonRole.IsCancel));

		if (buttons is CMessageBoxButtons.CancelTryContinue)
		{
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(TryAgainText, CMessageBoxResult.TryAgain));
			AddButtonToList(new CMessageBoxButton<CMessageBoxResult>(ContinueText, CMessageBoxResult.Continue));
		}

		return buttonList.ToArray();
	}

	#endregion Other methods
}
