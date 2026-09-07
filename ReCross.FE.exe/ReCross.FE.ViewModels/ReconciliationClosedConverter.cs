using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class ReconciliationClosedConverter : IValueConverter
{
	private static ReconciliationClosedConverter _instance = new ReconciliationClosedConverter();

	public static ReconciliationClosedConverter Instance => _instance;

	private ReconciliationClosedConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		bool flag = true;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected)
		{
			flag = viewModel.CurrentResult != null && viewModel.CurrentResult.Closed;
		}
		return (flag || !ReCrossApp.Current.UserGenericPermissions[OperationEnum.LinkUnlink.ToString().ToLower()].CanRead) ? Visibility.Hidden : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
