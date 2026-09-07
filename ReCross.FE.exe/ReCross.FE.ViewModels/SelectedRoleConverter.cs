using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class SelectedRoleConverter : IMultiValueConverter
{
	private static SelectedRoleConverter _instance = new SelectedRoleConverter();

	public static SelectedRoleConverter Instance => _instance;

	private SelectedRoleConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		IEntityRoles entityRoles = (IEntityRoles)parameter;
		Role result = null;
		try
		{
			if (values != null && values.Count() > 0)
			{
				if (values[0] is Role)
				{
					result = (Role)values[0];
				}
				else if (values[0] is int)
				{
					result = (Role)entityRoles.CurrentCollection.GetItemAt((int)values[0]);
				}
			}
		}
		catch
		{
		}
		return result;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		IEntityRoles entityRoles = (IEntityRoles)parameter;
		int num = 0;
		try
		{
			if (value != null)
			{
				num = entityRoles.TradeRole((Role)value);
			}
		}
		catch
		{
		}
		return new object[2]
		{
			num,
			(Role)value
		};
	}
}
