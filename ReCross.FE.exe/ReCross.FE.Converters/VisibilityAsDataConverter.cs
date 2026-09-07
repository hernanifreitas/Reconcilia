using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ReCross.FE.Utils;

namespace ReCross.FE.Converters;

public class VisibilityAsDataConverter : IValueConverter
{
	private static VisibilityAsDataConverter _instance = new VisibilityAsDataConverter();

	public static VisibilityAsDataConverter Instance => _instance;

	private VisibilityAsDataConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter != null && parameter.ToString() == "Visible")
		{
			return (!(bool)value) ? Visibility.Hidden : Visibility.Visible;
		}
		if (parameter != null && parameter.ToString() == OperationEnum.Import.ToString())
		{
			return (!(bool)value || !ReCrossApp.Current.UserGenericPermissions[parameter.ToString().ToLower()].CanRead) ? Visibility.Hidden : Visibility.Visible;
		}
		return ((bool)value) ? Visibility.Hidden : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("Nope.");
	}
}
