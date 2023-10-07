namespace CustomMessageBox.WPF;

public struct CMessageBoxButton<TResult> where TResult : struct
{
	public object Content { get; set; }
	public TResult Result { get; set; }
	public CSpecialButtonRole SpecialRole { get; set; }
	public object? StyleKey { get; set; }

	public CMessageBoxButton(object content, TResult result, CSpecialButtonRole specialRole = CSpecialButtonRole.None, object? styleKey = null)
	{
		Content = content;
		Result = result;
		SpecialRole = specialRole;
		StyleKey = styleKey;
	}
}
