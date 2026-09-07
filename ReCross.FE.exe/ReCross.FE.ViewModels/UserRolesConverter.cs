using System;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class UserRolesConverter : IMultiValueConverter
{
	private static UserRolesConverter _instance = new UserRolesConverter();

	public static UserRolesConverter Instance => _instance;

	private UserRolesConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		UserRolesViewModel userRolesViewModel = (UserRolesViewModel)parameter;
		try
		{
			if (values != null)
			{
				if (values[0] is User)
				{
					EntityCollection<Role> roles = ((User)values[0]).Roles;
					userRolesViewModel.SetUserRoles((roles != null) ? roles : null);
				}
				else
				{
					userRolesViewModel.SetUserRoles(null);
				}
			}
			else
			{
				userRolesViewModel.SetUserRoles(null);
			}
		}
		catch
		{
		}
		return userRolesViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
