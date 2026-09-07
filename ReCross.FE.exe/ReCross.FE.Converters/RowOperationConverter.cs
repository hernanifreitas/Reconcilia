using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReCross.FE.Converters;

public class RowOperationConverter : IValueConverter
{
	private static RowOperationConverter _instance = new RowOperationConverter();

	public static RowOperationConverter Instance => _instance;

	private RowOperationConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = null;
		if (value.ToString().IndexOf("LV") >= 0)
		{
			text = value.ToString().Replace("LV", "").ToLower();
		}
		else if (value.ToString().IndexOf("B") >= 0)
		{
			text = value.ToString().Replace("B", "").ToLower();
		}
		string[] array = text.Split('_');
		bool flag = true;
		string[] array2 = array;
		foreach (string key in array2)
		{
			if (ReCrossApp.Current.UserGenericPermissions.ContainsKey(key))
			{
				switch (parameter.ToString())
				{
				case "Create":
					flag = flag && ReCrossApp.Current.UserGenericPermissions[key].CanCreate;
					break;
				case "Read":
					flag = flag && ReCrossApp.Current.UserGenericPermissions[key].CanRead;
					break;
				case "Update":
					flag = flag && ReCrossApp.Current.UserGenericPermissions[key].CanUpdate;
					break;
				case "Delete":
					flag = flag && ReCrossApp.Current.UserGenericPermissions[key].CanDelete;
					break;
				}
			}
		}
		return (!flag) ? Visibility.Hidden : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("Nope.");
	}
}
