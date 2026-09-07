using System;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class GroupRolesConverter : IMultiValueConverter
{
	private static GroupRolesConverter _instance = new GroupRolesConverter();

	public static GroupRolesConverter Instance => _instance;

	private GroupRolesConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		GroupRolesViewModel groupRolesViewModel = (GroupRolesViewModel)parameter;
		try
		{
			if (values != null)
			{
				if (values[0] is Group)
				{
					EntityCollection<Role> roles = ((Group)values[0]).Roles;
					groupRolesViewModel.SetGroupRoles((roles != null) ? roles : null);
				}
				else
				{
					groupRolesViewModel.SetGroupRoles(null);
				}
			}
			else
			{
				groupRolesViewModel.SetGroupRoles(null);
			}
		}
		catch
		{
		}
		return groupRolesViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
