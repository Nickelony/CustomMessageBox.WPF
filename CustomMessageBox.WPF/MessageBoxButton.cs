using System.Windows;

namespace CustomMessageBox.WPF;

public struct MessageBoxButton<TResult> where TResult : struct
{
	public object Content { get; set; }
	public TResult Result { get; set; }
	public SpecialButtonRole SpecialRole { get; set; }
	public Style? Style { get; set; }

	public MessageBoxButton(object content, TResult result, SpecialButtonRole specialRole = SpecialButtonRole.None, Style? style = null)
	{
		Content = content;
		Result = result;
		SpecialRole = specialRole;
		Style = style;
	}
}
