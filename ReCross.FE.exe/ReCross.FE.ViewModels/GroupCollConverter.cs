using System;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class GroupCollConverter : IMultiValueConverter
{
	private static GroupCollConverter _instance = new GroupCollConverter();

	public static GroupCollConverter Instance => _instance;

	private GroupCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		GroupViewModel groupViewModel = (GroupViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			groupViewModel.SetCollection(CompiledQueries.GetGroupCollection(ReCrossApp.Current.CurrentDataContext));
		}
		return groupViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
