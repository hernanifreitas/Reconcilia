using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ReCross.FE.Properties;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class ReconciliationClosedAutoConverter : IValueConverter
{
	private static ReconciliationClosedAutoConverter _instance = new ReconciliationClosedAutoConverter();

	public static ReconciliationClosedAutoConverter Instance => _instance;

	private ReconciliationClosedAutoConverter()
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
		return (flag || !Settings.Default.AutoLinkEnabled || !ReCrossApp.Current.UserGenericPermissions[OperationEnum.AutoLink.ToString().ToLower()].CanRead) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
