using System;
using System.Globalization;
using System.Windows.Data;

namespace ReCross.FE.ViewModels;

public class BothMovementConverter : IMultiValueConverter
{
	private static BothMovementConverter _instance = new BothMovementConverter();

	public static BothMovementConverter Instance => _instance;

	private BothMovementConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		SearchViewModel searchViewModel = (SearchViewModel)parameter;
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
