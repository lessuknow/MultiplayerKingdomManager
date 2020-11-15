using UnityEngine;

public static class debug
{
	// A class to hold various debug util functions.

	// XXX : some of these are redundant, but we may want different or extra
	//		handling for warnings errors in the future. Plus, same format for
	//		all prints is nice.

	/* Example Functions */

	/// <summary>
	/// This example function takes your text, ignores it, and prints "Hello World!".
	/// </summary>
	/// <param name="text">Text that this function will completely ignore.</param>
	public static void example_func(string text)
	{
		debug.print_line("Hello World!");
	}

	/* Log Functions */

	/// <summary>
	/// Log console message with `text`.
	/// </summary>
	public static void print_line(string text)
	{
		Debug.Log(text);
	}

	/// <summary>
	/// Log console message with `text` if `value` is true.
	/// </summary>
	public static void print_line_if(bool value, string text)
	{
		if (value)
		{
			Debug.Log(text);
		}
	}

	/// <summary>
	/// Log warning with `text`.
	/// </summary>
	public static void print_warning(string text)
	{
		Debug.LogWarning(text);
	}

	/// <summary>
	/// Log warning with `text` if `value` is true.
	/// </summary>
	public static void print_warning_if(bool value, string text)
	{
		if (value)
		{
			Debug.LogWarning(text);
		}
	}

	/// <summary>
	/// Log error with `text`.
	/// </summary>
	public static void print_error(string text)
	{
		Debug.LogError(text);
	}

	/// <summary>
	/// Log error with `text` if `value` is true.
	/// </summary>
	public static void print_error_if(bool value, string text)
	{
		if (value)
		{
			Debug.LogError(text);
		}
	}
}
