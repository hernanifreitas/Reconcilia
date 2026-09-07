using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Strills.WPF.Security;

namespace ReCross.FE.Converters;

public class ItemVisibilityConverter : IValueConverter
{
	private static ItemVisibilityConverter _instance = new ItemVisibilityConverter();

	public static ItemVisibilityConverter Instance => _instance;

	private ItemVisibilityConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (!((Dictionary<string, Permission>)value).ContainsKey(parameter.ToString().ToLower()) || !((Dictionary<string, Permission>)value)[parameter.ToString().ToLower()].CanRead) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("Nope.");
	}
}
