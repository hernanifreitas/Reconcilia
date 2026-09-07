using System;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class UserCollConverter : IMultiValueConverter
{
	private static UserCollConverter _instance = new UserCollConverter();

	public static UserCollConverter Instance => _instance;

	private UserCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		UserViewModel userViewModel = (UserViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			userViewModel.SetCollection(CompiledQueries.GetUserCollection(ReCrossApp.Current.CurrentDataContext));
		}
		return userViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
